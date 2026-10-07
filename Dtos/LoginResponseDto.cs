namespace API_POUPA_FACIL.Dtos;

public sealed class LoginResponseDto
{
    public required string Token { get; init; }

    public DateTime ExpiraEm { get; init; }
}
