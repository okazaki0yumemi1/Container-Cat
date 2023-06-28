using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Container_Cat.Migrations
{
    /// <inheritdoc />
    public partial class DTOAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mount_BaseContainer_BaseContainerobjId",
                table: "Mount");

            migrationBuilder.DropForeignKey(
                name: "FK_Port_BaseContainer_BaseContainerobjId",
                table: "Port");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemDataObj_HostAddress_NetworkAddressId",
                table: "SystemDataObj");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Port",
                table: "Port");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mount",
                table: "Mount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HostAddress",
                table: "HostAddress");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "HostAddress");

            migrationBuilder.RenameTable(
                name: "Port",
                newName: "Ports");

            migrationBuilder.RenameTable(
                name: "Mount",
                newName: "Mounts");

            migrationBuilder.RenameTable(
                name: "HostAddress",
                newName: "HostAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_Port_BaseContainerobjId",
                table: "Ports",
                newName: "IX_Ports_BaseContainerobjId");

            migrationBuilder.RenameIndex(
                name: "IX_Mount_BaseContainerobjId",
                table: "Mounts",
                newName: "IX_Mounts_BaseContainerobjId");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetworkAddressId",
                table: "SystemDataObj",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "SystemDataObj",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ports",
                table: "Ports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mounts",
                table: "Mounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HostAddresses",
                table: "HostAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mounts_BaseContainer_BaseContainerobjId",
                table: "Mounts",
                column: "BaseContainerobjId",
                principalTable: "BaseContainer",
                principalColumn: "objId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ports_BaseContainer_BaseContainerobjId",
                table: "Ports",
                column: "BaseContainerobjId",
                principalTable: "BaseContainer",
                principalColumn: "objId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemDataObj_HostAddresses_NetworkAddressId",
                table: "SystemDataObj",
                column: "NetworkAddressId",
                principalTable: "HostAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mounts_BaseContainer_BaseContainerobjId",
                table: "Mounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ports_BaseContainer_BaseContainerobjId",
                table: "Ports");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemDataObj_HostAddresses_NetworkAddressId",
                table: "SystemDataObj");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ports",
                table: "Ports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mounts",
                table: "Mounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HostAddresses",
                table: "HostAddresses");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "SystemDataObj");

            migrationBuilder.RenameTable(
                name: "Ports",
                newName: "Port");

            migrationBuilder.RenameTable(
                name: "Mounts",
                newName: "Mount");

            migrationBuilder.RenameTable(
                name: "HostAddresses",
                newName: "HostAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Ports_BaseContainerobjId",
                table: "Port",
                newName: "IX_Port_BaseContainerobjId");

            migrationBuilder.RenameIndex(
                name: "IX_Mounts_BaseContainerobjId",
                table: "Mount",
                newName: "IX_Mount_BaseContainerobjId");

            migrationBuilder.AlterColumn<Guid>(
                name: "NetworkAddressId",
                table: "SystemDataObj",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "HostAddress",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Port",
                table: "Port",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mount",
                table: "Mount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HostAddress",
                table: "HostAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mount_BaseContainer_BaseContainerobjId",
                table: "Mount",
                column: "BaseContainerobjId",
                principalTable: "BaseContainer",
                principalColumn: "objId");

            migrationBuilder.AddForeignKey(
                name: "FK_Port_BaseContainer_BaseContainerobjId",
                table: "Port",
                column: "BaseContainerobjId",
                principalTable: "BaseContainer",
                principalColumn: "objId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemDataObj_HostAddress_NetworkAddressId",
                table: "SystemDataObj",
                column: "NetworkAddressId",
                principalTable: "HostAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
