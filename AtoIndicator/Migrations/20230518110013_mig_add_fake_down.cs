using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_fake_down : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nFakeDownCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownMinuteCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownUpperCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownCnt",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownMinuteCnt",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownUpperCnt",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownMinuteCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeDownUpperCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nFakeDownCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownMinuteCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownUpperCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownMinuteCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownUpperCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownMinuteCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nFakeDownUpperCnt",
                table: "buyReports");
        }
    }
}
