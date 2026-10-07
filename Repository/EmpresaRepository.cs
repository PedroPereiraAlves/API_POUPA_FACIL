using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_POUPA_FACIL.Repository;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly BaseContext _context;

    public EmpresaRepository(BaseContext context)
    {
        _context = context;
    }

    public Task<bool> ExisteCnpjAsync(string cnpj, CancellationToken cancellationToken)
    {
        return _context.Empresas.AsNoTracking().AnyAsync(
            empresa => empresa.Cnpj != null &&
                empresa.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "") == cnpj,
            cancellationToken);
    }

    public async Task<Empresa> AdicionarAsync(Empresa empresa, CancellationToken cancellationToken)
    {
        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync(cancellationToken);
        return empresa;
    }
}
