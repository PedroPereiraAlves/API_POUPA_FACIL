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
        public DbSet<CategoriasGastos> CategoriasGastos { get; set; }
        public DbSet<GastosUsuario> GastosUsuario { get; set; }
        public DbSet<RendaAtiva> RendaAtiva { get; set; }


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

            #region RendaAtiva
            
                modelBuilder.Entity<RendaAtiva>()
                    .HasKey(r => r.Codigo);

                modelBuilder.Entity<RendaAtiva>().ToTable("renda_ativa");

                modelBuilder.Entity<RendaAtiva>()
                    .Property(r => r.Codigo)
                    .HasColumnName("Id");

                modelBuilder.Entity<RendaAtiva>()
                    .Property(r => r.UsuarioCodigo)
                    .HasColumnName("Usuario_id");

                modelBuilder.Entity<RendaAtiva>()
                    .Property(r => r.RendaDescricao)
                    .HasColumnName("Renda_Descricao");

                modelBuilder.Entity<RendaAtiva>()
                    .Property(r => r.ValorRenda)
                    .HasColumnName("Valor_Renda");

            #endregion RendaAtiva

            #region GastosUsuario

                modelBuilder.Entity<GastosUsuario>()
                    .HasKey(g => g.Codigo);

                modelBuilder.Entity<GastosUsuario>().ToTable("gastos_usuario");

                modelBuilder.Entity<GastosUsuario>()
                    .Property(g => g.Codigo)
                    .HasColumnName("Id");

                modelBuilder.Entity<GastosUsuario>()
                    .Property(g => g.UsuarioCodigo)
                    .HasColumnName("Usuario_id");

                modelBuilder.Entity<GastosUsuario>()
                    .Property(g => g.CategoriaCodigo)
                    .HasColumnName("Categoria_id");

                modelBuilder.Entity<GastosUsuario>()
                    .Property(g => g.ValorGasto)
                    .HasColumnName("Valor_Gasto");

                modelBuilder.Entity<GastosUsuario>()
                    .Property(g => g.DescricaoGasto)
                    .HasColumnName("Descricao_Gasto");

            #endregion GastosUsuario


            #region  CategoriasGastos

                modelBuilder.Entity<CategoriasGastos>()
                    .HasKey(c => c.Codigo);

                modelBuilder.Entity<CategoriasGastos>().ToTable("categoria_gastos");

                modelBuilder.Entity<CategoriasGastos>()
                    .Property(c => c.Codigo)
                    .HasColumnName("Id");

                modelBuilder.Entity<CategoriasGastos>()
                    .Property(c => c.NomeCategoria)
                    .HasColumnName("Nome_Categoria");

            #endregion CategoriasGastos

            #region Relacionamentos

                modelBuilder.Entity<Usuarios>()
                    .HasMany(s => s.RendasAtiva)
                    .WithOne(r => r.Usuario)
                    .HasForeignKey(r => r.UsuarioCodigo);

                modelBuilder.Entity<CategoriasGastos>()
                    .HasMany(c => c.GastosUsuario)
                    .WithOne(g => g.CategoriasGastos)
                    .HasForeignKey(g => g.CategoriaCodigo);
                
                modelBuilder.Entity<Usuarios>()
                    .HasMany(s => s.GastosUsuario)
                    .WithOne(g => g.Usuario)
                    .HasForeignKey(g => g.UsuarioCodigo);

            #endregion Relacionamentos

        }
    }
}
