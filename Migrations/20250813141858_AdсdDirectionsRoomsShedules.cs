using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaksGym.Migrations
{
    /// <inheritdoc />
    public partial class AdсdDirectionsRoomsShedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Shedules_Groups_GroupsId1",
                table: "Shedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shedules_Rooms_RoomId",
                table: "Shedules");

            migrationBuilder.DropIndex(
                name: "IX_Shedules_GroupsId1",
                table: "Shedules");

            migrationBuilder.DropColumn(
                name: "GroupsId1",
                table: "Shedules");

            migrationBuilder.CreateIndex(
                name: "IX_Shedules_GroupsId",
                table: "Shedules",
                column: "GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "DirectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shedules_Groups_GroupsId",
                table: "Shedules",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "GroupsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shedules_Rooms_RoomId",
                table: "Shedules",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Shedules_Groups_GroupsId",
                table: "Shedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Shedules_Rooms_RoomId",
                table: "Shedules");

            migrationBuilder.DropIndex(
                name: "IX_Shedules_GroupsId",
                table: "Shedules");

            migrationBuilder.AddColumn<int>(
                name: "GroupsId1",
                table: "Shedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shedules_GroupsId1",
                table: "Shedules",
                column: "GroupsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Directions_DirectionId",
                table: "Groups",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "DirectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shedules_Groups_GroupsId1",
                table: "Shedules",
                column: "GroupsId1",
                principalTable: "Groups",
                principalColumn: "GroupsId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shedules_Rooms_RoomId",
                table: "Shedules",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
