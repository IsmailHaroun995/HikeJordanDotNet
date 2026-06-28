namespace HikeJordanDotNet.Services;

public interface IEmailService
{
    Task NotifyOrganizerSignupAsync(string organizerEmail, string organizerName);
    Task NotifyAdminNewOrganizerAsync(string organizerName, string organizerEmail, string whatsApp, string regions);
    Task NotifyOrganizerNewTripAsync(string organizerEmail, string organizerName, string tripTitle);
    Task NotifyAdminNewTripAsync(string organizerName, string organizerEmail, string tripTitle);
    Task NotifyOrganizerDocsRequestedAsync(string organizerEmail, string organizerName);
}
