using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class ExploreModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public record RegionCount(string Region, int Count);

    public IReadOnlyList<Post> Posts { get; private set; } = [];
    public HashSet<int> LikedPostIds { get; private set; } = [];
    public IReadOnlyList<AppUser> People { get; private set; } = [];
    public IReadOnlyList<AppUser> SuggestedUsers { get; private set; } = [];
    public IReadOnlyList<RegionCount> TrendingRegions { get; private set; } = [];
    public string? Region { get; private set; }
    public string? Query { get; private set; }
    public string Tab { get; private set; } = "latest";

    // Onboarding checklist (logged-in only)
    public bool ShowOnboarding { get; private set; }
    public bool StepPhoto { get; private set; }
    public bool StepBio { get; private set; }
    public bool StepPost { get; private set; }
    public bool StepFollow { get; private set; }

    public async Task OnGetAsync(string? region = null, string? q = null, string? tab = null)
    {
        Region = region;
        Query = q;
        Tab = tab == "following" && CurrentUserId is not null ? "following" : "latest";

        var posts = Db.Posts.Where(p => !p.IsHidden).Include(p => p.Author).AsQueryable();

        List<int> following = [];
        if (CurrentUserId is int fid)
        {
            following = await Db.Follows.Where(f => f.FollowerId == fid).Select(f => f.FollowingId).ToListAsync();
            if (Tab == "following")
                posts = posts.Where(p => following.Contains(p.AuthorId));
        }

        if (!string.IsNullOrWhiteSpace(region))
            posts = posts.Where(p => p.Region == region);

        if (!string.IsNullOrWhiteSpace(q))
            posts = posts.Where(p => p.Body.Contains(q) || p.LocationName.Contains(q));

        Posts = await posts.OrderByDescending(p => p.CreatedAtUtc).Take(60).ToListAsync();

        if (CurrentUserId is int uid)
        {
            LikedPostIds = await SocialOps.LikedPostIdsAsync(Db, uid, Posts.Select(p => p.Id));

            SuggestedUsers = await Db.Users
                .Where(u => u.Id != uid && !following.Contains(u.Id))
                .OrderByDescending(u => u.Posts.Count)
                .Take(5)
                .ToListAsync();

            var me = await Db.Users.FirstAsync(u => u.Id == uid);
            StepPhoto = !string.IsNullOrEmpty(me.AvatarUrl);
            StepBio = !string.IsNullOrWhiteSpace(me.Bio);
            StepPost = await Db.Posts.AnyAsync(p => p.AuthorId == uid);
            StepFollow = following.Count >= 3;
            var dismissed = Request.Cookies.ContainsKey("hj_onboard_done");
            ShowOnboarding = !dismissed && !(StepPhoto && StepBio && StepPost && StepFollow);
        }
        else
        {
            SuggestedUsers = await Db.Users
                .OrderByDescending(u => u.Posts.Count)
                .Take(5)
                .ToListAsync();
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            People = await Db.Users
                .Where(u => u.Name.Contains(q) || u.Username.Contains(q))
                .OrderByDescending(u => u.Posts.Count)
                .Take(6)
                .ToListAsync();
        }

        var trending = await Db.Posts
            .Where(p => !p.IsHidden && p.Region != "")
            .GroupBy(p => p.Region)
            .Select(g => new { Region = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(6)
            .ToListAsync();
        TrendingRegions = trending.Select(t => new RegionCount(t.Region, t.Count)).ToList();
    }

    public IActionResult OnPostDismissOnboarding()
    {
        Response.Cookies.Append("hj_onboard_done", "1",
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });
        return RedirectToPage("/Explore");
    }
}
