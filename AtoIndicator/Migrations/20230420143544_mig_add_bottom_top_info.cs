using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_bottom_top_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile60",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile60",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuyWhile30",
                table: "sellReports");
        }
    }
}
