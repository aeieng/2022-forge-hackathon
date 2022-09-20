using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Models_Buildings_BuildingId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "ExtractionLog");

            migrationBuilder.RenameColumn(
                name: "LastRun",
                table: "ExtractionLog",
                newName: "StartedRunAtUtc");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Models",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "ModelIds",
                table: "ExtractionLog",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "SelectedActivities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedActivities", x => x.ActivityId);
                });

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

            migrationBuilder.DropTable(
                name: "SelectedActivities");

            migrationBuilder.DropColumn(
                name: "ModelIds",
                table: "ExtractionLog");

            migrationBuilder.RenameColumn(
                name: "StartedRunAtUtc",
                table: "ExtractionLog",
                newName: "LastRun");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Models",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "ExtractionLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Models_Buildings_BuildingId",
                table: "Models",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id");
        }
    }
}
