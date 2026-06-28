using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class OrganizerRequest
{
    public int Id { get; set; }

    [MaxLength(140)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(40)]
    public string WhatsApp { get; set; } = string.Empty;

    [MaxLength(220)]
    public string Regions { get; set; } = string.Empty;

    [MaxLength(900)]
    public string Experience { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Status { get; set; } = AppConstants.AccountStatus.Submitted;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
