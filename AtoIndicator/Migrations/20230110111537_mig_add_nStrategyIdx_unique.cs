using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_nStrategyIdx_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_strategyNameDict_nStrategyNameIdx",
                table: "strategyNameDict",
                column: "nStrategyNameIdx",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_strategyNameDict_nStrategyNameIdx",
                table: "strategyNameDict");
        }
    }
}
