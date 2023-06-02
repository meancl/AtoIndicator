using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_buyreports_top_bottom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxPowerMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxPowerMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMinPowerMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMinPowerMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fTopPowerMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxPriceMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxPriceMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxTimeMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxTimeMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinPriceMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinPriceMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinTimeMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinTimeMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopPriceMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeAfterBuyWhile60",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuy",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuyWhile10",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTopTimeMinuteAfterBuyWhile30",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fMaxPowerMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fMaxPowerMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fMinPowerMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fMinPowerMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fTopPowerMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMaxPriceMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMaxPriceMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMaxTimeMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMaxTimeMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMinPriceMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMinPriceMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMinTimeMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMinTimeMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopPriceMinuteAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile30",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeAfterBuyWhile60",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuy",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuyWhile10",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nTopTimeMinuteAfterBuyWhile30",
                table: "buyReports");
        }
    }
}
