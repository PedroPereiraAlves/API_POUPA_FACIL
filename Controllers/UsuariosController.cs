using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using API_POUPA_FACIL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_POUPA_FACIL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _usuariosRepository;

        public UsuariosController(IUsuarios usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarUsuario([FromBody] UsuarioViewModel usuarioViewModel)
        {

            var novoUsuario = new Usuarios
            {
                Nome = usuarioViewModel.Nome,
                Email = usuarioViewModel.Email,
                NumeroTelefone = usuarioViewModel.NumeroTelefone,
                Cpf = usuarioViewModel.Cpf,
                Senha = usuarioViewModel.Senha,
                DataCriacao = DateTime.UtcNow
            };



            var usuario = await _usuariosRepository.AdicionarUsuario(novoUsuario);

    
            return Ok(new { message = "Usuário cadastrado com sucesso", usuario });
        }
    }
}
