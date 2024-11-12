using Microsoft.EntityFrameworkCore.Migrations;

namespace GameLibrary.Data.Migrations;

public partial class DropUsersAndCreateSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Drop existing users table
        migrationBuilder.Sql("DROP TABLE IF EXISTS users");

        // Create new tables
        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                Genre = table.Column<string>(type: "TEXT", nullable: false),
                ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                Developer = table.Column<string>(type: "TEXT", nullable: false),
                Publisher = table.Column<string>(type: "TEXT", nullable: false),
                ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                Rating = table.Column<double>(type: "REAL", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                AvatarUrl = table.Column<string>(type: "TEXT", nullable: true),
                IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Reviews",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                GameId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                Rating = table.Column<int>(type: "INTEGER", nullable: false),
                Comment = table.Column<string>(type: "TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
                table.ForeignKey(
                    name: "FK_Reviews_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Reviews_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserFavorites",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                GameId = table.Column<int>(type: "INTEGER", nullable: false),
                AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserFavorites", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserFavorites_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserFavorites_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_GameId",
            table: "Reviews",
            column: "GameId");

        migrationBuilder.CreateIndex(
            name: "IX_Reviews_UserId",
            table: "Reviews",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserFavorites_GameId",
            table: "UserFavorites",
            column: "GameId");

        migrationBuilder.CreateIndex(
            name: "IX_UserFavorites_UserId",
            table: "UserFavorites",
            column: "UserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "UserFavorites");
        migrationBuilder.DropTable(name: "Reviews");
        migrationBuilder.DropTable(name: "Games");
        migrationBuilder.DropTable(name: "Users");

        // Recreate original users table
        migrationBuilder.Sql(@"
            CREATE TABLE users (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                Firstname TEXT NOT NULL,
                Lastname TEXT NOT NULL
            )");
    }
}
