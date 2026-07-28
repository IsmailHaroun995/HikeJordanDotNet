using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Data;

public class HikeJordanDbContext(DbContextOptions<HikeJordanDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<PostLike> PostLikes => Set<PostLike>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<GroupReview> GroupReviews => Set<GroupReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Username)
            .IsUnique();

        // Post → Author (do not cascade user deletes into posts)
        modelBuilder.Entity<Post>()
            .HasOne(post => post.Author)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Post>()
            .HasIndex(post => new { post.IsHidden, post.CreatedAtUtc });

        modelBuilder.Entity<Post>()
            .HasIndex(post => post.Region);

        // Comment → Post cascades; Comment → Author does not (avoids multiple cascade paths)
        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Author)
            .WithMany()
            .HasForeignKey(comment => comment.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // PostLike → Post cascades; UserId is a plain column (no FK) to avoid cascade cycles
        modelBuilder.Entity<PostLike>()
            .HasOne(like => like.Post)
            .WithMany(post => post.Likes)
            .HasForeignKey(like => like.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PostLike>()
            .HasIndex(like => new { like.PostId, like.UserId })
            .IsUnique();

        // Follow: no navigations, plain columns with a unique pair index
        modelBuilder.Entity<Follow>()
            .HasIndex(follow => new { follow.FollowerId, follow.FollowingId })
            .IsUnique();

        modelBuilder.Entity<Follow>()
            .HasIndex(follow => follow.FollowingId);

        // GroupReview → Group (AppUser); cascade so removing a group clears its reviews.
        modelBuilder.Entity<GroupReview>()
            .HasOne(review => review.Group)
            .WithMany()
            .HasForeignKey(review => review.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupReview>()
            .HasIndex(review => new { review.GroupId, review.IsHidden });
    }
}
