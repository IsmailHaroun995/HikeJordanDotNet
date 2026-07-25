using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Pages;

public class PostModel(HikeJordanDbContext db) : CommunityPageModel(db)
{
    public Post Entry { get; private set; } = null!;
    public IReadOnlyList<Comment> Comments { get; private set; } = [];
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
        await Db.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
}
