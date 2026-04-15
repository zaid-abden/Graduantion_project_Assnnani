using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class b1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "DoctorSchedules",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "DoctorSchedules",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DoctorSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "DoctorSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAppointments",
                table: "DoctorSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "MaxAppointments",
                table: "DoctorSchedules");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "DoctorSchedules",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "DoctorSchedules",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
