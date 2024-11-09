using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yeshuapp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFrases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Passagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Livro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capitulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Versiculo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frases", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Frases");
        }
    }
}
