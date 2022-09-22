using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class UpdateDataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "ExteriorWallArea",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ExteriorWindowArea",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "BuildingRoomTypes");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "BuildingRoomTypes");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Rooms",
                newName: "ElementId");

            migrationBuilder.RenameColumn(
                name: "FloorArea",
                table: "Rooms",
                newName: "Area");

            migrationBuilder.AddColumn<string>(
                name: "BuildingId",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFloors",
                table: "Buildings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "Buildings",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "FloorArea",
                table: "Buildings",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<double>(
                name: "EquipmentCoolingLoad",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LightingCoolingLoad",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PeopleCoolingLoad",
                table: "Buildings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EquipmentDensity",
                table: "BuildingRoomTypes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LightingDensity",
                table: "BuildingRoomTypes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PeopleDensity",
                table: "BuildingRoomTypes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RoomTypeName",
                table: "BuildingRoomTypes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EquipmentCoolingLoad",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "LightingCoolingLoad",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "PeopleCoolingLoad",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "EquipmentDensity",
                table: "BuildingRoomTypes");

            migrationBuilder.DropColumn(
                name: "LightingDensity",
                table: "BuildingRoomTypes");

            migrationBuilder.DropColumn(
                name: "PeopleDensity",
                table: "BuildingRoomTypes");

            migrationBuilder.DropColumn(
                name: "RoomTypeName",
                table: "BuildingRoomTypes");

            migrationBuilder.RenameColumn(
                name: "ElementId",
                table: "Rooms",
                newName: "ModelId");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "Rooms",
                newName: "FloorArea");

            migrationBuilder.AddColumn<double>(
                name: "ExteriorWallArea",
                table: "Rooms",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExteriorWindowArea",
                table: "Rooms",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFloors",
                table: "Buildings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FloorArea",
                table: "Buildings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "BuildingRoomTypes",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomTypeId",
                table: "BuildingRoomTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentPowerDensity = table.Column<double>(type: "double precision", nullable: false),
                    LightingPowerDensity = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PeopleDensity = table.Column<double>(type: "double precision", nullable: false),
                    PeopleLatentRate = table.Column<double>(type: "double precision", nullable: false),
                    PeopleSensibleRate = table.Column<double>(type: "double precision", nullable: false),
                    VentilationRate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });
        }
    }
}
