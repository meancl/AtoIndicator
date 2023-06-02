using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_selledHogaCnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fSlotDownPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fSlotUpPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotHogaCount",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotUpDownCount",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fSlotDownPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fSlotUpPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nSlotHogaCount",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nSlotUpDownCount",
                table: "sellReports");
        }
    }
}
