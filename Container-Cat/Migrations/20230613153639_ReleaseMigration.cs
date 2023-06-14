using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Container_Cat.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Availability = table.Column<int>(type: "INTEGER", nullable: false),
                    Ip = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemDataObj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NetworkAddressId = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstalledContainerEngine = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDataObj", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemDataObj_HostAddress_NetworkAddressId",
                        column: x => x.NetworkAddressId,
                        principalTable: "HostAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseContainer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    SystemDataObjId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseContainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseContainer_SystemDataObj_SystemDataObjId",
                        column: x => x.SystemDataObjId,
                        principalTable: "SystemDataObj",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    RW = table.Column<bool>(type: "INTEGER", nullable: true),
                    BaseContainerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mount_BaseContainer_BaseContainerId",
                        column: x => x.BaseContainerId,
                        principalTable: "BaseContainer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Port",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    IP = table.Column<string>(type: "TEXT", nullable: true),
                    PrivatePort = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicPort = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    BaseContainerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Port", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Port_BaseContainer_BaseContainerId",
                        column: x => x.BaseContainerId,
                        principalTable: "BaseContainer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseContainer_SystemDataObjId",
                table: "BaseContainer",
                column: "SystemDataObjId");

            migrationBuilder.CreateIndex(
                name: "IX_Mount_BaseContainerId",
                table: "Mount",
                column: "BaseContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Port_BaseContainerId",
                table: "Port",
                column: "BaseContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemDataObj_NetworkAddressId",
                table: "SystemDataObj",
                column: "NetworkAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mount");

            migrationBuilder.DropTable(
                name: "Port");

            migrationBuilder.DropTable(
                name: "BaseContainer");

            migrationBuilder.DropTable(
                name: "SystemDataObj");

            migrationBuilder.DropTable(
                name: "HostAddress");
        }
    }
}
