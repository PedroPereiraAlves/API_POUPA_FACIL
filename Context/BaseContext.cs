using Microsoft.EntityFrameworkCore;
using API_POUPA_FACIL.Classes;

namespace API_POUPA_FACIL.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
        }
        public DbSet<Usuarios> Usuarios { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseNpgsql(
        //     "Server=localhost;" +
        //     "Port=5432;Database=APP_POUPA_FACIL;" +
        //     "User Id=postgres;" +
        //     "Password=995736;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Usuarios

            modelBuilder.Entity<Usuarios>()
                    .HasKey(s => s.Codigo);

            modelBuilder.Entity<Usuarios>().ToTable("usuarios");

            modelBuilder.Entity<Usuarios>()
                .Property(s => s.Codigo)
                .HasColumnName("id");

            modelBuilder.Entity<Usuarios>()
                .Property(s => s.Nome)
                .HasColumnName("nomeusuario");

            modelBuilder.Entity<Usuarios>()
                .Property(s => s.Senha)
                .HasColumnName("senha");

            modelBuilder.Entity<Usuarios>()
                .Property(s => s.Email)
                .HasColumnName("email");

            modelBuilder.Entity<Usuarios>()
                .Property(s => s.DataCriacao)
                .HasColumnName("datacriacao");

            modelBuilder.Entity<Usuarios>()
                   .Property(s => s.Cpf)
                   .HasColumnName("cpf");

            modelBuilder.Entity<Usuarios>()
                    .Property(s => s.NumeroTelefone)
                    .HasColumnName("numero_telefone");

            #endregion Usuarios

        }
    }
}
