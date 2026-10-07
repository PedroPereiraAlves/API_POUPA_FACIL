using System.Text.Json.Serialization;

namespace API_POUPA_FACIL.Classes
{
    public class Usuarios
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string NumeroTelefone {  get; set; }
        public required string Cpf { get; set; }

        [JsonIgnore]
        public required string Senha { get; set; }

        public DateTime DataCriacao { get; set; }
        public ICollection<RendaAtiva> RendasAtiva { get; set; } = new List<RendaAtiva>();
        public ICollection<GastosUsuario> GastosUsuario { get; set; } = new List<GastosUsuario>();
    }
}
