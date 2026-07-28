using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HikeJordanDotNet.Pages;

public abstract class CommunityPageModel(HikeJordanDbContext db) : PageModel
{
    protected HikeJordanDbContext Db => db;

    public int? CurrentUserId =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public async Task<IActionResult> OnPostLikeAsync(int postId, string? returnUrl)
    {
        if (CurrentUserId is int uid)
            await SocialOps.ToggleLikeAsync(db, uid, postId);

        return Back(returnUrl);
    }

    public async Task<IActionResult> OnPostFollowAsync(int targetId, string? returnUrl)
    {
        if (CurrentUserId is int uid)
            await SocialOps.ToggleFollowAsync(db, uid, targetId);

        return Back(returnUrl);
    }

    protected IActionResult Back(string? returnUrl) =>
        !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToPage("/Index");
}
