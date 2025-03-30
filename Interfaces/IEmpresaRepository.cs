using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces
{
    public interface IEmpresaRepository
    {
        Task<Empresa> AdicionarEmpresa(Empresa empresa);
    }
}
