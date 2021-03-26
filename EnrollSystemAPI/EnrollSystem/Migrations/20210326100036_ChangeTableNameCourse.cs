using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnrollSystem.Migrations
{
    public partial class ChangeTableNameCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceImages_Classes_ClassId",
                table: "AttendanceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_StudentClasses_StudentClassId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceImages_ClassId",
                table: "AttendanceImages");

            migrationBuilder.DropColumn(
                name: "StudentClassId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "AttendanceImages");

            migrationBuilder.AddColumn<int>(
                name: "StudentCourseId",
                table: "Attendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AttendanceImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
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
                name: "IX_Attendances_StudentCourseId",
                table: "Attendances",
                column: "StudentCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceImages_CourseId",
                table: "AttendanceImages",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_RoomId",
                table: "Courses",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceImages_Courses_CourseId",
                table: "AttendanceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_StudentCourses_StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceImages_CourseId",
                table: "AttendanceImages");

            migrationBuilder.DropColumn(
                name: "StudentCourseId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AttendanceImages");

            migrationBuilder.AddColumn<int>(
                name: "StudentClassId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "AttendanceImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<int>(type: "int", nullable: false),
                    MaxSlot = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slot = table.Column<int>(type: "int", nullable: false),
                    StartDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentClassId",
                table: "Attendances",
                column: "StudentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceImages_ClassId",
                table: "AttendanceImages",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_RoomId",
                table: "Classes",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_ClassId",
                table: "StudentClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId",
                table: "StudentClasses",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceImages_Classes_ClassId",
                table: "AttendanceImages",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_StudentClasses_StudentClassId",
                table: "Attendances",
                column: "StudentClassId",
                principalTable: "StudentClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
