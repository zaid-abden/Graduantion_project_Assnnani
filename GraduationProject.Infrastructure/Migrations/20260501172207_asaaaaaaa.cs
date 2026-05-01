using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClinicLocation",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicLocation",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Doctors");
        }
    }
}
