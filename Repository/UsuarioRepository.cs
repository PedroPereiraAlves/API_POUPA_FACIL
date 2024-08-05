using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Connections;

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
