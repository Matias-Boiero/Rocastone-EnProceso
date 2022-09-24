using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocastone.Data.Migrations
{
    public partial class CorrecionDbsetAplicaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_tiposplicaciones_TipoAplicacionId",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categorias",
                table: "categorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tiposplicaciones",
                table: "tiposplicaciones");

            migrationBuilder.RenameTable(
                name: "categorias",
                newName: "Categorias");

            migrationBuilder.RenameTable(
                name: "tiposplicaciones",
                newName: "TiposAplicaciones");

            migrationBuilder.RenameIndex(
                name: "IX_categorias_NombreCategoria",
                table: "Categorias",
                newName: "IX_Categorias_NombreCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_tiposplicaciones_Nombre",
                table: "TiposAplicaciones",
                newName: "IX_TiposAplicaciones_Nombre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposAplicaciones",
                table: "TiposAplicaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_TiposAplicaciones_TipoAplicacionId",
                table: "Productos",
                column: "TipoAplicacionId",
                principalTable: "TiposAplicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_TiposAplicaciones_TipoAplicacionId",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposAplicaciones",
                table: "TiposAplicaciones");

            migrationBuilder.RenameTable(
                name: "Categorias",
                newName: "categorias");

            migrationBuilder.RenameTable(
                name: "TiposAplicaciones",
                newName: "tiposplicaciones");

            migrationBuilder.RenameIndex(
                name: "IX_Categorias_NombreCategoria",
                table: "categorias",
                newName: "IX_categorias_NombreCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_TiposAplicaciones_Nombre",
                table: "tiposplicaciones",
                newName: "IX_tiposplicaciones_Nombre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categorias",
                table: "categorias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tiposplicaciones",
                table: "tiposplicaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_categorias_CategoriaId",
                table: "Productos",
                column: "CategoriaId",
                principalTable: "categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_tiposplicaciones_TipoAplicacionId",
                table: "Productos",
                column: "TipoAplicacionId",
                principalTable: "tiposplicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
