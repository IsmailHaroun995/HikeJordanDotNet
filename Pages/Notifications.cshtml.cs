using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

[Authorize]
public class NotificationsModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public IReadOnlyList<Notification> Items { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        if (CurrentUserId is not int uid)
            return RedirectToPage("/Login");

        Items = await Db.Notifications
            .Where(n => n.RecipientId == uid)
            .Include(n => n.Actor)
            .OrderByDescending(n => n.CreatedAtUtc)
            .Take(60)
            .ToListAsync();

        // Mark everything read now that they're viewing the list.
        await Db.Notifications
            .Where(n => n.RecipientId == uid && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));

        return Page();
    }
}
