using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class xxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorSchedules_DoctorScheduleId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorScheduleId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DoctorScheduleId",
                table: "Appointments",
                newName: "ScheduleSlotId");

            migrationBuilder.AddColumn<int>(
                name: "doctorScheduleScheduleId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorScheduleId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSlot_DoctorSchedules_DoctorScheduleId",
                        column: x => x.DoctorScheduleId,
                        principalTable: "DoctorSchedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_doctorScheduleScheduleId",
                table: "Appointments",
                column: "doctorScheduleScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ScheduleSlotId",
                table: "Appointments",
                column: "ScheduleSlotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSlot_DoctorScheduleId",
                table: "ScheduleSlot",
                column: "DoctorScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorSchedules_doctorScheduleScheduleId",
                table: "Appointments",
                column: "doctorScheduleScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ScheduleSlot_ScheduleSlotId",
                table: "Appointments",
                column: "ScheduleSlotId",
                principalTable: "ScheduleSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorSchedules_doctorScheduleScheduleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ScheduleSlot_ScheduleSlotId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "ScheduleSlot");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_doctorScheduleScheduleId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ScheduleSlotId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "doctorScheduleScheduleId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ScheduleSlotId",
                table: "Appointments",
                newName: "DoctorScheduleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorScheduleId",
                table: "Appointments",
                column: "DoctorScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorSchedules_DoctorScheduleId",
                table: "Appointments",
                column: "DoctorScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
