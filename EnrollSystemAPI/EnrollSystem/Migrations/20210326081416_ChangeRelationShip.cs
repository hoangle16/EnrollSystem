using Microsoft.EntityFrameworkCore.Migrations;

namespace EnrollSystem.Migrations
{
    public partial class ChangeRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "StudentClassId",
                table: "Attendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentClassId",
                table: "Attendances",
                column: "StudentClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_StudentClasses_StudentClassId",
                table: "Attendances",
                column: "StudentClassId",
                principalTable: "StudentClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_StudentClasses_StudentClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentClassId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "StudentClassId",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
