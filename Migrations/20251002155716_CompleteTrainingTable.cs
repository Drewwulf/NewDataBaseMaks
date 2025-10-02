using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaksGym.Migrations
{
    /// <inheritdoc />
    public partial class CompleteTrainingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Trainings",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_CoachId",
                table: "Trainings",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_directionId",
                table: "Trainings",
                column: "directionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_RoomId",
                table: "Trainings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_StudentId",
                table: "Trainings",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_subscriptionId",
                table: "Trainings",
                column: "subscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Coaches_CoachId",
                table: "Trainings",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "CoachId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Directions_directionId",
                table: "Trainings",
                column: "directionId",
                principalTable: "Directions",
                principalColumn: "DirectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Rooms_RoomId",
                table: "Trainings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Students_StudentId",
                table: "Trainings",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Subscriptions_subscriptionId",
                table: "Trainings",
                column: "subscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Coaches_CoachId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Directions_directionId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Rooms_RoomId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Students_StudentId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Subscriptions_subscriptionId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_CoachId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_directionId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_RoomId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_StudentId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_subscriptionId",
                table: "Trainings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Trainings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
