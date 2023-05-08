using Microsoft.EntityFrameworkCore.Migrations;

namespace MJTradier.Migrations
{
    public partial class mig_add_locOfComp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_sellReports",
                table: "sellReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_buyReports",
                table: "buyReports");

            migrationBuilder.AddColumn<int>(
                name: "nLocationOfComp",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nLocationOfComp",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_sellReports",
                table: "sellReports",
                columns: new[] { "dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx", "nLocationOfComp" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_buyReports",
                table: "buyReports",
                columns: new[] { "dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx", "nLocationOfComp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_sellReports",
                table: "sellReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_buyReports",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nLocationOfComp",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nLocationOfComp",
                table: "buyReports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sellReports",
                table: "sellReports",
                columns: new[] { "dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_buyReports",
                table: "buyReports",
                columns: new[] { "dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx" });
        }
    }
}
