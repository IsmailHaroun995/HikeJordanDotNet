namespace HikeJordanDotNet.Core;

public static class AppConstants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Organizer = "Organizer";
        public const string Visitor = "Visitor";
    }

    public static class HikeStatus
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string Approved = "Approved";
        public const string Published = "Published";
        public const string Hidden = "Hidden";
        public const string Rejected = "Rejected";
        public const string Archived = "Archived";
        public const string Completed = "Completed";
    }

    public static class AccountStatus
    {
        public const string Approved = "Approved";
        public const string Pending = "Pending";
        public const string Rejected = "Rejected";
        public const string Disabled = "Disabled";
        public const string Verified = "Verified";
        public const string Submitted = "Submitted";
    }

    public const string AuthCookieName = "HikeJordan.Auth";
    public const string DefaultConnectionName = "DefaultConnection";
    public const string LoginRateLimitPolicy = "login";
    public const string ApprovalStatusClaim = "approval_status";
}
