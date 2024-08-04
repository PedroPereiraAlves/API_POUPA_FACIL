using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.ViewModels;

namespace API_POUPA_FACIL.Repository
{
    public class UsuarioRepository : IUsuarios
    {
        private readonly BaseContext _context = new BaseContext();

        public async Task<Usuarios> AdicionarUsuario(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }
    }
}
