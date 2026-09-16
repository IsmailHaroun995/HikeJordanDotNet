using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class Notification
{
    public int Id { get; set; }

    /// <summary>Who receives the notification.</summary>
    public int RecipientId { get; set; }

    /// <summary>Who triggered it.</summary>
    public int ActorId { get; set; }

    /// <summary>"like", "follow" or "comment".</summary>
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    /// <summary>Related post, when applicable (like/comment).</summary>
    public int? PostId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public AppUser Actor { get; set; } = null!;
}
