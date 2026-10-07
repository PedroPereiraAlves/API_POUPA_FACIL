using API_POUPA_FACIL.Dtos;

namespace API_POUPA_FACIL.Interfaces;

public interface IEmpresaService
{
    Task<EmpresaResponseDto> CadastrarAsync(EmpresaCreateDto request, CancellationToken cancellationToken);
}
