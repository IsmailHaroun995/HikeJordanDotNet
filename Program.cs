using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddHikeJordanServices(builder.Configuration, builder.Environment);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "0";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
        await next();
    });

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseStaticFiles();
    app.UseRequestLocalization();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    // Language toggle: sets the culture cookie, then returns to the current page.
    app.MapGet("/set-language/{culture}", (string culture, string? redirect, HttpContext ctx) =>
    {
        var chosen = culture == "ar" ? "ar" : "en";
        ctx.Response.Cookies.Append(
            Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
            Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(
                new Microsoft.AspNetCore.Localization.RequestCulture(chosen)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, HttpOnly = false });

        var target = !string.IsNullOrEmpty(redirect) && redirect.StartsWith('/') && !redirect.StartsWith("//")
            ? redirect
            : "/";
        return Results.LocalRedirect(target);
    });

    app.MapRazorPages();
    app.MapHealthChecks("/health");

    await HikeJordanDotNet.Data.DatabaseSeeder.InitializeAsync(app.Services);

    Log.Information("HikeJordan started on {Environment}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
