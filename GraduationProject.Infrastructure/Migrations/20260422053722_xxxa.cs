using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class xxxa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ScheduleSlot_ScheduleSlotId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleSlot_DoctorSchedules_DoctorScheduleId",
                table: "ScheduleSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleSlot",
                table: "ScheduleSlot");

            migrationBuilder.RenameTable(
                name: "ScheduleSlot",
                newName: "ScheduleSlots");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlot_DoctorScheduleId",
                table: "ScheduleSlots",
                newName: "IX_ScheduleSlots_DoctorScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleSlots",
                table: "ScheduleSlots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ScheduleSlots_ScheduleSlotId",
                table: "Appointments",
                column: "ScheduleSlotId",
                principalTable: "ScheduleSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleSlots_DoctorSchedules_DoctorScheduleId",
                table: "ScheduleSlots",
                column: "DoctorScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ScheduleSlots_ScheduleSlotId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleSlots_DoctorSchedules_DoctorScheduleId",
                table: "ScheduleSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleSlots",
                table: "ScheduleSlots");

            migrationBuilder.RenameTable(
                name: "ScheduleSlots",
                newName: "ScheduleSlot");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlots_DoctorScheduleId",
                table: "ScheduleSlot",
                newName: "IX_ScheduleSlot_DoctorScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleSlot",
                table: "ScheduleSlot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ScheduleSlot_ScheduleSlotId",
                table: "Appointments",
                column: "ScheduleSlotId",
                principalTable: "ScheduleSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleSlot_DoctorSchedules_DoctorScheduleId",
                table: "ScheduleSlot",
                column: "DoctorScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
