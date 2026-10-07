using Npgsql;

namespace API_POUPA_FACIL.Infrastructure;

public static class DatabaseConnection
{
    public static string Resolve(IConfiguration configuration, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrWhiteSpace(databaseUrl))
                throw new InvalidOperationException("DATABASE_URL environment variable is not set.");

            return FromDatabaseUrl(databaseUrl);
        }

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. Set ConnectionStrings__DefaultConnection or a user secret.");
        }

        return connectionString;
    }

    internal static string FromDatabaseUrl(string databaseUrl)
    {
        if (!Uri.TryCreate(databaseUrl, UriKind.Absolute, out var databaseUri))
            throw new InvalidOperationException("DATABASE_URL is not a valid URI.");

        var userInfo = databaseUri.UserInfo.Split(':', 2);
        if (userInfo.Length == 0 || string.IsNullOrWhiteSpace(userInfo[0]))
            throw new InvalidOperationException("DATABASE_URL is missing a username.");

        var database = databaseUri.LocalPath.TrimStart('/');
        if (string.IsNullOrWhiteSpace(database))
            throw new InvalidOperationException("DATABASE_URL is missing a database name.");

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.IsDefaultPort ? 5432 : databaseUri.Port,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
            Database = database,
            SslMode = ResolveSslMode(databaseUri)
        };

        return builder.ConnectionString;
    }

    private static SslMode ResolveSslMode(Uri databaseUri)
    {
        var requested = ReadQueryValue(databaseUri.Query, "sslmode");
        if (requested is not null && TryMapSslMode(requested, out var parsed))
            return parsed;

        // Public hostnames (for example Render external URLs) must not fall back to cleartext.
        // Internal hostnames without a dot keep Prefer so private network links still start.
        return databaseUri.Host.Contains('.') ? SslMode.Require : SslMode.Prefer;
    }

    private static bool TryMapSslMode(string value, out SslMode sslMode)
    {
        switch (value.Trim().ToLowerInvariant())
        {
            case "disable":
                sslMode = SslMode.Disable;
                return true;
            case "allow":
                sslMode = SslMode.Allow;
                return true;
            case "prefer":
                sslMode = SslMode.Prefer;
                return true;
            case "require":
                sslMode = SslMode.Require;
                return true;
            case "verify-ca":
                sslMode = SslMode.VerifyCA;
                return true;
            case "verify-full":
                sslMode = SslMode.VerifyFull;
                return true;
            default:
                sslMode = default;
                return false;
        }
    }

    private static string? ReadQueryValue(string query, string key)
    {
        if (string.IsNullOrEmpty(query))
            return null;

        var trimmed = query.TrimStart('?');
        foreach (var pair in trimmed.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var separator = pair.IndexOf('=');
            if (separator <= 0)
                continue;

            var name = Uri.UnescapeDataString(pair[..separator]);
            if (!name.Equals(key, StringComparison.OrdinalIgnoreCase))
                continue;

            return Uri.UnescapeDataString(pair[(separator + 1)..]);
        }

        return null;
    }
}
