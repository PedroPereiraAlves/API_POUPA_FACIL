using System.ComponentModel.DataAnnotations;

namespace API_POUPA_FACIL.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CpfAttribute : ValidationAttribute
{
    public CpfAttribute() : base("Informe um CPF válido.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is not string texto || string.IsNullOrWhiteSpace(texto))
            return false;

        return DocumentoBrasileiro.CpfValido(texto);
    }
}
