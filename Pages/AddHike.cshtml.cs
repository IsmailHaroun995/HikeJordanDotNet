using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize(Roles = AppConstants.Roles.Organizer)]
public class AddHikeModel(HikeJordanDbContext db, ILogger<AddHikeModel> logger, IEmailService email, IWebHostEnvironment env) : PageModel
{
    [BindProperty]
    public HikeSubmission Input { get; set; } = new();

    public bool Submitted { get; private set; }
    public bool IsApprovedOrganizer { get; private set; }
    public string OrganizerDisplayName { get; private set; } = "Organizer";
    public IReadOnlyList<Destination> Destinations { get; private set; } = [];

    public async Task OnGetAsync()
    {
        LoadOrganizerState();
        await LoadDestinationsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LoadOrganizerState();
        await LoadDestinationsAsync();

        if (!IsApprovedOrganizer)
        {
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        string? imagePath = null;
        if (Input.Image is { Length: > 0 })
        {
            var ext = Path.GetExtension(Input.Image.FileName).ToLowerInvariant();
            if (ext is ".jpg" or ".jpeg" or ".png" or ".webp")
            {
                var uploadsDir = Path.Combine(env.WebRootPath, "uploads", "trips");
                Directory.CreateDirectory(uploadsDir);
                var fileName = $"{Guid.NewGuid()}{ext}";
                using var stream = System.IO.File.Create(Path.Combine(uploadsDir, fileName));
                await Input.Image.CopyToAsync(stream);
                imagePath = $"/uploads/trips/{fileName}";
            }
        }

        var hike = new HikeListing
        {
            Title = Input.Title,
            Organizer = OrganizerDisplayName,
            WhatsApp = Input.WhatsAppNumber,
            Region = Input.Region,
            Difficulty = Input.Difficulty,
            Status = AppConstants.HikeStatus.Submitted,
            DateLabel = Input.Date?.ToString("MMM d") ?? "TBD",
            TimeLabel = Input.Time?.ToString(@"hh\:mm") ?? string.Empty,
            TripDate = Input.Date.HasValue
                ? (Input.Time.HasValue
                    ? Input.Date.Value.Date.Add(Input.Time.Value)
                    : Input.Date.Value.Date)
                : null,
            SpotsLeft = Input.Capacity ?? 0,
            Price = Input.Price ?? 0,
            DurationHours = Input.DurationHours,
            DistanceKm = Input.DistanceKm,
            Description = Input.Description,
            MeetingPoint = Input.MeetingPoint,
            RequiredGear = Input.RequiredGear,
            IncludedItems = Input.IncludedItems,
            ExcludedItems = Input.ExcludedItems,
            PaymentType = Input.PaymentType,
            GroupName = Input.GroupName,
            InstagramPage = Input.InstagramPage,
            ImagePath = imagePath,
            Note = string.Empty
        };

        db.HikeListings.Add(hike);
        await db.SaveChangesAsync();

        logger.LogInformation("Organizer {Organizer} submitted hike '{Title}' (Id={Id})",
            OrganizerDisplayName, hike.Title, hike.Id);

        var organizerUser = await db.Users.FirstOrDefaultAsync(u => u.Name == OrganizerDisplayName);
        var organizerEmail = organizerUser?.Email ?? string.Empty;
        if (!string.IsNullOrEmpty(organizerEmail))
            await email.NotifyOrganizerNewTripAsync(organizerEmail, OrganizerDisplayName, hike.Title);
        await email.NotifyAdminNewTripAsync(OrganizerDisplayName, organizerEmail, hike.Title);

        Submitted = true;
        return Page();
    }

    private void LoadOrganizerState()
    {
        OrganizerDisplayName = User.Identity?.Name ?? "Organizer";
        IsApprovedOrganizer = User.FindFirstValue(AppConstants.ApprovalStatusClaim) == AppConstants.AccountStatus.Approved;
        Input.OrganizerName = string.IsNullOrWhiteSpace(Input.OrganizerName)
            ? OrganizerDisplayName
            : Input.OrganizerName;
    }

    private async Task LoadDestinationsAsync()
    {
        Destinations = await db.Destinations
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public class HikeSubmission
    {
        [Required]
        [Display(Name = "Organizer name")]
        public string OrganizerName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "WhatsApp number")]
        public string WhatsAppNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(90)]
        [Display(Name = "Hike title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(700)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        [Required]
        public string Difficulty { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan? Time { get; set; }

        [Required]
        [Range(0, 500)]
        public decimal? Price { get; set; }

        [Range(1, 48)]
        public int? DurationHours { get; set; }

        [Range(1, 60)]
        public decimal? DistanceKm { get; set; }

        [Range(1, 200)]
        public int? Capacity { get; set; }

        [Display(Name = "Meeting point")]
        public string MeetingPoint { get; set; } = string.Empty;

        [Display(Name = "Required gear")]
        public string RequiredGear { get; set; } = string.Empty;

        [Display(Name = "Included items")]
        public string IncludedItems { get; set; } = string.Empty;

        [Display(Name = "Excluded items")]
        public string ExcludedItems { get; set; } = string.Empty;

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; } = string.Empty;

        [Display(Name = "Group name")]
        public string GroupName { get; set; } = string.Empty;

        [Display(Name = "Instagram page")]
        public string InstagramPage { get; set; } = string.Empty;

        [Display(Name = "Trip photo")]
        public IFormFile? Image { get; set; }
    }
}
