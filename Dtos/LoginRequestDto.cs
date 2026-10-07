using System.ComponentModel.DataAnnotations;

namespace API_POUPA_FACIL.Dtos;

public sealed class LoginRequestDto
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(254, ErrorMessage = "O e-mail deve ter no máximo 254 caracteres.")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(72, MinimumLength = 1, ErrorMessage = "A senha informada é inválida.")]
    public required string Senha { get; init; }
}
