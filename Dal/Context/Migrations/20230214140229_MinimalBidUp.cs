using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Context.Migrations
{
    public partial class MinimalBidUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimalBid",
                table: "Lots",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimalBid",
                table: "Lots");
        }
    }
}
