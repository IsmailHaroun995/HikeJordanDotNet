using System.ComponentModel.DataAnnotations;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class ReviewModel(HikeJordanDbContext db, ILogger<ReviewModel> logger) : PageModel
{
    [BindProperty]
    public ReviewInput Input { get; set; } = new();

    public HikeListing? Hike { get; private set; }
    public bool Submitted { get; private set; }

    public bool ReviewsClosed { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Hike = await db.HikeListings.FindAsync(id);
        if (Hike is null) return NotFound();
        ReviewsClosed = Hike.Status == AppConstants.HikeStatus.Completed;
        Input.HikeId = id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Hike = await db.HikeListings.FindAsync(Input.HikeId);
        if (Hike is null) return NotFound();

        if (Hike.Status == AppConstants.HikeStatus.Completed)
        {
            ReviewsClosed = true;
            return Page();
        }

        if (!ModelState.IsValid) return Page();

        db.TripReviews.Add(new TripReview
        {
            HikeListingId = Input.HikeId,
            ReviewerName = Input.ReviewerName,
            Rating = Input.Rating,
            ReviewText = Input.ReviewText,
            Status = "Pending"
        });

        await db.SaveChangesAsync();
        logger.LogInformation("Review submitted for hike {HikeId} by {Reviewer}", Input.HikeId, Input.ReviewerName);

        Submitted = true;
        return Page();
    }

    public class ReviewInput
    {
        public int HikeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Your name")]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Please choose a rating from 1 to 5.")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Please write at least 10 characters.")]
        [MaxLength(600)]
        [Display(Name = "Your review")]
        public string ReviewText { get; set; } = string.Empty;
    }
}
