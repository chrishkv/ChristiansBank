using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cuenta_Movimiento.Migrations
{
    public partial class Change_Persona_Cliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "persona_id",
                table: "Cuentas",
                newName: "cliente_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cliente_id",
                table: "Cuentas",
                newName: "persona_id");
        }
    }
}
