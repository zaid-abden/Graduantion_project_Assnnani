using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dsadassslkdaladdx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecordAttachment_MedicalRecords_MedicalRecordId",
                table: "MedicalRecordAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalRecordAttachment",
                table: "MedicalRecordAttachment");

            migrationBuilder.RenameTable(
                name: "MedicalRecordAttachment",
                newName: "MedicalRecordAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecordAttachment_MedicalRecordId",
                table: "MedicalRecordAttachments",
                newName: "IX_MedicalRecordAttachments_MedicalRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalRecordAttachments",
                table: "MedicalRecordAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecordAttachments_MedicalRecords_MedicalRecordId",
                table: "MedicalRecordAttachments",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecordAttachments_MedicalRecords_MedicalRecordId",
                table: "MedicalRecordAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalRecordAttachments",
                table: "MedicalRecordAttachments");

            migrationBuilder.RenameTable(
                name: "MedicalRecordAttachments",
                newName: "MedicalRecordAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecordAttachments_MedicalRecordId",
                table: "MedicalRecordAttachment",
                newName: "IX_MedicalRecordAttachment_MedicalRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalRecordAttachment",
                table: "MedicalRecordAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecordAttachment_MedicalRecords_MedicalRecordId",
                table: "MedicalRecordAttachment",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
