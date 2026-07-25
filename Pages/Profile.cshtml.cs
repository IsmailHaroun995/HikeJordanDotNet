using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class ProfileModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public AppUser Profile { get; private set; } = null!;
    public IReadOnlyList<Post> Posts { get; private set; } = [];
    public HashSet<int> LikedPostIds { get; private set; } = [];
    public int FollowerCount { get; private set; }
    public int FollowingCount { get; private set; }
    public bool IsFollowing { get; private set; }
    public bool IsSelf { get; private set; }

    public async Task<IActionResult> OnGetAsync(string username)
    {
        var user = await Db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return NotFound();

        Profile = user;

        Posts = await Db.Posts
            .Where(p => p.AuthorId == user.Id && !p.IsHidden)
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync();

        FollowerCount = await Db.Follows.CountAsync(f => f.FollowingId == user.Id);
        FollowingCount = await Db.Follows.CountAsync(f => f.FollowerId == user.Id);

        if (CurrentUserId is int uid)
        {
            IsSelf = uid == user.Id;
            IsFollowing = await Db.Follows.AnyAsync(f => f.FollowerId == uid && f.FollowingId == user.Id);
            LikedPostIds = await SocialOps.LikedPostIdsAsync(Db, uid, Posts.Select(p => p.Id));
        }

        return Page();
    }
}
