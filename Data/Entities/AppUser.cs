using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class AppUser
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Role { get; set; } = AppConstants.Roles.Member;

    [MaxLength(40)]
    public string ApprovalStatus { get; set; } = AppConstants.AccountStatus.Approved;

    [MaxLength(500)]
    public string Bio { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? AvatarUrl { get; set; }

    [MaxLength(400)]
    public string? CoverUrl { get; set; }

    [MaxLength(80)]
    public string Location { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    [MaxLength(64)]
    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailTokenGeneratedUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Post> Posts { get; set; } = [];
}
