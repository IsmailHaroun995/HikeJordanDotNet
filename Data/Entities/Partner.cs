using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class Partner
{
    public int Id { get; set; }

    [MaxLength(140)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? ImageUrl { get; set; }

    [MaxLength(120)]
    public string? InstagramPage { get; set; }

    public bool IsActive { get; set; } = true;
}
