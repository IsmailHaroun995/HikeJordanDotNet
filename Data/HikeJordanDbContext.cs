using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Data;

public class HikeJordanDbContext(DbContextOptions<HikeJordanDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<OrganizerRequest> OrganizerRequests => Set<OrganizerRequest>();
    public DbSet<HikeListing> HikeListings => Set<HikeListing>();
    public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();
    public DbSet<ReviewFlag> ReviewFlags => Set<ReviewFlag>();
    public DbSet<TripReview> TripReviews => Set<TripReview>();
    public DbSet<Destination> Destinations => Set<Destination>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<OrganizerRequest>()
            .HasIndex(request => request.Email);

        modelBuilder.Entity<HikeListing>()
            .HasIndex(hike => new { hike.Status, hike.Region, hike.Difficulty });

        modelBuilder.Entity<TripReview>()
            .HasOne(review => review.Hike)
            .WithMany()
            .HasForeignKey(review => review.HikeListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TripReview>()
            .HasIndex(review => new { review.HikeListingId, review.Status });
    }
}
