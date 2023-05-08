using Microsoft.EntityFrameworkCore.Migrations;

namespace MJTradier.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sdf2",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sdf2",
                table: "sellReports");
        }
    }
}
