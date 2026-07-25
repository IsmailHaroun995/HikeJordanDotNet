using System.Security.Claims;
using HikeJordanDotNet.Core;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize]
public class SettingsModel(HikeJordanDbContext db, IWebHostEnvironment env) : PageModel
{
    [BindProperty] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Bio { get; set; } = string.Empty;
    [BindProperty] public string Location { get; set; } = string.Empty;
    [BindProperty] public IFormFile? Avatar { get; set; }
    [BindProperty] public IFormFile? Cover { get; set; }

    public AppUser Account { get; private set; } = null!;
    public string? Message { get; private set; }
    public string? Error { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await LoadAsync();
        if (user is null) return RedirectToPage("/Login");

        Name = user.Name;
        Bio = user.Bio;
        Location = user.Location;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await LoadAsync();
        if (user is null) return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(Name))
        {
            Error = "Name can't be empty.";
            return Page();
        }

        user.Name = Name.Trim();
        user.Bio = Bio?.Trim() ?? string.Empty;
        user.Location = Location?.Trim() ?? string.Empty;

        var avatarPath = await SaveImageAsync(Avatar, "avatars");
        if (avatarPath is not null) user.AvatarUrl = avatarPath;

        var coverPath = await SaveImageAsync(Cover, "covers");
        if (coverPath is not null) user.CoverUrl = coverPath;

        await db.SaveChangesAsync();

        // Refresh the auth cookie so name/avatar update everywhere immediately.
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            AuthClaims.Build(user));

        Message = "Profile updated.";
        return Page();
    }

    private async Task<AppUser?> LoadAsync()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid))
            return null;
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == uid);
        if (user is not null) Account = user;
        return user;
    }

    private async Task<string?> SaveImageAsync(IFormFile? file, string folder)
    {
        if (file is not { Length: > 0 }) return null;

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext) || file.Length > 8 * 1024 * 1024)
        {
            Error = "Images must be JPG/PNG/WEBP/GIF under 8 MB.";
            return null;
        }

        var dir = Path.Combine(env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(dir);
        var fileName = $"{Guid.NewGuid():N}{ext}";
        await using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
        await file.CopyToAsync(stream);
        return $"/uploads/{folder}/{fileName}";
    }
}
