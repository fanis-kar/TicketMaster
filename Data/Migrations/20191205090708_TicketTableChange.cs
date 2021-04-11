using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace TicketMaster.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class TicketTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Shows_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Tickets");

            migrationBuilder.AddColumn<long>(
                name: "ShowId",
                table: "Tickets",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShowId",
                table: "Tickets",
                column: "ShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Shows_ShowId",
                table: "Tickets",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Shows_ShowId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "Tickets");

            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "Tickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Shows_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
