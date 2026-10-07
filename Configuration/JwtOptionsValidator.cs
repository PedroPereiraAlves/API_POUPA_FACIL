using System.Text;
using Microsoft.Extensions.Options;

namespace API_POUPA_FACIL.Configuration;

public sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    private readonly IHostEnvironment _environment;

    public JwtOptionsValidator(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Issuer))
            return ValidateOptionsResult.Fail("Jwt:Issuer is required.");

        if (string.IsNullOrWhiteSpace(options.Audience))
            return ValidateOptionsResult.Fail("Jwt:Audience is required.");

        if (options.ExpirationMinutes is < 5 or > 1440)
            return ValidateOptionsResult.Fail("Jwt:ExpirationMinutes must be between 5 and 1440.");

        if (string.IsNullOrWhiteSpace(options.Key))
            return ValidateOptionsResult.Fail(
                "Jwt:Key is required. Set Jwt__Key to a unique secret of at least 32 bytes.");

        if (Encoding.UTF8.GetByteCount(options.Key) < 32)
            return ValidateOptionsResult.Fail("Jwt:Key must be at least 32 bytes for HMAC-SHA256.");

        if (options.Key == JwtOptions.RevokedKey)
            return ValidateOptionsResult.Fail(
                "Jwt:Key was committed to source control and can no longer be used. Set a new Jwt__Key secret.");

        if (!_environment.IsDevelopment() && options.Key == JwtOptions.DevelopmentOnlyKey)
            return ValidateOptionsResult.Fail(
                "The development JWT key cannot be used outside Development. Set Jwt__Key.");

        return ValidateOptionsResult.Success;
    }
}
