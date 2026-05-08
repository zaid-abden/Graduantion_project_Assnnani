using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addlistofreceptionistindoctortable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receptionists_Doctors_DoctorId",
                table: "Receptionists");

           

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_DoctorId",
                table: "Receptionists",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionists_Doctors_DoctorId",
                table: "Receptionists",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receptionists_Doctors_DoctorId",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_DoctorId",
                table: "Receptionists");

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_DoctorId",
                table: "Receptionists",
                column: "DoctorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionists_Doctors_DoctorId",
                table: "Receptionists",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
