using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APIPOUPAFACIL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria_gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeCategoria = table.Column<string>(name: "Nome_Categoria", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria_gastos", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "usuarios",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         nomeusuario = table.Column<string>(type: "text", nullable: false),
            //         email = table.Column<string>(type: "text", nullable: false),
            //         numerotelefone = table.Column<string>(name: "numero_telefone", type: "text", nullable: false),
            //         cpf = table.Column<string>(type: "text", nullable: false),
            //         senha = table.Column<string>(type: "text", nullable: false),
            //         datacriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_usuarios", x => x.id);
            //     });

            migrationBuilder.CreateTable(
                name: "gastos_usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Usuarioid = table.Column<int>(name: "Usuario_id", type: "integer", nullable: false),
                    Categoriaid = table.Column<int>(name: "Categoria_id", type: "integer", nullable: false),
                    ValorGasto = table.Column<double>(name: "Valor_Gasto", type: "double precision", nullable: false),
                    DescricaoGasto = table.Column<string>(name: "Descricao_Gasto", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gastos_usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gastos_usuario_categoria_gastos_Categoria_id",
                        column: x => x.Categoriaid,
                        principalTable: "categoria_gastos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gastos_usuario_usuarios_Usuario_id",
                        column: x => x.Usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "renda_ativa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Usuarioid = table.Column<int>(name: "Usuario_id", type: "integer", nullable: false),
                    RendaDescricao = table.Column<string>(name: "Renda_Descricao", type: "text", nullable: false),
                    ValorRenda = table.Column<double>(name: "Valor_Renda", type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renda_ativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_renda_ativa_usuarios_Usuario_id",
                        column: x => x.Usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gastos_usuario_Categoria_id",
                table: "gastos_usuario",
                column: "Categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_gastos_usuario_Usuario_id",
                table: "gastos_usuario",
                column: "Usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_renda_ativa_Usuario_id",
                table: "renda_ativa",
                column: "Usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gastos_usuario");

            migrationBuilder.DropTable(
                name: "renda_ativa");

            migrationBuilder.DropTable(
                name: "categoria_gastos");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
