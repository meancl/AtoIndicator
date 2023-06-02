using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_scale_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scaleDatasDict",
                columns: table => new
                {
                    dTime = table.Column<DateTime>(nullable: false),
                    sScaleMethod = table.Column<string>(nullable: false),
                    sVariableName = table.Column<string>(nullable: false),
                    sModelName = table.Column<string>(nullable: false),
                    fD0 = table.Column<double>(nullable: false),
                    fD1 = table.Column<double>(nullable: false),
                    fD2 = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scaleDatasDict", x => new { x.dTime, x.sScaleMethod, x.sVariableName, x.sModelName});
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scaleDatasDict");
        }
    }
}
