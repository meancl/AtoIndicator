using Microsoft.EntityFrameworkCore.Migrations;

namespace MJTradier.Migrations
{
    public partial class mig_add_sell_parted_idx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nPartedIdx",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nPartedIdx",
                table: "sellReports");
        }
    }
}
