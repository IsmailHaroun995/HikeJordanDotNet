using QRCoder;

namespace HikeJordanDotNet.Core;

public static class QrCode
{
    /// <summary>Returns a PNG data-URI QR code for the given text (embeddable in an &lt;img src&gt;).</summary>
    public static string DataUri(string content, int pixelsPerModule = 8)
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var png = new PngByteQRCode(data);
        var bytes = png.GetGraphic(pixelsPerModule);
        return "data:image/png;base64," + Convert.ToBase64String(bytes);
    }
}
