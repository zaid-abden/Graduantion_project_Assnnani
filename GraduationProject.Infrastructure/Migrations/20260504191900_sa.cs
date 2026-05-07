using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "University",
                table: "StudentDoctors",
                newName: "NationalId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "VerifiedAt",
                table: "StudentDoctors",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "StudentDoctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "StudentDoctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentDoctorId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisingNumber",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDoctors_DoctorId",
                table: "StudentDoctors",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_StudentDoctorId",
                table: "Feedbacks",
                column: "StudentDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SupervisingNumber",
                table: "Doctors",
                column: "SupervisingNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_StudentDoctors_StudentDoctorId",
                table: "Feedbacks",
                column: "StudentDoctorId",
                principalTable: "StudentDoctors",
                principalColumn: "StudentDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDoctors_Doctors_DoctorId",
                table: "StudentDoctors",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_StudentDoctors_StudentDoctorId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDoctors_Doctors_DoctorId",
                table: "StudentDoctors");

            migrationBuilder.DropIndex(
                name: "IX_StudentDoctors_DoctorId",
                table: "StudentDoctors");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_StudentDoctorId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_SupervisingNumber",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "StudentDoctors");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "StudentDoctors");

            migrationBuilder.DropColumn(
                name: "StudentDoctorId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "SupervisingNumber",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "NationalId",
                table: "StudentDoctors",
                newName: "University");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VerifiedAt",
                table: "StudentDoctors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
