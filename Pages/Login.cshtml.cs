using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class LoginModel(HikeJordanDbContext db, ILogger<LoginModel> logger, IPasswordService passwords) : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var account = await db.Users.FirstOrDefaultAsync(user =>
            user.Email.ToLower() == Input.Email.ToLower());

        if (account is null || !passwords.Verify(Input.Password, account.Password))
        {
            logger.LogWarning("Failed login attempt for email {Email} from {IP}",
                Input.Email, HttpContext.Connection.RemoteIpAddress);
            ModelState.AddModelError(string.Empty, "Invalid credentials.");
            return Page();
        }

        if (account.ApprovalStatus == AppConstants.AccountStatus.Disabled)
        {
            logger.LogWarning("Disabled account login attempt for {Email}", account.Email);
            ModelState.AddModelError(string.Empty, "Your account has been disabled. Contact support.");
            return Page();
        }

        logger.LogInformation("User {Email} ({Role}) signed in", account.Email, account.Role);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, account.Name),
            new(ClaimTypes.Email, account.Email),
            new(ClaimTypes.Role, account.Role),
            new(AppConstants.ApprovalStatusClaim, account.ApprovalStatus)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return account.Role == AppConstants.Roles.Admin ? RedirectToPage("/Admin") : RedirectToPage("/AddHike");
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        logger.LogInformation("User {Email} signed out", email);
        return RedirectToPage("/Index");
    }

    public class LoginInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
