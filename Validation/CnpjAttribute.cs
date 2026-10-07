using System.ComponentModel.DataAnnotations;

namespace API_POUPA_FACIL.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CnpjAttribute : ValidationAttribute
{
    public CnpjAttribute() : base("Informe um CNPJ válido.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is not string texto || string.IsNullOrWhiteSpace(texto))
            return false;

        return DocumentoBrasileiro.CnpjValido(texto);
    }
}
