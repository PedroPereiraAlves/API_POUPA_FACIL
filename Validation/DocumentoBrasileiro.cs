namespace API_POUPA_FACIL.Validation;

public static class DocumentoBrasileiro
{
    public static bool CpfValido(string valor)
    {
        var digitos = SomenteDigitos(valor);
        if (digitos.Length != 11 || TodosIguais(digitos))
            return false;

        return DigitoVerificador(digitos, 9, 10) == digitos[9] - '0'
            && DigitoVerificador(digitos, 10, 11) == digitos[10] - '0';
    }

    public static bool CnpjValido(string valor)
    {
        var digitos = SomenteDigitos(valor);
        if (digitos.Length != 14 || TodosIguais(digitos))
            return false;

        var primeiro = DigitoCnpj(digitos, 12);
        var segundo = DigitoCnpj(digitos, 13);
        return primeiro == digitos[12] - '0' && segundo == digitos[13] - '0';
    }

    public static string SomenteDigitos(string valor)
        => new(valor.Where(char.IsDigit).ToArray());

    private static int DigitoVerificador(string digitos, int tamanho, int pesoInicial)
    {
        var soma = 0;
        for (var i = 0; i < tamanho; i++)
            soma += (digitos[i] - '0') * (pesoInicial - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    private static int DigitoCnpj(string digitos, int tamanho)
    {
        var peso = tamanho - 7;
        var soma = 0;
        for (var i = 0; i < tamanho; i++)
        {
            soma += (digitos[i] - '0') * peso;
            peso = peso == 2 ? 9 : peso - 1;
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    private static bool TodosIguais(string digitos)
    {
        for (var i = 1; i < digitos.Length; i++)
        {
            if (digitos[i] != digitos[0])
                return false;
        }

        return true;
    }
}
