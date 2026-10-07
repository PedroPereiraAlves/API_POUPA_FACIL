using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Configuration;
using API_POUPA_FACIL.Dtos;
using API_POUPA_FACIL.Exceptions;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Validation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API_POUPA_FACIL.Services;

public class UsuarioService : IUsuarioService
{
    private const int BcryptWorkFactor = 12;

    private static readonly string DummyPasswordHash = BCrypt.Net.BCrypt.HashPassword(
        "dummy-password-not-used",
        workFactor: BcryptWorkFactor);

    private readonly IUsuarioRepository _usuarios;
    private readonly JwtOptions _jwt;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(
        IUsuarioRepository usuarios,
        IOptions<JwtOptions> jwt,
        ILogger<UsuarioService> logger)
    {
        _usuarios = usuarios;
        _jwt = jwt.Value;
        _logger = logger;
    }

    public async Task<UsuarioResponseDto> CadastrarAsync(
        UsuarioCreateDto request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existente = await _usuarios.ObterPorEmailAsync(email, cancellationToken);
        if (existente is not null)
            throw new ConflictException("Já existe um usuário com este e-mail.");

        var usuario = new Usuarios
        {
            Nome = request.Nome.Trim(),
            Email = email,
            NumeroTelefone = request.NumeroTelefone.Trim(),
            Cpf = DocumentoBrasileiro.SomenteDigitos(request.Cpf),
            Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha, workFactor: BcryptWorkFactor),
            DataCriacao = DateTime.UtcNow
        };

        var salvo = await _usuarios.AdicionarAsync(usuario, cancellationToken);
        return new UsuarioResponseDto
        {
            Codigo = salvo.Codigo,
            Nome = salvo.Nome,
            Email = salvo.Email
        };
    }

    public async Task<LoginResponseDto?> AutenticarAsync(
        string email,
        string senha,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObterPorEmailAsync(email, cancellationToken);
        if (usuario is null)
        {
            BCrypt.Net.BCrypt.Verify(senha, DummyPasswordHash);
            _logger.LogWarning("Falha de autenticação.");
            return null;
        }

        var senhaValida = false;
        try
        {
            senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
        }
        catch (Exception exception) when (exception is BCrypt.Net.SaltParseException or ArgumentException)
        {
            _logger.LogWarning("Hash de senha inválido para o usuário {UsuarioId}.", usuario.Codigo);
        }

        if (!senhaValida)
        {
            _logger.LogWarning("Falha de autenticação.");
            return null;
        }

        return GerarToken(usuario);
    }

    private LoginResponseDto GerarToken(Usuarios usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiraEm = DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes);
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Codigo.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ],
            expires: expiraEm,
            signingCredentials: credentials);

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEm = expiraEm
        };
    }
}
