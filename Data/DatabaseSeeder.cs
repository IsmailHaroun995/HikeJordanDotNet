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

        if (await db.Users.AnyAsync())
        {
            return;
        }

        var admin = new AppUser
        {
            Name = "HikeJordan",
            Username = "hikejordan",
            Email = "admin@hikejordan.test",
            Password = passwords.Hash("admin123"),
            Role = AppConstants.Roles.Admin,
            ApprovalStatus = AppConstants.AccountStatus.Approved,
            EmailConfirmed = true,
            Bio = "The community for Jordan's outdoor explorers.",
            Location = "Amman, Jordan",
            AvatarUrl = "https://images.unsplash.com/photo-1533105079780-92b9be482077?auto=format&fit=crop&w=200&q=80"
        };

        var lina = new AppUser
        {
            Name = "Lina Haddad",
            Username = "lina",
            Email = "lina@hikejordan.test",
            Password = passwords.Hash("member123"),
            Role = AppConstants.Roles.Member,
            ApprovalStatus = AppConstants.AccountStatus.Approved,
            EmailConfirmed = true,
            Bio = "Weekend hiker. Chasing sunrises across the Jordan Trail.",
            Location = "Amman",
            AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?auto=format&fit=crop&w=200&q=80"
        };

        var omar = new AppUser
        {
            Name = "Omar Nasser",
            Username = "omar",
            Email = "omar@hikejordan.test",
            Password = passwords.Hash("member123"),
            Role = AppConstants.Roles.Member,
            ApprovalStatus = AppConstants.AccountStatus.Approved,
            EmailConfirmed = true,
            Bio = "Canyon guide in Wadi Mujib. Water, rock, repeat.",
            Location = "Madaba",
            AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=200&q=80"
        };

        db.Users.AddRange(admin, lina, omar);
        await db.SaveChangesAsync();

        var posts = new[]
        {
            new Post
            {
                AuthorId = lina.Id,
                Region = "Wadi Rum",
                LocationName = "Sunset Ridge",
                Body = "Camped in Wadi Rum last night and hiked up to Sunset Ridge for golden hour. The silence out here is unreal — no signal, no noise, just red sand and sky. Bring 3L of water, it's dry.",
                ImageUrl = "https://images.unsplash.com/photo-1548786811-dd6e453ccca7?auto=format&fit=crop&w=1200&q=80",
                CreatedAtUtc = DateTime.UtcNow.AddHours(-3)
            },
            new Post
            {
                AuthorId = omar.Id,
                Region = "Wadi Mujib",
                LocationName = "Siq Trail",
                Body = "Ran the Mujib Siq Trail with a group this morning. Water level is perfect right now — waist-deep in places. Wear shoes with grip, the rocks are slick. Closed season starts soon so go while you can!",
                ImageUrl = "https://images.unsplash.com/photo-1504151932400-72d4384f04b3?auto=format&fit=crop&w=1200&q=80",
                CreatedAtUtc = DateTime.UtcNow.AddHours(-8)
            },
            new Post
            {
                AuthorId = lina.Id,
                Region = "Ajloun",
                LocationName = "Forest Reserve",
                Body = "Easy morning loop through Ajloun's oak forest. Great for beginners and families — shaded most of the way and the castle viewpoint at the end is worth it.",
                ImageUrl = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=80",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-1)
            },
            new Post
            {
                AuthorId = omar.Id,
                Region = "Dana",
                LocationName = "Dana to Feynan",
                Body = "Two days descending from Dana village down to Feynan. From cool highlands to desert heat in one trail. One of the best routes in the country. Hire a local guide for the lower section.",
                ImageUrl = "https://images.unsplash.com/photo-1522163182402-834f871fd851?auto=format&fit=crop&w=1200&q=80",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-2)
            }
        };

        db.Posts.AddRange(posts);
        await db.SaveChangesAsync();

        db.Follows.AddRange(
            new Follow { FollowerId = lina.Id, FollowingId = omar.Id },
            new Follow { FollowerId = omar.Id, FollowingId = lina.Id },
            new Follow { FollowerId = admin.Id, FollowingId = lina.Id },
            new Follow { FollowerId = admin.Id, FollowingId = omar.Id });

        db.Comments.Add(new Comment
        {
            PostId = posts[0].Id,
            AuthorId = omar.Id,
            Body = "Sunset Ridge never disappoints. Did you catch the sunrise too?"
        });

        db.PostLikes.AddRange(
            new PostLike { PostId = posts[0].Id, UserId = omar.Id },
            new PostLike { PostId = posts[0].Id, UserId = admin.Id },
            new PostLike { PostId = posts[1].Id, UserId = lina.Id });

        posts[0].LikeCount = 2;
        posts[0].CommentCount = 1;
        posts[1].LikeCount = 1;

        await db.SaveChangesAsync();
    }
}
