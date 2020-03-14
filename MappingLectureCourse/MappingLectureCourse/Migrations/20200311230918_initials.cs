using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MappingLectureCourse.Migrations
{
    public partial class initials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    DepartmentID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "designations",
                columns: table => new
                {
                    DesignationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ScalingFactor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_designations", x => x.DesignationID);
                });

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    LevelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_levels", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "qualifications",
                columns: table => new
                {
                    QualificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qualifications", x => x.QualificationID);
                });

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    SemesterID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semesters", x => x.SemesterID);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    SessionID = table.Column<Guid>(nullable: false),
                    SessionName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.SessionID);
                });

            migrationBuilder.CreateTable(
                name: "StaffRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "researchAreas",
                columns: table => new
                {
                    ResearchAreaID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DepartmentID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researchAreas", x => x.ResearchAreaID);
                    table.ForeignKey(
                        name: "FK_researchAreas_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DepartmentID = table.Column<Guid>(nullable: false),
                    RegisterDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffUser_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lectures",
                columns: table => new
                {
                    LectureID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    DesignationID = table.Column<int>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    DepartmentID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lectures", x => x.LectureID);
                    table.ForeignKey(
                        name: "FK_lectures_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lectures_designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listLectureCourses",
                columns: table => new
                {
                    ListLectureCourseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<Guid>(nullable: false),
                    SemesterID = table.Column<int>(nullable: false),
                    DepartmentID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listLectureCourses", x => x.ListLectureCourseID);
                    table.ForeignKey(
                        name: "FK_listLectureCourses_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_listLectureCourses_semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "semesters",
                        principalColumn: "SemesterID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_listLectureCourses_sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "sessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffRoleClaim_StaffRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "StaffRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    CourseID = table.Column<Guid>(nullable: false),
                    CourseCode = table.Column<string>(nullable: false),
                    CourseTitle = table.Column<string>(nullable: false),
                    CourseUnit = table.Column<int>(nullable: false),
                    LevelID = table.Column<int>(nullable: false),
                    ResearchAreaID = table.Column<Guid>(nullable: false),
                    SemesterID = table.Column<int>(nullable: false),
                    DepartmentID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_courses_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courses_levels_LevelID",
                        column: x => x.LevelID,
                        principalTable: "levels",
                        principalColumn: "LevelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courses_researchAreas_ResearchAreaID",
                        column: x => x.ResearchAreaID,
                        principalTable: "researchAreas",
                        principalColumn: "ResearchAreaID");
                    table.ForeignKey(
                        name: "FK_courses_semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "semesters",
                        principalColumn: "SemesterID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffUserClaim_StaffUser_UserId",
                        column: x => x.UserId,
                        principalTable: "StaffUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_StaffUserLogin_StaffUser_UserId",
                        column: x => x.UserId,
                        principalTable: "StaffUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_StaffUserRole_StaffRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "StaffRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffUserRole_StaffUser_UserId",
                        column: x => x.UserId,
                        principalTable: "StaffUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffUserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_StaffUserToken_StaffUser_UserId",
                        column: x => x.UserId,
                        principalTable: "StaffUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lectureQualifications",
                columns: table => new
                {
                    LectureQualificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureID = table.Column<Guid>(nullable: false),
                    QualificationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lectureQualifications", x => x.LectureQualificationID);
                    table.ForeignKey(
                        name: "FK_lectureQualifications_lectures_LectureID",
                        column: x => x.LectureID,
                        principalTable: "lectures",
                        principalColumn: "LectureID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lectureQualifications_qualifications_QualificationID",
                        column: x => x.QualificationID,
                        principalTable: "qualifications",
                        principalColumn: "QualificationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lectureResearchAreas",
                columns: table => new
                {
                    LectureResearchAreaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureID = table.Column<Guid>(nullable: false),
                    ResearchAreaID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lectureResearchAreas", x => x.LectureResearchAreaID);
                    table.ForeignKey(
                        name: "FK_lectureResearchAreas_lectures_LectureID",
                        column: x => x.LectureID,
                        principalTable: "lectures",
                        principalColumn: "LectureID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lectureResearchAreas_researchAreas_ResearchAreaID",
                        column: x => x.ResearchAreaID,
                        principalTable: "researchAreas",
                        principalColumn: "ResearchAreaID");
                });

            migrationBuilder.CreateTable(
                name: "lectureCourses",
                columns: table => new
                {
                    LectureCourseID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureID = table.Column<Guid>(nullable: false),
                    CourseID = table.Column<Guid>(nullable: false),
                    DepartmentID = table.Column<Guid>(nullable: false),
                    SemesterID = table.Column<int>(nullable: false),
                    SessionID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lectureCourses", x => x.LectureCourseID);
                    table.ForeignKey(
                        name: "FK_lectureCourses_courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lectureCourses_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "DepartmentID");
                    table.ForeignKey(
                        name: "FK_lectureCourses_lectures_LectureID",
                        column: x => x.LectureID,
                        principalTable: "lectures",
                        principalColumn: "LectureID");
                    table.ForeignKey(
                        name: "FK_lectureCourses_semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "semesters",
                        principalColumn: "SemesterID");
                    table.ForeignKey(
                        name: "FK_lectureCourses_sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "sessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_DepartmentID",
                table: "courses",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_LevelID",
                table: "courses",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_ResearchAreaID",
                table: "courses",
                column: "ResearchAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_SemesterID",
                table: "courses",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureCourses_CourseID",
                table: "lectureCourses",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureCourses_DepartmentID",
                table: "lectureCourses",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureCourses_LectureID",
                table: "lectureCourses",
                column: "LectureID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureCourses_SemesterID",
                table: "lectureCourses",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureCourses_SessionID",
                table: "lectureCourses",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureQualifications_LectureID",
                table: "lectureQualifications",
                column: "LectureID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureQualifications_QualificationID",
                table: "lectureQualifications",
                column: "QualificationID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureResearchAreas_LectureID",
                table: "lectureResearchAreas",
                column: "LectureID");

            migrationBuilder.CreateIndex(
                name: "IX_lectureResearchAreas_ResearchAreaID",
                table: "lectureResearchAreas",
                column: "ResearchAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_DepartmentID",
                table: "lectures",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_DesignationID",
                table: "lectures",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_listLectureCourses_DepartmentID",
                table: "listLectureCourses",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_listLectureCourses_SemesterID",
                table: "listLectureCourses",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_listLectureCourses_SessionID",
                table: "listLectureCourses",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_researchAreas_DepartmentID",
                table: "researchAreas",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "StaffRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRoleClaim_RoleId",
                table: "StaffRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffUser_DepartmentID",
                table: "StaffUser",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "StaffUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "StaffUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StaffUserClaim_UserId",
                table: "StaffUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffUserLogin_UserId",
                table: "StaffUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffUserRole_RoleId",
                table: "StaffUserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lectureCourses");

            migrationBuilder.DropTable(
                name: "lectureQualifications");

            migrationBuilder.DropTable(
                name: "lectureResearchAreas");

            migrationBuilder.DropTable(
                name: "listLectureCourses");

            migrationBuilder.DropTable(
                name: "StaffRoleClaim");

            migrationBuilder.DropTable(
                name: "StaffUserClaim");

            migrationBuilder.DropTable(
                name: "StaffUserLogin");

            migrationBuilder.DropTable(
                name: "StaffUserRole");

            migrationBuilder.DropTable(
                name: "StaffUserToken");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "qualifications");

            migrationBuilder.DropTable(
                name: "lectures");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "StaffRole");

            migrationBuilder.DropTable(
                name: "StaffUser");

            migrationBuilder.DropTable(
                name: "levels");

            migrationBuilder.DropTable(
                name: "researchAreas");

            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.DropTable(
                name: "designations");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
