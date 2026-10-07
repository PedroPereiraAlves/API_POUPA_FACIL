namespace API_POUPA_FACIL.Exceptions;

public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
