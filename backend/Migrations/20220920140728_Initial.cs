using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ModelType = table.Column<int>(type: "integer", nullable: false),
                    AppBundleId = table.Column<string>(type: "text", nullable: false),
                    ActivityType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PrimaryBuildingType = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    ExtractedFloorArea = table.Column<double>(type: "double precision", nullable: false),
                    FloorAreaOverride = table.Column<double>(type: "double precision", nullable: true),
                    ExtractedExteriorWallArea = table.Column<double>(type: "double precision", nullable: false),
                    ExteriorWallAreaOverride = table.Column<double>(type: "double precision", nullable: true),
                    ExtractedExteriorGlazingArea = table.Column<double>(type: "double precision", nullable: false),
                    ExteriorGlazingAreaOverride = table.Column<double>(type: "double precision", nullable: true),
                    NumberOfFloors = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    ExteriorWallUValue = table.Column<double>(type: "double precision", nullable: false),
                    GlazingUValue = table.Column<double>(type: "double precision", nullable: false),
                    GlazingSolarHeatGainCoefficient = table.Column<double>(type: "double precision", nullable: false),
                    InfiltrationRate = table.Column<double>(type: "double precision", nullable: false),
                    OperationalEnergyUseIntensity = table.Column<double>(type: "double precision", nullable: false),
                    NaturalGasCarbonIntensity = table.Column<double>(type: "double precision", nullable: false),
                    NaturalGasEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: false),
                    ElectricityCarbonIntensity = table.Column<double>(type: "double precision", nullable: false),
                    ElectricityEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: false),
                    OtherEnergySourceCarbonIntensity = table.Column<double>(type: "double precision", nullable: false),
                    OtherEnergySourcePercentage = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtractionLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastRun = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    VentilationRate = table.Column<double>(type: "double precision", nullable: false),
                    PeopleDensity = table.Column<double>(type: "double precision", nullable: false),
                    PeopleSensibleRate = table.Column<double>(type: "double precision", nullable: false),
                    PeopleLatentRate = table.Column<double>(type: "double precision", nullable: false),
                    LightingPowerDensity = table.Column<double>(type: "double precision", nullable: false),
                    EquipmentPowerDensity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AutodeskItemId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RevitVersion = table.Column<string>(type: "text", nullable: false),
                    AutodeskHubId = table.Column<string>(type: "text", nullable: false),
                    AutodeskProjectId = table.Column<string>(type: "text", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ModelId = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FloorArea = table.Column<double>(type: "double precision", nullable: false),
                    ExteriorWallArea = table.Column<double>(type: "double precision", nullable: false),
                    ExteriorWindowArea = table.Column<double>(type: "double precision", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomTypePercentage",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Percentage = table.Column<double>(type: "double precision", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "IX_Models_BuildingId",
                table: "Models",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypePercentage_BuildingId",
                table: "RoomTypePercentage",
                column: "BuildingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ExtractionLog");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomTypePercentage");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
