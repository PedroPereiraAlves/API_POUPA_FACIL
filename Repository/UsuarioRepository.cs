using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_POUPA_FACIL.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly BaseContext _context;

    public UsuarioRepository(BaseContext context)
    {
        _context = context;
    }

    public Task<Usuarios?> ObterPorEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Email.ToLower() == normalized, cancellationToken);
    }

    public async Task<Usuarios> AdicionarAsync(Usuarios usuario, CancellationToken cancellationToken)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);
        return usuario;
    }
}
