using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class ReviewFlag
{
    public int Id { get; set; }

    [MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Detail { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Priority { get; set; } = "Medium";

    [MaxLength(40)]
    public string Status { get; set; } = "Open";
}
