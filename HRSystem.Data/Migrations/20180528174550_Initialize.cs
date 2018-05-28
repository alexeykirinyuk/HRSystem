using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRSystem.Data.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveDirectoryAttributeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveDirectoryAttributeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Login = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Office = table.Column<string>(nullable: true),
                    ManagerLogin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerLogin",
                        column: x => x.ManagerLogin,
                        principalTable: "Employees",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttributeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ActiveDirectoryAttributeInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeInfos_ActiveDirectoryAttributeInfos_ActiveDirectoryAttributeInfoId",
                        column: x => x.ActiveDirectoryAttributeInfoId,
                        principalTable: "ActiveDirectoryAttributeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttributeBases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeLogin = table.Column<string>(nullable: false),
                    AttributeInfoId = table.Column<int>(nullable: false),
                    Descriminator = table.Column<int>(nullable: false),
                    BoolAttributeValue = table.Column<bool>(nullable: true),
                    DateTimeAttributeValue = table.Column<DateTime>(nullable: true),
                    IntAttributeValue = table.Column<int>(nullable: true),
                    StringAttributeValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeBases_AttributeInfos_AttributeInfoId",
                        column: x => x.AttributeInfoId,
                        principalTable: "AttributeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributeBases_Employees_EmployeeLogin",
                        column: x => x.EmployeeLogin,
                        principalTable: "Employees",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeBases_AttributeInfoId",
                table: "AttributeBases",
                column: "AttributeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeBases_EmployeeLogin",
                table: "AttributeBases",
                column: "EmployeeLogin");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeInfos_ActiveDirectoryAttributeInfoId",
                table: "AttributeInfos",
                column: "ActiveDirectoryAttributeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerLogin",
                table: "Employees",
                column: "ManagerLogin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeBases");

            migrationBuilder.DropTable(
                name: "AttributeInfos");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ActiveDirectoryAttributeInfos");
        }
    }
}
