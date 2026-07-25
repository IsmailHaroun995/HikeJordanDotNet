using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HikeJordanDotNet.Pages;

[Authorize]
public class ComposeModel(HikeJordanDbContext db, IWebHostEnvironment env) : PageModel
{
    [BindProperty]
    public string Body { get; set; } = string.Empty;

    [BindProperty]
    public string Region { get; set; } = string.Empty;

    [BindProperty]
    public string LocationName { get; set; } = string.Empty;

    [BindProperty]
    public IFormFile? Image { get; set; }

    public string? Error { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid))
            return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(Body))
        {
            Error = "Write something before posting.";
            return Page();
        }

        string? imageUrl = null;
        if (Image is { Length: > 0 })
        {
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(Image.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
            {
                Error = "Image must be a JPG, PNG, WEBP or GIF file.";
                return Page();
            }
            if (Image.Length > 8 * 1024 * 1024)
            {
                Error = "Image must be smaller than 8 MB.";
                return Page();
            }

            var dir = Path.Combine(env.WebRootPath, "uploads", "posts");
            Directory.CreateDirectory(dir);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(dir, fileName);
            await using (var stream = System.IO.File.Create(fullPath))
            {
                await Image.CopyToAsync(stream);
            }
            imageUrl = $"/uploads/posts/{fileName}";
        }

        var post = new Post
        {
            AuthorId = uid,
            Body = Body.Trim(),
            Region = Region?.Trim() ?? string.Empty,
            LocationName = LocationName?.Trim() ?? string.Empty,
            ImageUrl = imageUrl
        };

        db.Posts.Add(post);
        await db.SaveChangesAsync();

        return RedirectToPage("/Post", new { id = post.Id });
    }
}
