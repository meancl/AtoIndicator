using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_ai_version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nAIVersion",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nAIVersion",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nAIVersion",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nAIVersion",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nAIVersion",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nAIVersion",
                table: "buyReports");
        }
    }
}
