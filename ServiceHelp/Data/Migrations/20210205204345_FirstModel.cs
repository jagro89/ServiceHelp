using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceHelp.Data.Migrations
{
    public partial class FirstModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.IdCategory);
                });

            migrationBuilder.CreateTable(
                name: "Prioritet",
                columns: table => new
                {
                    IdPrioritet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prioritet", x => x.IdPrioritet);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IdStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IdStatus);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IdIssue = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdPrioritet = table.Column<int>(type: "int", nullable: false),
                    IdServiceUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.IdIssue);
                    table.ForeignKey(
                        name: "FK_Issue_AspNetUsers_IdServiceUser",
                        column: x => x.IdServiceUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Issue_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issue_Prioritet_IdPrioritet",
                        column: x => x.IdPrioritet,
                        principalTable: "Prioritet",
                        principalColumn: "IdPrioritet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Issue_Status_IdStatus",
                        column: x => x.IdStatus,
                        principalTable: "Status",
                        principalColumn: "IdStatus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentIssue",
                columns: table => new
                {
                    IdAttachmentIssue = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdIssue = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentIssue", x => x.IdAttachmentIssue);
                    table.ForeignKey(
                        name: "FK_AttachmentIssue_Issue_IdIssue",
                        column: x => x.IdIssue,
                        principalTable: "Issue",
                        principalColumn: "IdIssue",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueCategory",
                columns: table => new
                {
                    IdIssue = table.Column<int>(type: "int", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.IdCategory, x.IdIssue });
                    table.ForeignKey(
                        name: "FK_IssueCategory_Category_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Category",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssueCategory_Issue_IdIssue",
                        column: x => x.IdIssue,
                        principalTable: "Issue",
                        principalColumn: "IdIssue",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentIssue_IdIssue",
                table: "AttachmentIssue",
                column: "IdIssue");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_IdPrioritet",
                table: "Issue",
                column: "IdPrioritet");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_IdServiceUser",
                table: "Issue",
                column: "IdServiceUser");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_IdStatus",
                table: "Issue",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_IdUser",
                table: "Issue",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_IssueCategory_IdIssue",
                table: "IssueCategory",
                column: "IdIssue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentIssue");

            migrationBuilder.DropTable(
                name: "IssueCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "Prioritet");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
