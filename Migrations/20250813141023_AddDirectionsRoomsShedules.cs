using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaksGym.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectionsRoomsShedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    DirectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.DirectionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DirectionId",
                table: "Groups",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "DirectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropIndex(
                name: "IX_Groups_DirectionId",
                table: "Groups");
        }
    }
}
