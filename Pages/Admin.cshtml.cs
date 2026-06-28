using HikeJordanDotNet.Data;
using HikeJordanDotNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize(Roles = AppConstants.Roles.Admin)]
public class AdminModel(HikeJordanDbContext db, ILogger<AdminModel> logger, IPasswordService passwords, IEmailService email) : PageModel
{
    public IReadOnlyList<OrganizerRequest> OrganizerRequests { get; private set; } = [];
    public IReadOnlyList<HikeListing> SubmittedHikes { get; private set; } = [];
    public IReadOnlyList<OrganizerProfile> OrganizerReviews { get; private set; } = [];
    public IReadOnlyList<ReviewFlag> ReviewFlags { get; private set; } = [];
    public IReadOnlyList<AppUser> ActiveOrganizers { get; private set; } = [];
    public IReadOnlyList<TripReview> PendingReviews { get; private set; } = [];
    public IReadOnlyList<Destination> Destinations { get; private set; } = [];

    public async Task OnGetAsync()
    {
        OrganizerRequests = await db.OrganizerRequests
            .OrderByDescending(request => request.CreatedAtUtc)
            .ToListAsync();

        SubmittedHikes = await db.HikeListings
            .OrderBy(hike => hike.Status == AppConstants.HikeStatus.Submitted ? 0 : 1)
            .ThenBy(hike => hike.DateLabel)
            .ToListAsync();

        OrganizerReviews = await db.OrganizerProfiles
            .OrderBy(profile => profile.Status == AppConstants.AccountStatus.Pending ? 0 : 1)
            .ThenBy(profile => profile.Name)
            .ToListAsync();

        ReviewFlags = await db.ReviewFlags
            .Where(flag => flag.Status != "Resolved")
            .OrderBy(flag => flag.Priority == "High" ? 0 : flag.Priority == "Medium" ? 1 : 2)
            .ToListAsync();

        ActiveOrganizers = await db.Users
            .Where(user => user.Role == AppConstants.Roles.Organizer)
            .OrderBy(user => user.ApprovalStatus == AppConstants.AccountStatus.Disabled ? 1 : 0)
            .ThenBy(user => user.Name)
            .ToListAsync();

        PendingReviews = await db.TripReviews
            .Where(r => r.Status == "Pending")
            .Include(r => r.Hike)
            .OrderBy(r => r.CreatedAtUtc)
            .ToListAsync();

        Destinations = await db.Destinations
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddDestinationAsync(string name, string? coverImageUrl)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var slug = name.Trim().ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace(",", "");
            db.Destinations.Add(new Destination
            {
                Name = name.Trim(),
                Slug = slug,
                CoverImageUrl = string.IsNullOrWhiteSpace(coverImageUrl) ? null : coverImageUrl.Trim(),
                IsActive = true
            });
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleDestinationAsync(int id)
    {
        var dest = await db.Destinations.FindAsync(id);
        if (dest is not null)
        {
            dest.IsActive = !dest.IsActive;
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteDestinationAsync(int id)
    {
        var dest = await db.Destinations.FindAsync(id);
        if (dest is not null)
        {
            db.Destinations.Remove(dest);
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateOrganizerRequestAsync(int id, string status)
    {
        var request = await db.OrganizerRequests.FindAsync(id);
        if (request is null)
        {
            return RedirectToPage();
        }

        request.Status = status;
        var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        logger.LogInformation("Admin {Admin} set organizer request {Id} ({Name}) to status {Status}",
            adminEmail, id, request.Name, status);

        var user = await db.Users.FirstOrDefaultAsync(item => item.Email == request.Email);
        if (status == AppConstants.AccountStatus.Approved)
        {
            if (user is null)
            {
                var tempPassword = passwords.GenerateTemporary();
                db.Users.Add(new AppUser
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = passwords.Hash(tempPassword),
                    Role = AppConstants.Roles.Organizer,
                    ApprovalStatus = AppConstants.AccountStatus.Approved
                });
                logger.LogWarning("New organizer account created for {Email}. Temp password must be communicated securely.", request.Email);
            }
            else
            {
                user.ApprovalStatus = AppConstants.AccountStatus.Approved;
                user.Role = AppConstants.Roles.Organizer;
            }

            if (!await db.OrganizerProfiles.AnyAsync(profile => profile.Name == request.Name))
            {
                db.OrganizerProfiles.Add(new OrganizerProfile
                {
                    Name = request.Name,
                    Status = AppConstants.AccountStatus.Verified,
                    Rating = "New",
                    PastTrips = "0 trips",
                    Note = $"Approved for: {request.Regions}"
                });
            }
        }
        else if (status == "Docs requested")
        {
            await email.NotifyOrganizerDocsRequestedAsync(request.Email, request.Name);
        }
        else if (user is not null && status == AppConstants.AccountStatus.Rejected)
        {
            user.ApprovalStatus = AppConstants.AccountStatus.Rejected;
        }

        await db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateHikeAsync(int id, string status)
    {
        var hike = await db.HikeListings.FindAsync(id);
        if (hike is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            logger.LogInformation("Admin {Admin} set hike {Id} ({Title}) to status {Status}",
                adminEmail, id, hike.Title, status);
            hike.Status = status;
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDisableOrganizerAsync(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            user.ApprovalStatus = AppConstants.AccountStatus.Disabled;
            await db.SaveChangesAsync();
            logger.LogWarning("Admin {Admin} disabled organizer account {Id} ({Email})", adminEmail, id, user.Email);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEnableOrganizerAsync(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            user.ApprovalStatus = AppConstants.AccountStatus.Approved;
            await db.SaveChangesAsync();
            logger.LogInformation("Admin {Admin} re-enabled organizer account {Id} ({Email})", adminEmail, id, user.Email);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateOrganizerProfileAsync(int id, string status)
    {
        var profile = await db.OrganizerProfiles.FindAsync(id);
        if (profile is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            logger.LogInformation("Admin {Admin} set organizer profile {Id} ({Name}) to status {Status}",
                adminEmail, id, profile.Name, status);
            profile.Status = status;
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResolveFlagAsync(int id)
    {
        var flag = await db.ReviewFlags.FindAsync(id);
        if (flag is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            logger.LogInformation("Admin {Admin} resolved review flag {Id} ({Title})",
                adminEmail, id, flag.Title);
            flag.Status = "Resolved";
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostApproveReviewAsync(int id)
    {
        var review = await db.TripReviews.FindAsync(id);
        if (review is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            review.Status = "Approved";
            await db.SaveChangesAsync();
            logger.LogInformation("Admin {Admin} approved trip review {Id}", adminEmail, id);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRejectReviewAsync(int id)
    {
        var review = await db.TripReviews.FindAsync(id);
        if (review is not null)
        {
            var adminEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            review.Status = "Rejected";
            await db.SaveChangesAsync();
            logger.LogInformation("Admin {Admin} rejected trip review {Id}", adminEmail, id);
        }
        return RedirectToPage();
    }
}
