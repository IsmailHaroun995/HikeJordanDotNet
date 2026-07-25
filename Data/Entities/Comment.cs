using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class Comment
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int AuthorId { get; set; }

    [MaxLength(1000)]
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; } = null!;

    public AppUser Author { get; set; } = null!;
}
