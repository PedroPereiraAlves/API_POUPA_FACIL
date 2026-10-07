namespace API_POUPA_FACIL.Dtos;

public sealed class EmpresaResponseDto
{
    public int Codigo { get; init; }

    public required string Nome { get; init; }

    public required string Cnpj { get; init; }
}
