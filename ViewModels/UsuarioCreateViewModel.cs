namespace API_POUPA_FACIL.ViewModels
{
    public class UsuarioCreateViewModel
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string NumeroTelefone { get; set; }
        public required string Cpf { get; set; }
        public required string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
