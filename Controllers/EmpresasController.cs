using API_POUPA_FACIL.Dtos;
using API_POUPA_FACIL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_POUPA_FACIL.Controllers;

[ApiController]
[Authorize]
[Route("api/empresas")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _empresas;

    public EmpresasController(IEmpresaService empresas)
    {
        _empresas = empresas;
    }

    [HttpPost]
    [HttpPost("AdicionarEmpresa")]
    [HttpPost("/AdicionarEmpresa")]
    [ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cadastrar(
        [FromBody] EmpresaCreateDto request,
        CancellationToken cancellationToken)
    {
        var empresa = await _empresas.CadastrarAsync(request, cancellationToken);
        return Created($"/api/empresas/{empresa.Codigo}", empresa);
    }
}
