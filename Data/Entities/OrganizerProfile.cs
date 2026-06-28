using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class OrganizerProfile
{
    public int Id { get; set; }

    [MaxLength(140)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Status { get; set; } = AppConstants.AccountStatus.Pending;

    [MaxLength(20)]
    public string Rating { get; set; } = string.Empty;

    [MaxLength(40)]
    public string PastTrips { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Note { get; set; } = string.Empty;
}
