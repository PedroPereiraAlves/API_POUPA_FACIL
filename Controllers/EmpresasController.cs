using Microsoft.AspNetCore.Mvc;
using API_POUPA_FACIL.Services;
using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.ViewModels;
using API_POUPA_FACIL.Interfaces;

namespace API_POUPA_FACIL.Controllers
{
    public class EmpresasController : ControllerBase
    {
        private IEmpresaService _empresaServices;
        public EmpresasController(IEmpresaService empresaService)
        {
            _empresaServices = empresaService;
        }

        [HttpPost]
        [Route("AdicionarEmpresa")]
        public async Task<IActionResult> AdicionarEmpresa([FromBody] EmpresaCreateDto empresa)
        {
            if(!ModelState.IsValid)
                return Unauthorized(ModelState);

            var novaEmpresa = new Empresa
            {
                Nome = empresa.Nome,
                Cnpj = empresa.Cnpj
            };

            var command = await _empresaServices.AdicionarEmpresa(novaEmpresa);

            if (command is Empresa)
                return Ok("Empresa Cadastrada com sucesso!");


            return BadRequest("A empresa não foi cadastrada");
        }
    }
}
