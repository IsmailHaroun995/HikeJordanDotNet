using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class LeaderboardModel(HikeJordanDbContext db) : PageModel
{
    public record Row(AppUser User, decimal Distance, int Hikes, int Elevation);

    public IReadOnlyList<Row> Rows { get; private set; } = [];
    public string Period { get; private set; } = "month";

    public async Task OnGetAsync(string? period)
    {
        Period = period == "all" ? "all" : "month";
        var since = Period == "month" ? DateTime.UtcNow.AddDays(-30) : DateTime.MinValue;

        var agg = await db.Posts
            .Where(p => !p.IsHidden && p.DistanceKm != null && p.CreatedAtUtc >= since)
            .GroupBy(p => p.AuthorId)
            .Select(g => new
            {
                AuthorId = g.Key,
                Distance = g.Sum(p => p.DistanceKm ?? 0),
                Hikes = g.Count(),
                Elevation = g.Sum(p => p.ElevationGainM ?? 0)
            })
            .OrderByDescending(x => x.Distance)
            .Take(50)
            .ToListAsync();

        var ids = agg.Select(a => a.AuthorId).ToList();
        var users = await db.Users
            .Where(u => ids.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        Rows = agg
            .Where(a => users.ContainsKey(a.AuthorId))
            .Select(a => new Row(users[a.AuthorId], a.Distance, a.Hikes, a.Elevation))
            .ToList();
    }
}
