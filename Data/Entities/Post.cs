using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class Post
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    [MaxLength(2000)]
    public string Body { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Region { get; set; } = string.Empty;

    [MaxLength(120)]
    public string LocationName { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? ImageUrl { get; set; }

    public int LikeCount { get; set; }

    public int CommentCount { get; set; }

    public bool IsHidden { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public AppUser Author { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<PostLike> Likes { get; set; } = [];
}
