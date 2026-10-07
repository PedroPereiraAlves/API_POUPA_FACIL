using API_POUPA_FACIL.Dtos;
using API_POUPA_FACIL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API_POUPA_FACIL.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarios;

    public UsuariosController(IUsuarioService usuarios)
    {
        _usuarios = usuarios;
    }

    [AllowAnonymous]
    [HttpPost]
    [HttpPost("AdicionarUsuario")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cadastrar(
        [FromBody] UsuarioCreateDto request,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.CadastrarAsync(request, cancellationToken);
        return Created($"/api/usuarios/{usuario.Codigo}", usuario);
    }

    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var sessao = await _usuarios.AutenticarAsync(request.Email, request.Senha, cancellationToken);
        if (sessao is null)
        {
            return Problem(
                title: "Credenciais inválidas",
                detail: "E-mail ou senha inválidos.",
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return Ok(sessao);
    }
}
