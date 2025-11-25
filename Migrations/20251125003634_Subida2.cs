using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Web_de_Nominas.Migrations
{
    /// <inheritdoc />
    public partial class Subida2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Empleado",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_UsuarioId",
                table: "Empleado",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Usuario_UsuarioId",
                table: "Empleado",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Usuario_UsuarioId",
                table: "Empleado");

            migrationBuilder.DropIndex(
                name: "IX_Empleado_UsuarioId",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Empleado");
        }
    }
}
