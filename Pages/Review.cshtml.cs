using HikeJordanDotNet.Core;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class ReviewModel(HikeJordanDbContext db) : PageModel
{
    public AppUser Group { get; private set; } = null!;
    public bool Submitted { get; private set; }

    [BindProperty] public string? ReviewerName { get; set; }
    [BindProperty] public int Rating { get; set; }
    [BindProperty] public string? Comment { get; set; }

    public async Task<IActionResult> OnGetAsync(string username)
    {
        var group = await FindGroupAsync(username);
        if (group is null) return NotFound();
        Group = group;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string username)
    {
        var group = await FindGroupAsync(username);
        if (group is null) return NotFound();
        Group = group;

        if (Rating is < 1 or > 5)
        {
            ModelState.AddModelError(nameof(Rating), "Please choose a rating from 1 to 5 stars.");
            return Page();
        }

        db.GroupReviews.Add(new GroupReview
        {
            GroupId = group.Id,
            ReviewerName = string.IsNullOrWhiteSpace(ReviewerName) ? "Anonymous" : ReviewerName.Trim(),
            Rating = Rating,
            Comment = Comment?.Trim() ?? string.Empty
        });
        await db.SaveChangesAsync();

        Submitted = true;
        return Page();
    }

    private Task<AppUser?> FindGroupAsync(string username) =>
        db.Users.FirstOrDefaultAsync(u =>
            u.Username == username && u.AccountType == AppConstants.AccountType.Group);
}
