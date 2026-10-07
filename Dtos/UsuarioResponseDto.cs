namespace API_POUPA_FACIL.Dtos;

public sealed class UsuarioResponseDto
{
    public int Codigo { get; init; }

    public required string Nome { get; init; }

    public required string Email { get; init; }
}
