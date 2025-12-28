using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspTemp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthProvider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ClientId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RecordStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthIdentity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuthProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProviderUserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthIdentity", x => new { x.UserId, x.AuthProviderId });
                    table.ForeignKey(
                        name: "FK_AuthIdentity_AuthProvider_AuthProviderId",
                        column: x => x.AuthProviderId,
                        principalTable: "AuthProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuthProvider",
                columns: new[] { "Id", "ClientId", "CreatedBy", "CreatedDate", "Name", "RecordStatus", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("00000000-0000-0000-0001-000000000001"), null, null, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "local", 1, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_AuthIdentity_AuthProviderId",
                table: "AuthIdentity",
                column: "AuthProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthIdentity");

            migrationBuilder.DropTable(
                name: "AuthProvider");
        }
    }
}
