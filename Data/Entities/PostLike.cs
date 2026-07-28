namespace HikeJordanDotNet.Data;

public class PostLike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; } = null!;
}
