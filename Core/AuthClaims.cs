using System.Security.Claims;
using HikeJordanDotNet.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HikeJordanDotNet.Core;

public static class AuthClaims
{
    public static ClaimsPrincipal Build(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("username", user.Username),
            new("avatar", user.AvatarUrl ?? string.Empty),
            new(AppConstants.ApprovalStatusClaim, user.ApprovalStatus)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}
