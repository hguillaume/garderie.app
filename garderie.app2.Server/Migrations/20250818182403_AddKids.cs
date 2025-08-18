using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace garderie.app2.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddKids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Daycares",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Kids",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    daycareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kids", x => x.id);
                    table.ForeignKey(
                        name: "FK_Kids_Daycares_daycareId",
                        column: x => x.daycareId,
                        principalTable: "Daycares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kids_daycareId",
                table: "Kids",
                column: "daycareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares");

            migrationBuilder.DropTable(
                name: "Kids");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Daycares",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
