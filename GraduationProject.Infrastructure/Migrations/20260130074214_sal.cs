using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specialization_SpecializationId",
                table: "Doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialization",
                table: "Specialization");

            migrationBuilder.RenameTable(
                name: "Specialization",
                newName: "Specializations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specializations_SpecializationId",
                table: "Doctors",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specializations_SpecializationId",
                table: "Doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations");

            migrationBuilder.RenameTable(
                name: "Specializations",
                newName: "Specialization");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialization",
                table: "Specialization",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specialization_SpecializationId",
                table: "Doctors",
                column: "SpecializationId",
                principalTable: "Specialization",
                principalColumn: "Id");
        }
    }
}
