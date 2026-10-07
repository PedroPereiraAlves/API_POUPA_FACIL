using API_POUPA_FACIL.Classes;
using API_POUPA_FACIL.Dtos;
using API_POUPA_FACIL.Exceptions;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Validation;

namespace API_POUPA_FACIL.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _empresas;

    public EmpresaService(IEmpresaRepository empresas)
    {
        _empresas = empresas;
    }

    public async Task<EmpresaResponseDto> CadastrarAsync(
        EmpresaCreateDto request,
        CancellationToken cancellationToken)
    {
        var cnpj = DocumentoBrasileiro.SomenteDigitos(request.Cnpj);
        if (await _empresas.ExisteCnpjAsync(cnpj, cancellationToken))
            throw new ConflictException("Já existe uma empresa com este CNPJ.");

        var empresa = new Empresa
        {
            Nome = request.Nome.Trim(),
            Cnpj = cnpj
        };

        var salva = await _empresas.AdicionarAsync(empresa, cancellationToken);
        return new EmpresaResponseDto
        {
            Codigo = salva.Codigo,
            Nome = salva.Nome ?? string.Empty,
            Cnpj = salva.Cnpj ?? cnpj
        };
    }
}
