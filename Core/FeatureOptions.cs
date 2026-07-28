namespace HikeJordanDotNet.Core;

public sealed class FeatureOptions
{
    /// <summary>When true, new accounts must confirm their email before they can sign in.</summary>
    public bool RequireEmailVerification { get; set; } = true;
}
