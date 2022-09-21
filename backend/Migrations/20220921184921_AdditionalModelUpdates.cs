using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class AdditionalModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Buildings_BuildingId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomTypePercentage");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Models_BuildingId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ModelIds",
                table: "ExtractionLog");

            migrationBuilder.DropColumn(
                name: "ElectricityCarbonIntensity",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ElectricityEnergySourcePercentage",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExteriorGlazingAreaOverride",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExteriorWallAreaOverride",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExteriorWallUValue",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExtractedExteriorGlazingArea",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExtractedExteriorWallArea",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ExtractedFloorArea",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "FloorAreaOverride",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "GlazingSolarHeatGainCoefficient",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "GlazingUValue",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "InfiltrationRate",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "NaturalGasCarbonIntensity",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "NaturalGasEnergySourcePercentage",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "OperationalEnergyUseIntensity",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "OtherEnergySourceCarbonIntensity",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "OtherEnergySourcePercentage",
                table: "Buildings",
                newName: "FloorArea");

            migrationBuilder.AddColumn<string>(
                name: "DesignAutomationLog",
                table: "ExtractionLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignAutomationWorkItemId",
                table: "ExtractionLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "ExtractionLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryBuildingType",
                table: "Buildings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Buildings",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Buildings",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AppBundleId",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "BuildingCosts",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArchitecturalCost = table.Column<double>(type: "double precision", nullable: false),
                    StructuralCost = table.Column<double>(type: "double precision", nullable: false),
                    MechanicalCost = table.Column<double>(type: "double precision", nullable: false),
                    PipingCost = table.Column<double>(type: "double precision", nullable: false),
                    ElectricalCost = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingCosts", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "BuildingEmbodiedCarbons",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalEmbodiedCarbonUseIntensity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingEmbodiedCarbons", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "BuildingOperationalCarbons",
                columns: table => new
                {
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationalEnergyUseIntensity = table.Column<double>(type: "double precision", nullable: true),
                    NaturalGasCarbonIntensity = table.Column<double>(type: "double precision", nullable: true),
                    NaturalGasEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: true),
                    ElectricityCarbonIntensity = table.Column<double>(type: "double precision", nullable: true),
                    ElectricityEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: true),
                    OtherEnergySourceCarbonIntensity = table.Column<double>(type: "double precision", nullable: true),
                    OtherEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: true),
                    TotalOperatingCarbonUseIntensity = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingOperationalCarbons", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "BuildingRoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    Area = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingRoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "varchar(32)", nullable: false),
                    SubCategory = table.Column<string>(type: "varchar(32)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    BaselineEpd = table.Column<double>(type: "double precision", nullable: false),
                    AchievableEpd = table.Column<double>(type: "double precision", nullable: false),
                    RealizedEpd = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingCosts");

            migrationBuilder.DropTable(
                name: "BuildingEmbodiedCarbons");

            migrationBuilder.DropTable(
                name: "BuildingOperationalCarbons");

            migrationBuilder.DropTable(
                name: "BuildingRoomTypes");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropColumn(
                name: "DesignAutomationLog",
                table: "ExtractionLog");

            migrationBuilder.DropColumn(
                name: "DesignAutomationWorkItemId",
                table: "ExtractionLog");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "ExtractionLog");

            migrationBuilder.RenameColumn(
                name: "FloorArea",
                table: "Buildings",
                newName: "OtherEnergySourcePercentage");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "ModelIds",
                table: "ExtractionLog",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryBuildingType",
                table: "Buildings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ElectricityCarbonIntensity",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ElectricityEnergySourcePercentage",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExteriorGlazingAreaOverride",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExteriorWallAreaOverride",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExteriorWallUValue",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtractedExteriorGlazingArea",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtractedExteriorWallArea",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtractedFloorArea",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FloorAreaOverride",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GlazingSolarHeatGainCoefficient",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GlazingUValue",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InfiltrationRate",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NaturalGasCarbonIntensity",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NaturalGasEnergySourcePercentage",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperationalEnergyUseIntensity",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OtherEnergySourceCarbonIntensity",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppBundleId",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RoomTypePercentage",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: true),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypePercentage", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_RoomTypePercentage_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypePercentage_BuildingId",
                table: "RoomTypePercentage",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Buildings_BuildingId",
                table: "Models",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Buildings_BuildingId",
                table: "Rooms",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id");
        }
    }
}
