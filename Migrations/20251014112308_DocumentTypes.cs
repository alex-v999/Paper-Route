using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paper_Route.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "DocumentCases");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DocumentCases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestTypeId",
                table: "DocumentCases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocumentRequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRequestType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequiredForVerification = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_DocumentTypeEntity_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCaseFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCaseId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCaseFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentCaseFile_DocumentCases_DocumentCaseId",
                        column: x => x.DocumentCaseId,
                        principalTable: "DocumentCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentCaseFile_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCases_RequestTypeId",
                table: "DocumentCases",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_DocumentTypeId",
                table: "Document",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCaseFile_DocumentCaseId",
                table: "DocumentCaseFile",
                column: "DocumentCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCaseFile_DocumentId",
                table: "DocumentCaseFile",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCases_DocumentRequestType_RequestTypeId",
                table: "DocumentCases",
                column: "RequestTypeId",
                principalTable: "DocumentRequestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCases_DocumentRequestType_RequestTypeId",
                table: "DocumentCases");

            migrationBuilder.DropTable(
                name: "DocumentCaseFile");

            migrationBuilder.DropTable(
                name: "DocumentRequestType");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "DocumentTypeEntity");

            migrationBuilder.DropIndex(
                name: "IX_DocumentCases_RequestTypeId",
                table: "DocumentCases");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DocumentCases");

            migrationBuilder.DropColumn(
                name: "RequestTypeId",
                table: "DocumentCases");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DocumentCases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
