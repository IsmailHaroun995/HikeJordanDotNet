using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

/// <summary>A review left for a group account, typically by a joiner who scanned the group's QR code.</summary>
public class GroupReview
{
    public int Id { get; set; }

    /// <summary>The group being reviewed (AppUser with AccountType = Group).</summary>
    public int GroupId { get; set; }

    [MaxLength(80)]
    public string ReviewerName { get; set; } = string.Empty;

    /// <summary>1–5 stars.</summary>
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    public bool IsHidden { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public AppUser Group { get; set; } = null!;
}
