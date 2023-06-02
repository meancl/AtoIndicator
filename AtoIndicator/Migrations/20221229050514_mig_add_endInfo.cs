using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_endInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fSlotDownEndPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fSlotUpEndPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotChegyulEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotHogaEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotUpDownEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "fSlotDownEndPower",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fSlotUpEndPower",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotChegyulEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotHogaEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotUpDownEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fSlotDownEndPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fSlotUpEndPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nSlotChegyulEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nSlotHogaEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nSlotUpDownEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fSlotDownEndPower",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fSlotUpEndPower",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nSlotChegyulEndCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nSlotHogaEndCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nSlotUpDownEndCnt",
                table: "buyReports");
        }
    }
}
