using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paper_Route.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTypes1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_DocumentTypeEntity_DocumentTypeId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCaseFile_DocumentCases_DocumentCaseId",
                table: "DocumentCaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCaseFile_Document_DocumentId",
                table: "DocumentCaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCases_DocumentRequestType_RequestTypeId",
                table: "DocumentCases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypeEntity",
                table: "DocumentTypeEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentRequestType",
                table: "DocumentRequestType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCaseFile",
                table: "DocumentCaseFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.RenameTable(
                name: "DocumentTypeEntity",
                newName: "DocumentTypes");

            migrationBuilder.RenameTable(
                name: "DocumentRequestType",
                newName: "DocumentRequestTypes");

            migrationBuilder.RenameTable(
                name: "DocumentCaseFile",
                newName: "DocumentCaseFiles");

            migrationBuilder.RenameTable(
                name: "Document",
                newName: "Documents");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCaseFile_DocumentId",
                table: "DocumentCaseFiles",
                newName: "IX_DocumentCaseFiles_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCaseFile_DocumentCaseId",
                table: "DocumentCaseFiles",
                newName: "IX_DocumentCaseFiles_DocumentCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_DocumentTypeId",
                table: "Documents",
                newName: "IX_Documents_DocumentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentRequestTypes",
                table: "DocumentRequestTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCaseFiles",
                table: "DocumentCaseFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCaseFiles_DocumentCases_DocumentCaseId",
                table: "DocumentCaseFiles",
                column: "DocumentCaseId",
                principalTable: "DocumentCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCaseFiles_Documents_DocumentId",
                table: "DocumentCaseFiles",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCases_DocumentRequestTypes_RequestTypeId",
                table: "DocumentCases",
                column: "RequestTypeId",
                principalTable: "DocumentRequestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCaseFiles_DocumentCases_DocumentCaseId",
                table: "DocumentCaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCaseFiles_Documents_DocumentId",
                table: "DocumentCaseFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCases_DocumentRequestTypes_RequestTypeId",
                table: "DocumentCases");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentRequestTypes",
                table: "DocumentRequestTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCaseFiles",
                table: "DocumentCaseFiles");

            migrationBuilder.RenameTable(
                name: "DocumentTypes",
                newName: "DocumentTypeEntity");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "Document");

            migrationBuilder.RenameTable(
                name: "DocumentRequestTypes",
                newName: "DocumentRequestType");

            migrationBuilder.RenameTable(
                name: "DocumentCaseFiles",
                newName: "DocumentCaseFile");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Document",
                newName: "IX_Document_DocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCaseFiles_DocumentId",
                table: "DocumentCaseFile",
                newName: "IX_DocumentCaseFile_DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCaseFiles_DocumentCaseId",
                table: "DocumentCaseFile",
                newName: "IX_DocumentCaseFile_DocumentCaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypeEntity",
                table: "DocumentTypeEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentRequestType",
                table: "DocumentRequestType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCaseFile",
                table: "DocumentCaseFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_DocumentTypeEntity_DocumentTypeId",
                table: "Document",
                column: "DocumentTypeId",
                principalTable: "DocumentTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCaseFile_DocumentCases_DocumentCaseId",
                table: "DocumentCaseFile",
                column: "DocumentCaseId",
                principalTable: "DocumentCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCaseFile_Document_DocumentId",
                table: "DocumentCaseFile",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCases_DocumentRequestType_RequestTypeId",
                table: "DocumentCases",
                column: "RequestTypeId",
                principalTable: "DocumentRequestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
