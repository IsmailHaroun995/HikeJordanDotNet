using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using HikeJordanDotNet.Core;
using HikeJordanDotNet.Data;
using HikeJordanDotNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class RegisterModel(
    HikeJordanDbContext db,
    IPasswordService passwords,
    IEmailService email,
    ILogger<RegisterModel> logger) : PageModel
{
    [BindProperty]
    public RegisterInput Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var emailAddr = Input.Email.Trim().ToLowerInvariant();
        var username = Slugify(Input.Username);

        if (username.Length < 3)
        {
            ModelState.AddModelError("Input.Username", "Username must be at least 3 characters (letters, numbers, underscore).");
            return Page();
        }

        if (await db.Users.AnyAsync(u => u.Email == emailAddr))
        {
            ModelState.AddModelError("Input.Email", "An account with this email already exists.");
            return Page();
        }

        if (await db.Users.AnyAsync(u => u.Username == username))
        {
            ModelState.AddModelError("Input.Username", "That username is taken.");
            return Page();
        }

        var user = new AppUser
        {
            Name = Input.Name.Trim(),
            Username = username,
            Email = emailAddr,
            Password = passwords.Hash(Input.Password),
            Role = AppConstants.Roles.Member,
            ApprovalStatus = AppConstants.AccountStatus.Approved,
            EmailConfirmed = false
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        await EmailVerification.IssueAndSendAsync(db, email, logger, user, baseUrl);

        return RedirectToPage("/RegisterConfirmation", new { email = user.Email });
    }

    private static string Slugify(string input) =>
        Regex.Replace(input.Trim().ToLowerInvariant(), @"[^a-z0-9_]", "");

    public class RegisterInput
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(160)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
