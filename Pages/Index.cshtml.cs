using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class IndexModel(HikeJordanDbContext db) : PageModel
{
    public record Hiker(AppUser User, decimal Distance);
    public record RegionCount(string Region, int Count);

    public int PostCount { get; private set; }
    public int MemberCount { get; private set; }
    public int PlacesCount { get; private set; }
    public IReadOnlyList<RegionCount> PopularRegions { get; private set; } = [];
    public IReadOnlyList<Hiker> TopHikers { get; private set; } = [];

    public async Task OnGetAsync()
    {
        PostCount = await db.Posts.CountAsync(p => !p.IsHidden);
        MemberCount = await db.Users.CountAsync();

        var regions = await db.Posts
            .Where(p => !p.IsHidden && p.Region != "")
            .GroupBy(p => p.Region)
            .Select(g => new { Region = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(8)
            .ToListAsync();
        PopularRegions = regions.Select(r => new RegionCount(r.Region, r.Count)).ToList();
        PlacesCount = await db.Posts.Where(p => !p.IsHidden && p.Region != "").Select(p => p.Region).Distinct().CountAsync();

        var top = await db.Posts
            .Where(p => !p.IsHidden && p.DistanceKm != null)
            .GroupBy(p => p.AuthorId)
            .Select(g => new { AuthorId = g.Key, Distance = g.Sum(p => p.DistanceKm ?? 0) })
            .OrderByDescending(x => x.Distance)
            .Take(3)
            .ToListAsync();
        var ids = top.Select(t => t.AuthorId).ToList();
        var users = await db.Users.Where(u => ids.Contains(u.Id)).ToDictionaryAsync(u => u.Id);
        TopHikers = top.Where(t => users.ContainsKey(t.AuthorId))
            .Select(t => new Hiker(users[t.AuthorId], t.Distance))
            .ToList();
    }
}
