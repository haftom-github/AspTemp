using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspTemp.Migrations
{
    /// <inheritdoc />
    public partial class TableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthIdentity_AuthProvider_AuthProviderId",
                table: "AuthIdentity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthProvider",
                table: "AuthProvider");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthIdentity",
                table: "AuthIdentity");

            migrationBuilder.RenameTable(
                name: "AuthProvider",
                newName: "authProviders");

            migrationBuilder.RenameTable(
                name: "AuthIdentity",
                newName: "authIdentities");

            migrationBuilder.RenameIndex(
                name: "IX_AuthIdentity_AuthProviderId",
                table: "authIdentities",
                newName: "IX_authIdentities_AuthProviderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_authProviders",
                table: "authProviders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_authIdentities",
                table: "authIdentities",
                columns: new[] { "UserId", "AuthProviderId" });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    RecordStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_authIdentities_Users_UserId",
                table: "authIdentities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_authIdentities_authProviders_AuthProviderId",
                table: "authIdentities",
                column: "AuthProviderId",
                principalTable: "authProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_authIdentities_Users_UserId",
                table: "authIdentities");

            migrationBuilder.DropForeignKey(
                name: "FK_authIdentities_authProviders_AuthProviderId",
                table: "authIdentities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_authProviders",
                table: "authProviders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_authIdentities",
                table: "authIdentities");

            migrationBuilder.RenameTable(
                name: "authProviders",
                newName: "AuthProvider");

            migrationBuilder.RenameTable(
                name: "authIdentities",
                newName: "AuthIdentity");

            migrationBuilder.RenameIndex(
                name: "IX_authIdentities_AuthProviderId",
                table: "AuthIdentity",
                newName: "IX_AuthIdentity_AuthProviderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthProvider",
                table: "AuthProvider",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthIdentity",
                table: "AuthIdentity",
                columns: new[] { "UserId", "AuthProviderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthIdentity_AuthProvider_AuthProviderId",
                table: "AuthIdentity",
                column: "AuthProviderId",
                principalTable: "AuthProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
