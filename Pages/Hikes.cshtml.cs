using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class HikesModel(HikeJordanDbContext db, IWhatsAppService whatsApp) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Region { get; set; }

    public IReadOnlyList<HikeListing> Hikes { get; private set; } = [];
    public Destination? DestinationInfo { get; private set; }
    public string CoverImage { get; private set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var query = db.HikeListings.Where(h =>
            h.Status == AppConstants.HikeStatus.Approved ||
            h.Status == AppConstants.HikeStatus.Published);

        if (!string.IsNullOrEmpty(Region))
        {
            DestinationInfo = await db.Destinations
                .FirstOrDefaultAsync(d => d.Slug == Region && d.IsActive);

            if (DestinationInfo is null) return NotFound();

            query = query.Where(h => h.Region == DestinationInfo.Name);

            CoverImage = !string.IsNullOrEmpty(DestinationInfo.CoverImageUrl)
                ? DestinationInfo.CoverImageUrl
                : IndexModel.ImageForRegion(DestinationInfo.Name);

            ViewData["Title"] = $"Hiking Trips in {DestinationInfo.Name}, Jordan";
            ViewData["Description"] = $"Find organized hiking trips in {DestinationInfo.Name}, Jordan. Compare routes, difficulty, dates, prices and organizers — then book directly on WhatsApp.";
        }
        else
        {
            CoverImage = IndexModel.ImageForRegion("Wadi Rum");
            ViewData["Title"] = "All Hiking Trips in Jordan";
            ViewData["Description"] = "Browse all organized hiking trips across Jordan — Wadi Rum, Ajloun, Dana, Dead Sea, Wadi Mujib and more. Compare and book directly on WhatsApp.";
        }

        Hikes = await query.OrderBy(h => h.DateLabel).ToListAsync();
        return Page();
    }

    public string WhatsAppBookingUrl(string whatsAppNumber, string hikeTitle) =>
        whatsApp.BookingUrl(whatsAppNumber, hikeTitle);
}
