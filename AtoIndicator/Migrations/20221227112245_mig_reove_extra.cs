using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_reove_extra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sExtraVariables",
                table: "tradeResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sExtraVariables",
                table: "tradeResult",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
