using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize(Roles = "Admin")]
public class AdminModel(HikeJordanDbContext db) : PageModel
{
    public IReadOnlyList<Post> Posts { get; private set; } = [];
    public IReadOnlyList<AppUser> Users { get; private set; } = [];
    public int TotalPosts { get; private set; }
    public int TotalUsers { get; private set; }
    public int TotalComments { get; private set; }

    public async Task OnGetAsync()
    {
        Posts = await db.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAtUtc)
            .Take(100)
            .ToListAsync();

        Users = await db.Users
            .OrderByDescending(u => u.CreatedAtUtc)
            .ToListAsync();

        TotalPosts = await db.Posts.CountAsync();
        TotalUsers = await db.Users.CountAsync();
        TotalComments = await db.Comments.CountAsync();
    }

    public async Task<IActionResult> OnPostToggleHideAsync(int id)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post is not null)
        {
            post.IsHidden = !post.IsHidden;
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeletePostAsync(int id)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post is not null)
        {
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleUserAsync(int id)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is not null && user.Role != AppConstants.Roles.Admin)
        {
            user.ApprovalStatus = user.ApprovalStatus == AppConstants.AccountStatus.Disabled
                ? AppConstants.AccountStatus.Approved
                : AppConstants.AccountStatus.Disabled;
            await db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
