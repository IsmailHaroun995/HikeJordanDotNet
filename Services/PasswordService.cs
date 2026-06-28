using System.Security.Cryptography;

namespace HikeJordanDotNet.Services;

public class PasswordService : IPasswordService
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);

    public string GenerateTemporary() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(12));
}
