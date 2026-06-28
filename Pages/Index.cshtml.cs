using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class IndexModel(HikeJordanDbContext db, IWhatsAppService whatsApp) : PageModel
{
    public IReadOnlyList<HikeListing> PublicHikes { get; private set; } = [];
    public IReadOnlyList<OrganizerProfile> TopOrganizers { get; private set; } = [];
    public IReadOnlyDictionary<int, IReadOnlyList<TripReview>> ApprovedReviews { get; private set; }
        = new Dictionary<int, IReadOnlyList<TripReview>>();
    public int VerifiedOrganizersCount { get; private set; }
    public IReadOnlyList<Destination> Destinations { get; private set; } = [];

    public async Task OnGetAsync()
    {
        PublicHikes = await db.HikeListings
            .Where(hike => hike.Status == AppConstants.HikeStatus.Approved
                        || hike.Status == AppConstants.HikeStatus.Published)
            .OrderBy(hike => hike.DateLabel)
            .ToListAsync();

        var hikeIds = PublicHikes.Select(h => h.Id).ToList();
        var reviews = await db.TripReviews
            .Where(r => hikeIds.Contains(r.HikeListingId) && r.Status == "Approved")
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync();

        ApprovedReviews = reviews
            .GroupBy(r => r.HikeListingId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<TripReview>)g.ToList());

        TopOrganizers = await db.OrganizerProfiles
            .Where(org => org.Status == AppConstants.AccountStatus.Verified)
            .OrderBy(org => org.Name)
            .Take(3)
            .ToListAsync();

        VerifiedOrganizersCount = await db.OrganizerProfiles
            .CountAsync(org => org.Status == AppConstants.AccountStatus.Verified);

        Destinations = await db.Destinations
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public string WhatsAppBookingUrl(string whatsAppNumber, string hikeTitle) =>
        whatsApp.BookingUrl(whatsAppNumber, hikeTitle);

    public static string TripLifecycle(HikeListing hike)
    {
        if (hike.TripDate is null) return "Upcoming";
        var now = DateTime.Now;
        var end = hike.TripDate.Value.AddHours(hike.DurationHours ?? 6);
        if (hike.TripDate.Value > now) return "Upcoming";
        if (now <= end) return "In Progress";
        return "Completed";
    }

    public static string LifecycleSlug(string lifecycle) =>
        lifecycle.ToLowerInvariant().Replace(" ", "-");

    public static string RegionSlug(string region) =>
        region.ToLowerInvariant().Replace(" ", "-");

    public static string DifficultySlug(string difficulty) =>
        difficulty.ToLowerInvariant();

    public static string ImageForRegion(string region) => region switch
    {
        "Ajloun" => "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=900&q=80",
        "Dana" => "https://images.unsplash.com/photo-1522163182402-834f871fd851?auto=format&fit=crop&w=900&q=80",
        "Dead Sea" => "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=900&q=80",
        _ => "https://images.unsplash.com/photo-1548786811-dd6e453ccca7?auto=format&fit=crop&w=900&q=80"
    };
}
