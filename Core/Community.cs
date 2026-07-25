using HikeJordanDotNet.Data;

namespace HikeJordanDotNet.Core;

public static class Community
{
    public static readonly string[] Regions =
    [
        "Wadi Rum", "Petra", "Dana", "Ajloun", "Dead Sea", "Wadi Mujib",
        "Aqaba", "Jerash", "Salt", "Amman", "Jordan Trail", "Other"
    ];

    public static string TimeAgo(DateTime utc)
    {
        var span = DateTime.UtcNow - utc;
        if (span.TotalSeconds < 60) return "just now";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours}h";
        if (span.TotalDays < 7) return $"{(int)span.TotalDays}d";
        if (span.TotalDays < 30) return $"{(int)(span.TotalDays / 7)}w";
        if (span.TotalDays < 365) return $"{(int)(span.TotalDays / 30)}mo";
        return $"{(int)(span.TotalDays / 365)}y";
    }

    public static string Initials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 1
            ? parts[0][..1].ToUpperInvariant()
            : (parts[0][..1] + parts[^1][..1]).ToUpperInvariant();
    }

    public static string RegionImage(string region) => region switch
    {
        "Wadi Rum" => "https://images.unsplash.com/photo-1548786811-dd6e453ccca7?auto=format&fit=crop&w=900&q=80",
        "Petra" => "https://images.unsplash.com/photo-1563177978-4c5ecd8f7f8b?auto=format&fit=crop&w=900&q=80",
        "Ajloun" => "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=900&q=80",
        "Dana" => "https://images.unsplash.com/photo-1522163182402-834f871fd851?auto=format&fit=crop&w=900&q=80",
        "Dead Sea" => "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=900&q=80",
        "Wadi Mujib" => "https://images.unsplash.com/photo-1504151932400-72d4384f04b3?auto=format&fit=crop&w=900&q=80",
        "Salt" => "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?auto=format&fit=crop&w=900&q=80",
        _ => "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=900&q=80"
    };
}
