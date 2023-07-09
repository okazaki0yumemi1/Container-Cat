using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Container_Cat.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Availability = table.Column<int>(type: "INTEGER", nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HostSystemBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NetworkAddressId = table.Column<Guid>(type: "TEXT", nullable: false),
                    InstalledContainerEngine = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostSystemBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostSystemBase_HostAddresses_NetworkAddressId",
                        column: x => x.NetworkAddressId,
                        principalTable: "HostAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseContainers",
                columns: table => new
                {
                    objId = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    HostSystemBaseContainerId = table.Column<Guid>(name: "HostSystem<BaseContainer>Id", type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseContainers", x => x.objId);
                    table.ForeignKey(
                        name: "FK_BaseContainers_HostSystemBase_HostSystem<BaseContainer>Id",
                        column: x => x.HostSystemBaseContainerId,
                        principalTable: "HostSystemBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    RW = table.Column<bool>(type: "INTEGER", nullable: true),
                    BaseContainerobjId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mounts_BaseContainers_BaseContainerobjId",
                        column: x => x.BaseContainerobjId,
                        principalTable: "BaseContainers",
                        principalColumn: "objId");
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    IP = table.Column<string>(type: "TEXT", nullable: true),
                    PrivatePort = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicPort = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    BaseContainerobjId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ports_BaseContainers_BaseContainerobjId",
                        column: x => x.BaseContainerobjId,
                        principalTable: "BaseContainers",
                        principalColumn: "objId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseContainers_HostSystem<BaseContainer>Id",
                table: "BaseContainers",
                column: "HostSystem<BaseContainer>Id");

            migrationBuilder.CreateIndex(
                name: "IX_HostSystemBase_NetworkAddressId",
                table: "HostSystemBase",
                column: "NetworkAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Mounts_BaseContainerobjId",
                table: "Mounts",
                column: "BaseContainerobjId");

            migrationBuilder.CreateIndex(
                name: "IX_Ports_BaseContainerobjId",
                table: "Ports",
                column: "BaseContainerobjId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mounts");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "BaseContainers");

            migrationBuilder.DropTable(
                name: "HostSystemBase");

            migrationBuilder.DropTable(
                name: "HostAddresses");
        }
    }
}
