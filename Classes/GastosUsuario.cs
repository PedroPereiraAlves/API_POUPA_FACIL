namespace API_POUPA_FACIL.Classes
{
    public class GastosUsuario
    {
        public int Codigo {get; set;}
        public int UsuarioCodigo {get; set;}
        public int CategoriaCodigo {get; set;}
        public required double ValorGasto {get; set;}
        public required string DescricaoGasto {get; set;}

        public Usuarios Usuario {get; set;}

        public CategoriasGastos CategoriasGastos {get; set;}
    }
}
