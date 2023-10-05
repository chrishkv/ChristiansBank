using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cuenta_Movimiento.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    cuenta_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numero_cuenta = table.Column<int>(type: "int", nullable: false),
                    tipo_cuenta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    saldo_inicial = table.Column<float>(type: "real", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    persona_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.cuenta_id);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    movimiento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    valor = table.Column<float>(type: "real", nullable: false),
                    saldo = table.Column<float>(type: "real", nullable: false),
                    cuenta_id = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.movimiento_id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Cuentas_cuenta_id",
                        column: x => x.cuenta_id,
                        principalTable: "Cuentas",
                        principalColumn: "cuenta_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_numero_cuenta",
                table: "Cuentas",
                column: "numero_cuenta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_cuenta_id",
                table: "Movimientos",
                column: "cuenta_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Cuentas");
        }
    }
}
