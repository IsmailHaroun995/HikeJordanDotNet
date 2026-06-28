using System.ComponentModel.DataAnnotations;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class OrganizerSignupModel(HikeJordanDbContext db, IPasswordService passwords, IEmailService email) : PageModel
{
    [BindProperty]
    public SignupInput Input { get; set; } = new();

    public bool Submitted { get; private set; }
    public bool AlreadyApproved { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingUser = await db.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == Input.Email.ToLower());
        if (existingUser is null)
        {
            db.Users.Add(new AppUser
            {
                Name = Input.OrganizerName,
                Email = Input.Email,
                Password = passwords.Hash(Input.Password),
                Role = AppConstants.Roles.Organizer,
                ApprovalStatus = AppConstants.AccountStatus.Pending
            });
        }
        else
        {
            // Account was pre-created by admin approval with a temp password the user doesn't know.
            // Update the password so the organizer can actually sign in.
            existingUser.Password = passwords.Hash(Input.Password);
            AlreadyApproved = existingUser.ApprovalStatus == AppConstants.AccountStatus.Approved;
        }

        db.OrganizerRequests.Add(new OrganizerRequest
        {
            Name = Input.OrganizerName,
            Email = Input.Email,
            WhatsApp = Input.WhatsAppNumber,
            Regions = Input.Regions,
            Experience = Input.Experience,
            Status = AppConstants.AccountStatus.Submitted
        });

        await db.SaveChangesAsync();

        await email.NotifyOrganizerSignupAsync(Input.Email, Input.OrganizerName);
        await email.NotifyAdminNewOrganizerAsync(Input.OrganizerName, Input.Email, Input.WhatsAppNumber, Input.Regions);

        Submitted = true;
        return Page();
    }

    public class SignupInput
    {
        [Required]
        [Display(Name = "Organizer / company name")]
        public string OrganizerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "WhatsApp number")]
        public string WhatsAppNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Main regions")]
        public string Regions { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Experience and safety notes")]
        public string Experience { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
