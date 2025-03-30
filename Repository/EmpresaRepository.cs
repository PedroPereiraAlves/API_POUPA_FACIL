using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Interfaces;
namespace API_POUPA_FACIL.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly BaseContext _context;

        public EmpresaRepository(BaseContext context)
        {
            _context = context;
        }

        public async Task<Empresa> AdicionarEmpresa(Empresa empresa)
        {
            try
            {
                _context.Empresa.Add(empresa);

                await _context.SaveChangesAsync();

                return empresa;

            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
