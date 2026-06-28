using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class TripReview
{
    public int Id { get; set; }

    public int HikeListingId { get; set; }

    [MaxLength(100)]
    public string ReviewerName { get; set; } = string.Empty;

    public int Rating { get; set; }

    [MaxLength(600)]
    public string ReviewText { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public HikeListing? Hike { get; set; }
}
