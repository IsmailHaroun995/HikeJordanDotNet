using HikeJordanDotNet.Core;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class VerifyEmailModel(HikeJordanDbContext db, ILogger<VerifyEmailModel> logger) : PageModel
{
    public enum Outcome { Success, AlreadyVerified, Invalid, Expired }

    public Outcome Result { get; private set; }
    public string? ResendEmail { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            Result = Outcome.Invalid;
            return Page();
        }

        var user = await db.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

        if (user is null)
        {
            // Token not found: either already used/cleared or never valid.
            Result = Outcome.Invalid;
            return Page();
        }

        if (user.EmailConfirmed)
        {
            Result = Outcome.AlreadyVerified;
            return Page();
        }

        if (EmailVerification.IsExpired(user.EmailTokenGeneratedUtc))
        {
            Result = Outcome.Expired;
            ResendEmail = user.Email;
            return Page();
        }

        user.EmailConfirmed = true;
        user.EmailVerificationToken = null;
        user.EmailTokenGeneratedUtc = null;
        await db.SaveChangesAsync();

        logger.LogInformation("Email verified for {Email}", user.Email);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            AuthClaims.Build(user));

        Result = Outcome.Success;
        return Page();
    }
}
