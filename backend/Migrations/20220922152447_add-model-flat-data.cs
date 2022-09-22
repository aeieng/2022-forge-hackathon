using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class addmodelflatdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementId",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ModelData_DuctSurfaceArea",
                table: "Models",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ModelData_ExteriorWallArea",
                table: "Models",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ModelData_GlazingArea",
                table: "Models",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ModelData_NumberOfCircuits",
                table: "Models",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModelData_NumberOfLightingFixtures",
                table: "Models",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ModelData_TotalPipeLength",
                table: "Models",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElementId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ModelData_DuctSurfaceArea",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelData_ExteriorWallArea",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelData_GlazingArea",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelData_NumberOfCircuits",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelData_NumberOfLightingFixtures",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelData_TotalPipeLength",
                table: "Models");
        }
    }
}
