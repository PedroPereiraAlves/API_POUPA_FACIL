using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Interfaces;

public interface IEmpresaRepository
{
    Task<bool> ExisteCnpjAsync(string cnpj, CancellationToken cancellationToken);

    Task<Empresa> AdicionarAsync(Empresa empresa, CancellationToken cancellationToken);
}
