using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoTrader.Migrations
{
    public partial class mig_extra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sExtraVariables",
                table: "tradeResult",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sExtraVariables",
                table: "tradeResult");
        }
    }
}
