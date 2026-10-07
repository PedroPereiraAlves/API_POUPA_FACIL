using System.Net;
using System.Net.Http.Json;

namespace API_POUPA_FACIL.Tests;

public class ApiSmokeTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CadastroInvalidoRetornaProblemDetails()
    {
        var response = await _client.PostAsJsonAsync("/api/usuarios", new { nome = "A" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
    }

    [Fact]
    public async Task RotaAntigaDeCadastroContinuaDisponivel()
    {
        var response = await _client.PostAsJsonAsync("/api/usuarios/AdicionarUsuario", new { nome = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EmpresaSemTokenRetornaNaoAutenticado()
    {
        var response = await _client.PostAsJsonAsync("/api/empresas", new
        {
            nome = "Acme",
            cnpj = "11.444.777/0001-61"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task RotaAntigaDeEmpresaTambemExigeToken()
    {
        var response = await _client.PostAsJsonAsync("/AdicionarEmpresa", new
        {
            nome = "Acme",
            cnpj = "11.444.777/0001-61"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task LoginInvalidoNaoVazaSeOEmailExiste()
    {
        var response = await _client.PostAsJsonAsync("/api/usuarios/login", new
        {
            email = "nao-e-email",
            senha = "12345678"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
