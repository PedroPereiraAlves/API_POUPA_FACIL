namespace API_POUPA_FACIL.Classes
{
    public class RendaAtiva
    {
        public int Codigo { get; set;}
        public int UsuarioCodigo { get; set;}   
        public required string RendaDescricao { get; set;}
        public required double ValorRenda {  get; set;}

        public Usuarios Usuario {get; set;}
    }
}
