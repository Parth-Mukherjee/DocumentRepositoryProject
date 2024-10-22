using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentRepository.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "documentModels",
                columns: table => new
                {
                    documentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    documentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    documentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadedFileDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    documentSize = table.Column<int>(type: "int", nullable: true),
                    documentExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploaded_DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentModels", x => x.documentID);
                });

            migrationBuilder.CreateTable(
                name: "errorLogDetails",
                columns: table => new
                {
                    logID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    errorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorLogOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    errorStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorRoute = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_errorLogDetails", x => x.logID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documentModels");

            migrationBuilder.DropTable(
                name: "errorLogDetails");
        }
    }
}
