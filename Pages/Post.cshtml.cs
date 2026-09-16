using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class PostModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public Post Entry { get; private set; } = null!;
    public IReadOnlyList<Comment> Comments { get; private set; } = [];
    public IReadOnlyList<Post> MoreFromRegion { get; private set; } = [];
    public bool Liked { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var post = await Db.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsHidden);

        if (post is null) return NotFound();

        Entry = post;

        Comments = await Db.Comments
            .Where(c => c.PostId == id)
            .Include(c => c.Author)
            .OrderBy(c => c.CreatedAtUtc)
            .ToListAsync();

        if (CurrentUserId is int uid)
            Liked = await Db.PostLikes.AnyAsync(l => l.PostId == id && l.UserId == uid);

        if (!string.IsNullOrEmpty(post.Region))
        {
            MoreFromRegion = await Db.Posts
                .Where(p => p.Region == post.Region && p.Id != post.Id && !p.IsHidden)
                .Include(p => p.Author)
                .OrderByDescending(p => p.CreatedAtUtc)
                .Take(3)
                .ToListAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCommentAsync(int id, string? body)
    {
        if (CurrentUserId is not int uid)
            return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(body))
            return RedirectToPage(new { id });

        var post = await Db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post is null) return NotFound();

        Db.Comments.Add(new Comment
        {
            PostId = id,
            AuthorId = uid,
            Body = body.Trim()
        });
        post.CommentCount++;
        SocialOps.AddNotification(Db, post.AuthorId, uid, AppConstants.NotificationType.Comment, id);
        await Db.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
}
