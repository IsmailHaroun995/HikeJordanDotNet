using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HikeJordanDotNet.Services;

public sealed class EmailOptions
{
    public string SmtpHost { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = "Hike Jordan";
    public string AdminEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class EmailService(IOptions<EmailOptions> opts, ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailOptions _o = opts.Value;

    public Task NotifyOrganizerSignupAsync(string organizerEmail, string organizerName) =>
        SendAsync(organizerEmail,
            "Welcome to Hike Jordan — application received",
            $"""
            <div style="font-family:sans-serif;max-width:520px">
              <h2 style="color:#26483b">Hi {organizerName},</h2>
              <p>We've received your organizer application on <strong>Hike Jordan</strong>.</p>
              <p>Our team will review your profile and get back to you shortly.
                 Once approved you can start submitting trips.</p>
              <p style="color:#666">— The Hike Jordan team</p>
            </div>
            """);

    public Task NotifyAdminNewOrganizerAsync(string organizerName, string organizerEmail, string whatsApp, string regions) =>
        SendAsync(_o.AdminEmail,
            $"New organizer application: {organizerName}",
            $"""
            <div style="font-family:sans-serif;max-width:520px">
              <h2 style="color:#26483b">New organizer application</h2>
              <table style="border-collapse:collapse;width:100%">
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Name</td><td>{organizerName}</td></tr>
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Email</td><td>{organizerEmail}</td></tr>
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">WhatsApp</td><td>{whatsApp}</td></tr>
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Regions</td><td>{regions}</td></tr>
              </table>
              <p style="margin-top:16px">Log in to the admin panel to approve or request docs.</p>
            </div>
            """);

    public Task NotifyOrganizerNewTripAsync(string organizerEmail, string organizerName, string tripTitle) =>
        SendAsync(organizerEmail,
            $"Trip submitted for review: {tripTitle}",
            $"""
            <div style="font-family:sans-serif;max-width:520px">
              <h2 style="color:#26483b">Hi {organizerName},</h2>
              <p>Your trip <strong>"{tripTitle}"</strong> has been submitted and is awaiting admin review.</p>
              <p>We'll notify you once it's live on Hike Jordan.</p>
              <p style="color:#666">— The Hike Jordan team</p>
            </div>
            """);

    public Task NotifyAdminNewTripAsync(string organizerName, string organizerEmail, string tripTitle) =>
        SendAsync(_o.AdminEmail,
            $"New trip submitted: {tripTitle}",
            $"""
            <div style="font-family:sans-serif;max-width:520px">
              <h2 style="color:#26483b">New trip submitted</h2>
              <table style="border-collapse:collapse;width:100%">
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Trip</td><td>{tripTitle}</td></tr>
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Organizer</td><td>{organizerName}</td></tr>
                <tr><td style="padding:6px 12px 6px 0;font-weight:600">Email</td><td>{organizerEmail}</td></tr>
              </table>
              <p style="margin-top:16px">Log in to the admin panel to approve or reject.</p>
            </div>
            """);

    public Task NotifyOrganizerDocsRequestedAsync(string organizerEmail, string organizerName) =>
        SendAsync(organizerEmail,
            "Action needed: additional documents required — Hike Jordan",
            $"""
            <div style="font-family:sans-serif;max-width:520px">
              <h2 style="color:#26483b">Hi {organizerName},</h2>
              <p>Thank you for applying to be an organizer on <strong>Hike Jordan</strong>.</p>
              <p>Our team has reviewed your application and needs a few additional documents
                 before we can approve your account. Please reply to this email with:</p>
              <ul>
                <li>A copy of your ID or business registration</li>
                <li>Proof of past trip experience (photos, links, references)</li>
                <li>Any safety certifications you hold</li>
              </ul>
              <p>Once we receive your documents we'll complete the review quickly.</p>
              <p style="color:#666">— The Hike Jordan team</p>
            </div>
            """);

    private async Task SendAsync(string to, string subject, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(_o.SenderEmail) || string.IsNullOrWhiteSpace(_o.Password))
        {
            logger.LogWarning("Email not configured — skipping send to {To} ({Subject})", to, subject);
            return;
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_o.SenderName, _o.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(_o.SmtpHost, _o.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_o.SenderEmail, _o.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            logger.LogInformation("Email sent to {To}: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {To}: {Subject}", to, subject);
        }
    }
}
