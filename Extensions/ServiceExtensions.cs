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

        // Let the HTML encoder emit non-Latin scripts (Arabic) as raw UTF-8 instead of &#x…; entities.
        services.Configure<Microsoft.Extensions.WebEncoders.WebEncoderOptions>(options =>
        {
            options.TextEncoderSettings =
                new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All);
        });

        services.Configure<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>(options =>
        {
            var supported = new[]
            {
                new System.Globalization.CultureInfo("en"),
                new System.Globalization.CultureInfo("ar")
            };
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
            options.SupportedCultures = supported;
            options.SupportedUICultures = supported;
            // Language is chosen only via our cookie toggle (ignore Accept-Language header).
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new Microsoft.AspNetCore.Localization.CookieRequestCultureProvider());
        });

        services.AddScoped<IPasswordService, PasswordService>();
        services.Configure<FeatureOptions>(configuration.GetSection("Features"));
        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
