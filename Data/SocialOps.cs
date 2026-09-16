using HikeJordanDotNet.Core;
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
            AddNotification(db, post.AuthorId, userId, AppConstants.NotificationType.Like, postId);
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
        {
            db.Follows.Add(new Follow { FollowerId = followerId, FollowingId = followingId });
            AddNotification(db, followingId, followerId, AppConstants.NotificationType.Follow, null);
        }
        else
        {
            db.Follows.Remove(follow);
        }

        await db.SaveChangesAsync();
    }

    /// <summary>Queues a notification row (caller saves). Never notifies the actor about their own action.</summary>
    public static void AddNotification(HikeJordanDbContext db, int recipientId, int actorId, string type, int? postId)
    {
        if (recipientId == actorId) return;
        db.Notifications.Add(new Notification
        {
            RecipientId = recipientId,
            ActorId = actorId,
            Type = type,
            PostId = postId
        });
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
