using System.ComponentModel.DataAnnotations;

namespace HikeJordanDotNet.Data;

public class AppUser
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(40)]
    public string Role { get; set; } = AppConstants.Roles.Visitor;

    [MaxLength(40)]
    public string ApprovalStatus { get; set; } = AppConstants.AccountStatus.Pending;
}
