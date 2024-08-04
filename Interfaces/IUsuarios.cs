using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces
{
    public interface IUsuarios
    {
        Task<Usuarios> AdicionarUsuario(Usuarios usuario);
    }
}
