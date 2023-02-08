using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Context.Migrations
{
    public partial class ClosiTimeForLot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CloseTime",
                table: "Lots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Lots",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Lots");
        }
    }
}