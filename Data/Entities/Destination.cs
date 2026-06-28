using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class Destination
{
    public int Id { get; set; }

    [MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? CoverImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}
