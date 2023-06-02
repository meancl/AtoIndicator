using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_final_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fFinalDAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalDSlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHSlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalIAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalISlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMSlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRSlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTAngle",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTSlope",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nFinalPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalDAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalDSlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHSlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalIAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalISlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMSlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRSlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTAngle",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTSlope",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nFinalPrice",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalDAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalDSlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalHSlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalIAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalISlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalMSlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalRSlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTAngle",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fFinalTSlope",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "nFinalPrice",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fFinalDAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalDSlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalHAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalHSlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalIAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalISlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalMAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalMSlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalRAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalRSlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalTAngle",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalTSlope",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFinalPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fFinalDAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalDSlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalHAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalHSlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalIAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalISlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalMAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalMSlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalRAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalRSlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalTAngle",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalTSlope",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFinalPrice",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fFinalDAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalDSlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalHAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalHSlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalIAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalISlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalMAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalMSlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalRAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalRSlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalTAngle",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fFinalTSlope",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nFinalPrice",
                table: "buyReports");
        }
    }
}
