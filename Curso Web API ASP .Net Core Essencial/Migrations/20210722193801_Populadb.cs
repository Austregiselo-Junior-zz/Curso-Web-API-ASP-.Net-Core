using Microsoft.EntityFrameworkCore.Migrations;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Tb_Categorias(Nome, ImagemUrl) Values('Bebidas', " +
                "'http://www.CaminhodaImagem')");

            migrationBuilder.Sql("Insert into Tb_Produtos(Nome, Descricao, Preco, ImagemUrl, Etoque, " +
                "DataCadastro, CategoriaId) Values('Coca-Cola Diet', 'Refrigerante de cola 350 ml'," +
                "5.45, 'http://www.CaminhodaImagem', 50, GETUTCDATE(), " +
                "(Select CategoriaId from Tb_Categorias where Nome = 'Bebidas'))");
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Tb_Categorias");
            migrationBuilder.Sql("Delete from Tb_Produtos");

        }
    }
}
