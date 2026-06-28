using System.Text.RegularExpressions;

namespace HikeJordanDotNet.Services;

public class WhatsAppService : IWhatsAppService
{
    public string BookingUrl(string whatsApp, string hikeTitle)
    {
        var digits = Regex.Replace(whatsApp, @"[^\d]", "");
        var text = Uri.EscapeDataString($"Hi, I'd like to book the {hikeTitle} trip. Please share details.");
        return $"https://wa.me/{digits}?text={text}";
    }
}
