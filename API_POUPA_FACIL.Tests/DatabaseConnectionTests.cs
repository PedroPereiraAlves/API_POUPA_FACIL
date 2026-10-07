using API_POUPA_FACIL.Infrastructure;
using Npgsql;

namespace API_POUPA_FACIL.Tests;

public class DatabaseConnectionTests
{
    [Fact]
    public void DecodificaSenhaEExigeSslEmHostPublico()
    {
        var connectionString = DatabaseConnection.FromDatabaseUrl(
            "postgres://appuser:p%40ss:word@db.example.com:5433/poupa?sslmode=require");

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal(5433, builder.Port);
        Assert.Equal("appuser", builder.Username);
        Assert.Equal("p@ss:word", builder.Password);
        Assert.Equal("poupa", builder.Database);
        Assert.Equal(SslMode.Require, builder.SslMode);
    }

    [Fact]
    public void HostExternoSemSslModeUsaRequire()
    {
        var connectionString = DatabaseConnection.FromDatabaseUrl(
            "postgres://appuser:secret@dpg-abc.oregon-postgres.render.com/poupa");

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        Assert.Equal(SslMode.Require, builder.SslMode);
        Assert.Equal(5432, builder.Port);
    }

    [Fact]
    public void HostInternoSemPontoMantemPrefer()
    {
        var connectionString = DatabaseConnection.FromDatabaseUrl(
            "postgres://appuser:secret@dpg-abc-a/poupa");

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        Assert.Equal(SslMode.Prefer, builder.SslMode);
    }

    [Fact]
    public void RejeitaUrlSemBanco()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            DatabaseConnection.FromDatabaseUrl("postgres://appuser:secret@localhost"));

        Assert.Contains("database name", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
