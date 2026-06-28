using HikeJordanDotNet.Data;
using HikeJordanDotNet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace HikeJordanDotNet.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddHikeJordanServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString(AppConstants.DefaultConnectionName)
            ?? throw new InvalidOperationException(
                $"Connection string '{AppConstants.DefaultConnectionName}' not found.");

        services.AddDbContext<HikeJordanDbContext>(options =>
            options.UseSqlServer(connectionString));

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = AppConstants.AuthCookieName;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

        services.AddRazorPages();
        services.AddHealthChecks();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IWhatsAppService, WhatsAppService>();
        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
