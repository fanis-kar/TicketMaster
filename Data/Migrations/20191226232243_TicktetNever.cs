using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketMaster.Data.Migrations
{
    public partial class TicktetNever : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Tickets",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Tickets",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
