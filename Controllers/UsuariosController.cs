using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using API_POUPA_FACIL.Services;
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
        private readonly UsuarioService _usuariosServices;

        public UsuariosController(UsuarioService usuariosServices)
        {
            _usuariosServices = usuariosServices;
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

            var usuario = await _usuariosServices.AdicionarUsuario(novoUsuario);

            return Ok(new { message = "Usuário cadastrado com sucesso", usuario.Nome });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLoginDTO loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuariosServices.AuthenticaUsuario(loginRequest.Email, loginRequest.Senha);

            if(usuario is null)
                return Unauthorized(new { message = "Email ou senha inválidos" });

            var token = _usuariosServices.GenerateJwtToken(usuario);

            return Ok(new { token });
        }

    }
}
