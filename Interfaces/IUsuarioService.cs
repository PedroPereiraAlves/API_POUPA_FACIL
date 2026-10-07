using API_POUPA_FACIL.Dtos;

namespace API_POUPA_FACIL.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CadastrarAsync(UsuarioCreateDto request, CancellationToken cancellationToken);

    Task<LoginResponseDto?> AutenticarAsync(string email, string senha, CancellationToken cancellationToken);
}
