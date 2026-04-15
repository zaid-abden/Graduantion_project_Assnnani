using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MedicalLicenseNumber",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalLicenseNumber",
                table: "Doctors");
        }
    }
}
