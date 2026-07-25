using HikeJordanDotNet.Data;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class ExploreModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public IReadOnlyList<Post> Posts { get; private set; } = [];
    public HashSet<int> LikedPostIds { get; private set; } = [];
    public IReadOnlyList<AppUser> People { get; private set; } = [];
    public string? Region { get; private set; }
    public string? Query { get; private set; }

    public async Task OnGetAsync(string? region = null, string? q = null)
    {
        Region = region;
        Query = q;

        var posts = Db.Posts
            .Where(p => !p.IsHidden)
            .Include(p => p.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(region))
            posts = posts.Where(p => p.Region == region);

        if (!string.IsNullOrWhiteSpace(q))
            posts = posts.Where(p => p.Body.Contains(q) || p.LocationName.Contains(q));

        Posts = await posts
            .OrderByDescending(p => p.CreatedAtUtc)
            .Take(60)
            .ToListAsync();

        if (CurrentUserId is int uid)
            LikedPostIds = await SocialOps.LikedPostIdsAsync(Db, uid, Posts.Select(p => p.Id));

        var people = Db.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            people = people.Where(u => u.Name.Contains(q) || u.Username.Contains(q));

        People = await people
            .OrderByDescending(u => u.Posts.Count)
            .Take(6)
            .ToListAsync();
    }
}
