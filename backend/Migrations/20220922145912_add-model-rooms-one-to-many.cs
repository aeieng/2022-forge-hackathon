using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class addmodelroomsonetomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("alter table \"Rooms\" alter column \"ModelId\" type uuid using \"ModelId\"::uuid");
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "ModelId",
            //    table: "Rooms",
            //    type: "uuid",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ModelId",
                table: "Rooms",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Models_ModelId",
                table: "Rooms",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Models_ModelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ModelId",
                table: "Rooms");

            migrationBuilder.Sql("alter table \"Rooms\" alter column \"ModelId\" type text using \"ModelId\"::text");
            //migrationBuilder.AlterColumn<string>(
            //    name: "ModelId",
            //    table: "Rooms",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "uuid");
        }
    }
}
