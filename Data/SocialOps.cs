using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Data;

public static class SocialOps
{
    public static async Task ToggleLikeAsync(HikeJordanDbContext db, int userId, int postId)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        if (post is null) return;

        var like = await db.PostLikes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

        if (like is null)
        {
            db.PostLikes.Add(new PostLike { PostId = postId, UserId = userId });
            post.LikeCount++;
        }
        else
        {
            db.PostLikes.Remove(like);
            post.LikeCount = Math.Max(0, post.LikeCount - 1);
        }

        await db.SaveChangesAsync();
    }

    public static async Task ToggleFollowAsync(HikeJordanDbContext db, int followerId, int followingId)
    {
        if (followerId == followingId) return;

        var follow = await db.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        if (follow is null)
            db.Follows.Add(new Follow { FollowerId = followerId, FollowingId = followingId });
        else
            db.Follows.Remove(follow);

        await db.SaveChangesAsync();
    }

    public static async Task<HashSet<int>> LikedPostIdsAsync(HikeJordanDbContext db, int userId, IEnumerable<int> postIds)
    {
        var ids = postIds.ToList();
        if (ids.Count == 0) return [];

        return (await db.PostLikes
            .Where(l => l.UserId == userId && ids.Contains(l.PostId))
            .Select(l => l.PostId)
            .ToListAsync())
            .ToHashSet();
    }
}
