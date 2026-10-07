using System.ComponentModel.DataAnnotations;
using API_POUPA_FACIL.Validation;

namespace API_POUPA_FACIL.Dtos;

public sealed class UsuarioCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 150 caracteres.")]
    public required string Nome { get; init; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(254, ErrorMessage = "O e-mail deve ter no máximo 254 caracteres.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Informe um telefone válido.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "O telefone deve ter entre 8 e 20 caracteres.")]
    public required string NumeroTelefone { get; init; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [Cpf]
    public required string Cpf { get; init; }

    // BCrypt only hashes the first 72 bytes.
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(72, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 72 caracteres.")]
    public required string Senha { get; init; }
}
