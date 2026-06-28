using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Data;

public static class DatabaseSeeder
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HikeJordanDbContext>();
        var passwords = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new AppUser { Name = "Admin", Email = "admin@hikejordan.test", Password = passwords.Hash("admin123"), Role = AppConstants.Roles.Admin, ApprovalStatus = AppConstants.AccountStatus.Approved },
                new AppUser { Name = "Desert Paths", Email = "organizer@hikejordan.test", Password = passwords.Hash("organizer123"), Role = AppConstants.Roles.Organizer, ApprovalStatus = AppConstants.AccountStatus.Approved },
                new AppUser { Name = "Pending Organizer", Email = "pending@hikejordan.test", Password = passwords.Hash("pending123"), Role = AppConstants.Roles.Organizer, ApprovalStatus = AppConstants.AccountStatus.Pending });
        }

        if (!await db.OrganizerRequests.AnyAsync())
        {
            db.OrganizerRequests.AddRange(
                new OrganizerRequest { Name = "Wadi Nomads", Email = "owner@wadinomads.test", WhatsApp = "+962791112200", Regions = "Wadi Rum, Dead Sea", Experience = "6 years guiding desert and valley hikes. First-aid certified guides.", Status = AppConstants.AccountStatus.Submitted },
                new OrganizerRequest { Name = "Amman Weekend Treks", Email = "hello@ammanweekend.test", WhatsApp = "+962785550190", Regions = "Ajloun, Salt", Experience = "Small-group beginner hikes with transport from Amman.", Status = AppConstants.AccountStatus.Submitted },
                new OrganizerRequest { Name = "Dana Wild Routes", Email = "ops@danawild.test", WhatsApp = "+962772408821", Regions = "Dana, Wadi Mujib", Experience = "Advanced canyon routes. License and insurance pending review.", Status = "Needs docs" });
        }

        if (!await db.HikeListings.AnyAsync())
        {
            db.HikeListings.AddRange(
                new HikeListing { Title = "Wadi Rum Sunset Ridge", Organizer = "Desert Paths", WhatsApp = "+962791000001", Region = "Wadi Rum", Difficulty = "Moderate", Status = AppConstants.HikeStatus.Submitted, DateLabel = "Jul 3", SpotsLeft = 7, Price = 35, DurationHours = 4, DistanceKm = 8, Description = "Hike to the famous Sunset Ridge overlooking Wadi Rum. Guides provided.", Note = "Needs image quality check" },
                new HikeListing { Title = "Ajloun Forest Morning Loop", Organizer = "Green North", WhatsApp = "+962791000002", Region = "Ajloun", Difficulty = "Easy", Status = AppConstants.HikeStatus.Submitted, DateLabel = "Jul 4", SpotsLeft = 14, Price = 20, DurationHours = 3, DistanceKm = 5, Description = "A gentle morning loop through Ajloun's oak forests. Perfect for families.", Note = "Ready to publish" },
                new HikeListing { Title = "Dana Canyon Descent", Organizer = "Jordan Trail Co.", WhatsApp = "+962791000003", Region = "Dana", Difficulty = "Hard", Status = AppConstants.HikeStatus.Submitted, DateLabel = "Jul 10", SpotsLeft = 5, Price = 60, DurationHours = 8, DistanceKm = 14, Description = "Full-day descent through Dana's dramatic canyon system. Advanced hikers only.", Note = "Verify safety notes" });
        }

        if (!await db.OrganizerProfiles.AnyAsync())
        {
            db.OrganizerProfiles.AddRange(
                new OrganizerProfile { Name = "Desert Paths", Status = AppConstants.AccountStatus.Verified, Rating = "4.8", PastTrips = "97 trips", Note = "Local guide documents uploaded" },
                new OrganizerProfile { Name = "Green North", Status = AppConstants.AccountStatus.Pending, Rating = "4.7", PastTrips = "84 trips", Note = "Waiting for license photo" },
                new OrganizerProfile { Name = "Jordan Trail Co.", Status = AppConstants.AccountStatus.Verified, Rating = "4.9", PastTrips = "126 trips", Note = "Insurance proof current" });
        }

        if (!await db.Destinations.AnyAsync())
        {
            db.Destinations.AddRange(
                new Destination { Name = "Wadi Rum", Slug = "wadi-rum", CoverImageUrl = "https://images.unsplash.com/photo-1548786811-dd6e453ccca7?auto=format&fit=crop&w=900&q=80", IsActive = true },
                new Destination { Name = "Ajloun", Slug = "ajloun", CoverImageUrl = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=900&q=80", IsActive = true },
                new Destination { Name = "Dana", Slug = "dana", CoverImageUrl = "https://images.unsplash.com/photo-1522163182402-834f871fd851?auto=format&fit=crop&w=900&q=80", IsActive = true },
                new Destination { Name = "Dead Sea", Slug = "dead-sea", CoverImageUrl = "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=900&q=80", IsActive = true },
                new Destination { Name = "Wadi Mujib", Slug = "wadi-mujib", IsActive = true },
                new Destination { Name = "Salt", Slug = "salt", IsActive = true });
        }

        if (!await db.ReviewFlags.AnyAsync())
        {
            db.ReviewFlags.AddRange(
                new ReviewFlag { Title = "Possible duplicate review", Detail = "Same text appeared on two Dana trips", Priority = "Medium" },
                new ReviewFlag { Title = "Organizer response dispute", Detail = "Visitor says WhatsApp reply took 3 days", Priority = "Low" },
                new ReviewFlag { Title = "Unverified attendance", Detail = "Reviewer did not provide trip proof", Priority = "High" });
        }

        await db.SaveChangesAsync();
    }
}
