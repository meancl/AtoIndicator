using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_ai_scoreJar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJar",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJarDegree",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJar",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJarDegree",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJar",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAIScoreJarDegree",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fAIScoreJar",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fAIScoreJarDegree",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fAIScoreJar",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fAIScoreJarDegree",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fAIScoreJar",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fAIScoreJarDegree",
                table: "buyReports");
        }
    }
}
