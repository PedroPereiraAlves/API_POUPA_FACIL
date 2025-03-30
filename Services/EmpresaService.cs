using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Interfaces;

namespace API_POUPA_FACIL.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        public EmpresaService(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<Empresa> AdicionarEmpresa(Empresa empresa)
            => await _empresaRepository.AdicionarEmpresa(empresa);


    }
}
