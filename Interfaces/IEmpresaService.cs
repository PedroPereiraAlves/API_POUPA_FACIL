using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces
{
    public interface IEmpresaService
    {
        Task<Empresa> AdicionarEmpresa(Empresa empresa);
    }
}
