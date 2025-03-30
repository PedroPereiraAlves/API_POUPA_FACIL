using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Classes;
using Microsoft.EntityFrameworkCore;

namespace API_POUPA_FACIL.Repository
{
    public class UsuarioRepository : IUsuariosRepository
    {
        private readonly BaseContext _context;

        public UsuarioRepository(BaseContext context)
        {
            _context = context;
        }

        public async Task<Usuarios> AdicionarUsuario(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuarios> AuthenticaUsuario(string email, string senha)
        {
            var Usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (Usuario is null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(senha, Usuario.Senha))
                return null;
            

            return Usuario;
        }
    }
}
