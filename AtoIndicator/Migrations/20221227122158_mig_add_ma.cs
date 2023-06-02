using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_ma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fMa1hVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMa20mVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMa2hVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaDownFsVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxMa1hVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxMa20mVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxMa2hVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxMaDownFsVal",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nFb",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFs",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "nMaxMa1hTime",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "nMaxMa20mTime",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "nMaxMa2hTime",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "nMaxMaDownFsTime",
                table: "tradeResult",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fMa1hVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMa20mVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMa2hVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMaDownFsVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMaxMa1hVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMaxMa20mVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMaxMa2hVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "fMaxMaDownFsVal",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nFb",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nFs",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nMaxMa1hTime",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nMaxMa20mTime",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nMaxMa2hTime",
                table: "tradeResult");

            migrationBuilder.DropColumn(
                name: "nMaxMaDownFsTime",
                table: "tradeResult");
        }
    }
}
