using System.ComponentModel.DataAnnotations;
using API_POUPA_FACIL.Validation;

namespace API_POUPA_FACIL.Dtos;

public sealed class EmpresaCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 150 caracteres.")]
    public required string Nome { get; init; }

    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [Cnpj]
    public required string Cnpj { get; init; }
}
