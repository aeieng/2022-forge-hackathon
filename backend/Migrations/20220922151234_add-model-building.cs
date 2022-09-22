using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class addmodelbuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Models_BuildingId",
                table: "Models",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Buildings_BuildingId",
                table: "Models",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Buildings_BuildingId",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Models_BuildingId",
                table: "Models");
        }
    }
}
