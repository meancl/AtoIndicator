using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoTrader.Migrations
{
    public partial class mig_add_strategy_name_dict_group_num : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_strategyNameDict_nStrategyNameIdx",
                table: "strategyNameDict");

            migrationBuilder.AddColumn<int>(
                name: "nStrategyGroupNum",
                table: "strategyNameDict",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_strategyNameDict_nStrategyGroupNum_nStrategyNameIdx",
                table: "strategyNameDict",
                columns: new[] { "nStrategyGroupNum", "nStrategyNameIdx" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_strategyNameDict_nStrategyGroupNum_nStrategyNameIdx",
                table: "strategyNameDict");

            migrationBuilder.DropColumn(
                name: "nStrategyGroupNum",
                table: "strategyNameDict");

            migrationBuilder.CreateIndex(
                name: "IX_strategyNameDict_nStrategyNameIdx",
                table: "strategyNameDict",
                column: "nStrategyNameIdx",
                unique: true);
        }
    }
}
