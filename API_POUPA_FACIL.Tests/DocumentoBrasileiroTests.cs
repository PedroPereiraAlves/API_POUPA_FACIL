using API_POUPA_FACIL.Validation;

namespace API_POUPA_FACIL.Tests;

public class DocumentoBrasileiroTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void AceitaCpfValido(string cpf)
    {
        Assert.True(DocumentoBrasileiro.CpfValido(cpf));
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("529.982.247-26")]
    [InlineData("123")]
    [InlineData("   ")]
    public void RejeitaCpfInvalido(string cpf)
    {
        Assert.False(DocumentoBrasileiro.CpfValido(cpf));
    }

    [Theory]
    [InlineData("11.444.777/0001-61")]
    [InlineData("11444777000161")]
    public void AceitaCnpjValido(string cnpj)
    {
        Assert.True(DocumentoBrasileiro.CnpjValido(cnpj));
    }

    [Theory]
    [InlineData("11.444.777/0001-62")]
    [InlineData("00000000000000")]
    [InlineData("1144477700016")]
    public void RejeitaCnpjInvalido(string cnpj)
    {
        Assert.False(DocumentoBrasileiro.CnpjValido(cnpj));
    }
}
