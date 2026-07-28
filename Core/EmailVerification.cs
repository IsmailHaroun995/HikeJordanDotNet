using System.Security.Cryptography;
using HikeJordanDotNet.Data;
using HikeJordanDotNet.Services;
using Microsoft.Extensions.Logging;

namespace HikeJordanDotNet.Core;

public static class EmailVerification
{
    public static readonly TimeSpan Lifetime = TimeSpan.FromHours(24);

    public static string NewToken() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public static bool IsExpired(DateTime? generatedUtc) =>
        generatedUtc is null || DateTime.UtcNow - generatedUtc.Value > Lifetime;

    /// <summary>Generates a fresh token, persists it, logs the link (dev aid) and emails it.</summary>
    public static async Task IssueAndSendAsync(
        HikeJordanDbContext db,
        IEmailService email,
        ILogger logger,
        AppUser user,
        string baseUrl)
    {
        user.EmailVerificationToken = NewToken();
        user.EmailTokenGeneratedUtc = DateTime.UtcNow;
        await db.SaveChangesAsync();

        var verifyUrl = $"{baseUrl}/verify-email?token={user.EmailVerificationToken}";
        logger.LogInformation("Email verification link for {Email}: {Url}", user.Email, verifyUrl);

        await email.SendEmailVerificationAsync(user.Email, user.Name, verifyUrl);
    }
}
