using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize(Roles = AppConstants.Roles.Organizer)]
public class OrganizerDashboardModel(HikeJordanDbContext db, ILogger<OrganizerDashboardModel> logger) : PageModel
{
    public IReadOnlyList<HikeListing> MyTrips { get; private set; } = [];
    public IReadOnlyDictionary<int, IReadOnlyList<TripReview>> ApprovedReviews { get; private set; }
        = new Dictionary<int, IReadOnlyList<TripReview>>();
    public string OrganizerName { get; private set; } = string.Empty;
    public string ApprovalStatus { get; private set; } = string.Empty;

    public async Task<IActionResult> OnPostUpdateSpotsAsync(int id, int spots)
    {
        var organizerName = User.Identity?.Name ?? string.Empty;
        var hike = await db.HikeListings
            .FirstOrDefaultAsync(h => h.Id == id && h.Organizer == organizerName);

        if (hike is null)
        {
            logger.LogWarning("UpdateSpots: hike {Id} not found for organizer {Organizer}", id, organizerName);
            return RedirectToPage();
        }

        var isUpcoming = hike.TripDate is null || hike.TripDate > DateTime.Now;
        if (!isUpcoming)
        {
            logger.LogWarning("UpdateSpots: hike {Id} is not upcoming, rejecting spots update", id);
            return RedirectToPage();
        }

        hike.SpotsLeft = Math.Max(0, spots);
        await db.SaveChangesAsync();
        logger.LogInformation("Organizer {Organizer} updated spots for hike {Id} to {Spots}", organizerName, id, hike.SpotsLeft);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCloseTripAsync(int id)
    {
        var organizerName = User.Identity?.Name ?? string.Empty;
        var hike = await db.HikeListings
            .FirstOrDefaultAsync(h => h.Id == id && h.Organizer == organizerName);

        if (hike is not null &&
            (hike.Status == AppConstants.HikeStatus.Approved ||
             hike.Status == AppConstants.HikeStatus.Published))
        {
            hike.Status = AppConstants.HikeStatus.Completed;
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task OnGetAsync()
    {
        OrganizerName = User.Identity?.Name ?? string.Empty;
        ApprovalStatus = User.FindFirstValue(AppConstants.ApprovalStatusClaim) ?? string.Empty;

        MyTrips = await db.HikeListings
            .Where(h => h.Organizer == OrganizerName)
            .OrderByDescending(h => h.TripDate)
            .ThenByDescending(h => h.Id)
            .ToListAsync();

        if (MyTrips.Count > 0)
        {
            var ids = MyTrips.Select(h => h.Id).ToList();
            var reviews = await db.TripReviews
                .Where(r => ids.Contains(r.HikeListingId) && r.Status == "Approved")
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
            ApprovedReviews = reviews
                .GroupBy(r => r.HikeListingId)
                .ToDictionary(g => g.Key, g => (IReadOnlyList<TripReview>)g.ToList());
        }
    }

    public int PublishedCount => MyTrips.Count(h => h.Status == AppConstants.HikeStatus.Published);
    public int PendingCount => MyTrips.Count(h => h.Status == AppConstants.HikeStatus.Submitted);
    public int TotalReviews => ApprovedReviews.Values.Sum(r => r.Count);

    public static string TripLifecycle(HikeListing hike) => IndexModel.TripLifecycle(hike);
    public static string LifecycleSlug(string lc) => IndexModel.LifecycleSlug(lc);
}
