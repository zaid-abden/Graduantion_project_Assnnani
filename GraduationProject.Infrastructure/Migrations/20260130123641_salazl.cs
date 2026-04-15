using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class salazl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorCertificate",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorCertificate",
                table: "Doctors");
        }
    }
}
