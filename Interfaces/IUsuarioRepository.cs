using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuarios?> ObterPorEmailAsync(string email, CancellationToken cancellationToken);

    Task<Usuarios> AdicionarAsync(Usuarios usuario, CancellationToken cancellationToken);
}
