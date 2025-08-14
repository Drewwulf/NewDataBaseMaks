using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaksGym.Migrations
{
    /// <inheritdoc />
    public partial class DirectionsRoomsShedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentsToSubscriptions",
                columns: table => new
                {
                    StudentsToSubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsToSubscriptions", x => x.StudentsToSubscriptionId);
                    table.ForeignKey(
                        name: "FK_StudentsToSubscriptions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentsToSubscriptions_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentsToSubscriptions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionFreezeTimes",
                columns: table => new
                {
                    SubscriptionFreezeTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentsToSubscriptionId = table.Column<int>(type: "int", nullable: false),
                    FreezeStart = table.Column<DateOnly>(type: "date", nullable: false),
                    FreezeEnd = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionFreezeTimes", x => x.SubscriptionFreezeTimeId);
                    table.ForeignKey(
                        name: "FK_SubscriptionFreezeTimes_StudentsToSubscriptions_StudentsToSubscriptionId",
                        column: x => x.StudentsToSubscriptionId,
                        principalTable: "StudentsToSubscriptions",
                        principalColumn: "StudentsToSubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsToSubscriptions_StudentId",
                table: "StudentsToSubscriptions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsToSubscriptions_SubscriptionId",
                table: "StudentsToSubscriptions",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsToSubscriptions_TransactionId",
                table: "StudentsToSubscriptions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionFreezeTimes_StudentsToSubscriptionId",
                table: "SubscriptionFreezeTimes",
                column: "StudentsToSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StudentId",
                table: "Transactions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionFreezeTimes");

            migrationBuilder.DropTable(
                name: "StudentsToSubscriptions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
