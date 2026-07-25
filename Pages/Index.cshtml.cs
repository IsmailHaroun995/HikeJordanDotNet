using HikeJordanDotNet.Data;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class IndexModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public IReadOnlyList<Post> Posts { get; private set; } = [];
    public HashSet<int> LikedPostIds { get; private set; } = [];
    public IReadOnlyList<AppUser> SuggestedUsers { get; private set; } = [];
    public string Tab { get; private set; } = "latest";
    public int PostCount { get; private set; }
    public int MemberCount { get; private set; }

    public async Task OnGetAsync(string? tab = null)
    {
        Tab = tab == "following" && CurrentUserId is not null ? "following" : "latest";

        var query = Db.Posts
            .Where(p => !p.IsHidden)
            .Include(p => p.Author)
            .AsQueryable();

        if (Tab == "following" && CurrentUserId is int uid)
        {
            var followingIds = await Db.Follows
                .Where(f => f.FollowerId == uid)
                .Select(f => f.FollowingId)
                .ToListAsync();
            query = query.Where(p => followingIds.Contains(p.AuthorId));
        }

        Posts = await query
            .OrderByDescending(p => p.CreatedAtUtc)
            .Take(50)
            .ToListAsync();

        if (CurrentUserId is int currentUid)
        {
            LikedPostIds = await SocialOps.LikedPostIdsAsync(Db, currentUid, Posts.Select(p => p.Id));

            var followingIds = await Db.Follows
                .Where(f => f.FollowerId == currentUid)
                .Select(f => f.FollowingId)
                .ToListAsync();

            SuggestedUsers = await Db.Users
                .Where(u => u.Id != currentUid && !followingIds.Contains(u.Id))
                .OrderByDescending(u => u.Posts.Count)
                .Take(4)
                .ToListAsync();
        }

        PostCount = await Db.Posts.CountAsync(p => !p.IsHidden);
        MemberCount = await Db.Users.CountAsync();
    }
}
