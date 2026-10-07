using API_POUPA_FACIL.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace API_POUPA_FACIL.Tests;

public class JwtOptionsValidatorTests
{
    [Fact]
    public void RejeitaChaveQueVazouNoRepositorio()
    {
        var validator = new JwtOptionsValidator(new TestEnvironment(Environments.Development));
        var result = validator.Validate(null, Options(JwtOptions.RevokedKey));

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void AceitaChaveDeDesenvolvimentoApenasEmDevelopment()
    {
        var development = new JwtOptionsValidator(new TestEnvironment(Environments.Development));
        var production = new JwtOptionsValidator(new TestEnvironment(Environments.Production));

        Assert.True(development.Validate(null, Options(JwtOptions.DevelopmentOnlyKey)).Succeeded);
        Assert.False(production.Validate(null, Options(JwtOptions.DevelopmentOnlyKey)).Succeeded);
    }

    [Fact]
    public void RejeitaChaveCurta()
    {
        var validator = new JwtOptionsValidator(new TestEnvironment(Environments.Production));
        var result = validator.Validate(null, Options("curta-demais"));

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void AceitaChaveUnicaEmProducao()
    {
        var validator = new JwtOptionsValidator(new TestEnvironment(Environments.Production));
        var result = validator.Validate(null, Options("uma-chave-unica-com-32-bytes-ou-mais"));

        Assert.True(result.Succeeded);
    }

    private static JwtOptions Options(string key) => new()
    {
        Issuer = "API_POUPA_FACIL",
        Audience = "API_POUPA_FACIL",
        ExpirationMinutes = 60,
        Key = key
    };

    private sealed class TestEnvironment : IHostEnvironment
    {
        public TestEnvironment(string environmentName) => EnvironmentName = environmentName;

        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; } = "tests";

        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
