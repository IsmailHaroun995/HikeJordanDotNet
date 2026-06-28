using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Data;

public class HikeListing
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(140)]
    public string Organizer { get; set; } = string.Empty;

    [MaxLength(60)]
    public string WhatsApp { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Region { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Difficulty { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Status { get; set; } = AppConstants.HikeStatus.Submitted;

    [MaxLength(40)]
    public string DateLabel { get; set; } = string.Empty;

    [MaxLength(10)]
    public string TimeLabel { get; set; } = string.Empty;

    public int SpotsLeft { get; set; }

    [Precision(10, 2)]
    public decimal Price { get; set; }

    public int? DurationHours { get; set; }

    [Precision(6, 2)]
    public decimal? DistanceKm { get; set; }

    [MaxLength(700)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(160)]
    public string MeetingPoint { get; set; } = string.Empty;

    [MaxLength(300)]
    public string RequiredGear { get; set; } = string.Empty;

    [MaxLength(300)]
    public string IncludedItems { get; set; } = string.Empty;

    [MaxLength(300)]
    public string ExcludedItems { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Note { get; set; } = string.Empty;

    public DateTime? TripDate { get; set; }

    [MaxLength(60)]
    public string PaymentType { get; set; } = string.Empty;

    [MaxLength(120)]
    public string GroupName { get; set; } = string.Empty;

    [MaxLength(160)]
    public string InstagramPage { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? ImagePath { get; set; }
}
