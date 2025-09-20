using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace garderie.app2.Server.Migrations
{
    /// <inheritdoc />
    public partial class LinkAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Daycares",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_Daycares_userId",
                table: "Daycares",
                newName: "IX_Daycares_Userid");

            migrationBuilder.AlterColumn<int>(
                name: "Userid",
                table: "Daycares",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                table: "Daycares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Daycares_Users_Userid",
                table: "Daycares",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Daycares_Users_Userid",
                table: "Daycares");

            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                table: "Daycares");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Daycares",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Daycares_Userid",
                table: "Daycares",
                newName: "IX_Daycares_userId");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Daycares",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Daycares_Users_userId",
                table: "Daycares",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
