using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnrollSystem.Migrations
{
    public partial class AddNewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceImages_Courses_CourseId",
                table: "AttendanceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_StudentCourses_StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Rooms_RoomId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_RoomId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceImages_CourseId",
                table: "AttendanceImages");

            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MaxSlot",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AttendanceImages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "StudentSectionId",
                table: "Attendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "AttendanceImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    StartDay = table.Column<DateTime>(nullable: false),
                    EndDay = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<int>(nullable: false),
                    EndTime = table.Column<int>(nullable: false),
                    Schedule = table.Column<string>(nullable: false),
                    Slot = table.Column<int>(nullable: false),
                    MaxSlot = table.Column<int>(nullable: false, defaultValue: 30),
                    RoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSectionRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    HasApproval = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSectionRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSectionRegistrations_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSectionRegistrations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name",
                table: "Courses",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentSectionId",
                table: "Attendances",
                column: "StudentSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceImages_SectionId",
                table: "AttendanceImages",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId",
                table: "Sections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_RoomId",
                table: "Sections",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_TeacherId",
                table: "Sections",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSectionRegistrations_SectionId",
                table: "StudentSectionRegistrations",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSectionRegistrations_StudentId",
                table: "StudentSectionRegistrations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSections_SectionId",
                table: "StudentSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSections_StudentId",
                table: "StudentSections",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceImages_Sections_SectionId",
                table: "AttendanceImages",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_StudentSections_StudentSectionId",
                table: "Attendances",
                column: "StudentSectionId",
                principalTable: "StudentSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceImages_Sections_SectionId",
                table: "AttendanceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_StudentSections_StudentSectionId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "StudentSectionRegistrations");

            migrationBuilder.DropTable(
                name: "StudentSections");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Name",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentSectionId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceImages_SectionId",
                table: "AttendanceImages");

            migrationBuilder.DropColumn(
                name: "StudentSectionId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "AttendanceImages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDay",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EndTime",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxSlot",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDay",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StartTime",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentCourseId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AttendanceImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_RoomId",
                table: "Courses",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentCourseId",
                table: "Attendances",
                column: "StudentCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceImages_CourseId",
                table: "AttendanceImages",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceImages_Courses_CourseId",
                table: "AttendanceImages",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_StudentCourses_StudentCourseId",
                table: "Attendances",
                column: "StudentCourseId",
                principalTable: "StudentCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Rooms_RoomId",
                table: "Courses",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
