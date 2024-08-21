namespace API_POUPA_FACIL.Classes
{
    public class CategoriasGastos
    {
        public int Codigo {get; set;}
        public required string NomeCategoria {get; set;}
        public ICollection<GastosUsuario> GastosUsuario { get; set; }
    }
}