using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_POUPA_FACIL.Services
{
    public class UsuarioService : IUsuariosRepository
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuariosRepository usuariosRepository, IConfiguration configuration)
        {
            _usuariosRepository = usuariosRepository;
            _configuration = configuration;
        }
        public Task<Usuarios> AdicionarUsuario(Usuarios usuario) 
               => _usuariosRepository.AdicionarUsuario(usuario);

        public Task<Usuarios> AuthenticaUsuario(string email, string senha) 
               => _usuariosRepository.AuthenticaUsuario(email, senha);

        public string GenerateJwtToken(Usuarios usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                new Claim(ClaimTypes.Name, usuario.Codigo.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
