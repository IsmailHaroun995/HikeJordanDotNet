using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class HikeModel(HikeJordanDbContext db, IWhatsAppService whatsApp) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public HikeListing Hike { get; private set; } = null!;
    public IReadOnlyList<TripReview> Reviews { get; private set; } = [];
    public string WhatsAppUrl { get; private set; } = "";
    public string CoverImage { get; private set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var hike = await db.HikeListings.FirstOrDefaultAsync(h =>
            h.Id == Id &&
            (h.Status == AppConstants.HikeStatus.Approved ||
             h.Status == AppConstants.HikeStatus.Published));

        if (hike is null) return NotFound();

        Hike = hike;
        CoverImage = !string.IsNullOrEmpty(hike.ImagePath)
            ? hike.ImagePath
            : IndexModel.ImageForRegion(hike.Region);

        Reviews = await db.TripReviews
            .Where(r => r.HikeListingId == Id && r.Status == "Approved")
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync();

        WhatsAppUrl = whatsApp.BookingUrl(hike.WhatsApp, hike.Title);

        ViewData["Title"] = $"{hike.Title} — {hike.Region} Hiking Trip";
        ViewData["Description"] = !string.IsNullOrWhiteSpace(hike.Description)
            ? hike.Description[..Math.Min(hike.Description.Length, 155)]
            : $"Organized hiking trip in {hike.Region}, Jordan. {hike.Difficulty} difficulty, {hike.DurationHours}h, JD {hike.Price:0}. Book directly on WhatsApp.";

        return Page();
    }

    public static string StarRating(int rating) =>
        string.Concat(Enumerable.Repeat("★", rating)) +
        string.Concat(Enumerable.Repeat("☆", 5 - rating));
}
