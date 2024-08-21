using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using API_POUPA_FACIL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_POUPA_FACIL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _usuariosRepository;
        private readonly IConfiguration _configuration;

        public UsuariosController(IUsuarios usuariosRepository, IConfiguration configuration)
        {
            _usuariosRepository = usuariosRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AdicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] UsuarioCreateViewModel usuarioViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(usuarioViewModel.Senha);

            var novoUsuario = new Usuarios
            {
                Nome = usuarioViewModel.Nome,
                Email = usuarioViewModel.Email,
                NumeroTelefone = usuarioViewModel.NumeroTelefone,
                Cpf = usuarioViewModel.Cpf,
                Senha = senhaCriptografada,
                DataCriacao = DateTime.UtcNow
            };

            var usuario = await _usuariosRepository.AdicionarUsuario(novoUsuario);

            return Ok(new { message = "Usuário cadastrado com sucesso", usuario.Nome });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLoginDTO loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuariosRepository.AuthenticaUsuario(loginRequest.Email, loginRequest.Senha);

            if(usuario is null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            var token = GenerateJwtToken(usuario);

            return Ok(new { token });

        }

        private string GenerateJwtToken(Usuarios usuario)
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
