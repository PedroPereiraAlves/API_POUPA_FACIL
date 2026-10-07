namespace API_POUPA_FACIL.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// Local signing key shipped only in appsettings.Development.json.
    /// Rejected outside Development.
    /// </summary>
    public const string DevelopmentOnlyKey = "DEV-ONLY-poupa-facil-signing-key-32b";

    /// <summary>
    /// Previous key that was committed to the repository. It must not sign tokens again.
    /// </summary>
    public const string RevokedKey = "PEDROJOSEGABRIEL1234567890123456";

    public string Issuer { get; set; } = "API_POUPA_FACIL";

    public string Audience { get; set; } = "API_POUPA_FACIL";

    public string Key { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; } = 60;
}
