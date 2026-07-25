using HikeJordanDotNet.Core;
using HikeJordanDotNet.Data;
using HikeJordanDotNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class RegisterConfirmationModel(
    HikeJordanDbContext db,
    IEmailService emailService,
    ILogger<RegisterConfirmationModel> logger) : PageModel
{
    public string Email { get; private set; } = string.Empty;
    public string? Message { get; private set; }

    public void OnGet(string? email)
    {
        Email = email ?? string.Empty;
    }

    public async Task<IActionResult> OnPostResendAsync(string? email)
    {
        Email = email ?? string.Empty;

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Email);
        if (user is not null && !user.EmailConfirmed)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            await EmailVerification.IssueAndSendAsync(db, emailService, logger, user, baseUrl);
        }

        // Always show the same message — don't reveal whether the address exists.
        Message = "If that email still needs verifying, we've sent a fresh link.";
        return Page();
    }
}
