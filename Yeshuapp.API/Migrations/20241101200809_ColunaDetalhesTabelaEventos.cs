using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yeshuapp.API.Migrations
{
    /// <inheritdoc />
    public partial class ColunaDetalhesTabelaEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Detalhes",
                table: "Eventos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detalhes",
                table: "Eventos");
        }
    }
}
