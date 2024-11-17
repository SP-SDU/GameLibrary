using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformAndPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibrary_AspNetUsers_UserId",
                table: "UserLibrary");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibrary_Games_GameId",
                table: "UserLibrary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLibrary",
                table: "UserLibrary");

            migrationBuilder.DropIndex(
                name: "IX_UserLibrary_UserId",
                table: "UserLibrary");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "UserLibrary",
                newName: "UserLibraries");

            migrationBuilder.RenameIndex(
                name: "IX_UserLibrary_GameId",
                table: "UserLibraries",
                newName: "IX_UserLibraries_GameId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserFavorites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Reviews",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Reviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLibraries",
                table: "UserLibraries",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    AvatarUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_UserId1",
                table: "UserFavorites",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_User_UserId1",
                table: "UserFavorites",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraries_AspNetUsers_UserId",
                table: "UserLibraries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibraries_Games_GameId",
                table: "UserLibraries",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_UserId1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_User_UserId1",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraries_AspNetUsers_UserId",
                table: "UserLibraries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLibraries_Games_GameId",
                table: "UserLibraries");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites");

            migrationBuilder.DropIndex(
                name: "IX_UserFavorites_UserId1",
                table: "UserFavorites");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLibraries",
                table: "UserLibraries");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserFavorites");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "UserLibraries",
                newName: "UserLibrary");

            migrationBuilder.RenameIndex(
                name: "IX_UserLibraries_GameId",
                table: "UserLibrary",
                newName: "IX_UserLibrary_GameId");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLibrary",
                table: "UserLibrary",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibrary_UserId",
                table: "UserLibrary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibrary_AspNetUsers_UserId",
                table: "UserLibrary",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLibrary_Games_GameId",
                table: "UserLibrary",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
