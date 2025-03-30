using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<Usuarios> AdicionarUsuario(Usuarios usuario);
        Task<Usuarios> AuthenticaUsuario(string email, string senha);
    }
}
