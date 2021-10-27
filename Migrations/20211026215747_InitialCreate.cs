using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WhitePlayerId = table.Column<string>(type: "TEXT", nullable: true),
                    BlackPlayerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.GameSessionId);
                });

            migrationBuilder.CreateTable(
                name: "ObserverPlayer",
                columns: table => new
                {
                    ObserverPlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    playerId = table.Column<string>(type: "TEXT", nullable: true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObserverPlayer", x => x.ObserverPlayerId);
                    table.ForeignKey(
                        name: "FK_ObserverPlayer_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentGameSessionGameSessionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Users_GameSessions_CurrentGameSessionGameSessionId",
                        column: x => x.CurrentGameSessionGameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "GameSessionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObserverPlayer_GameSessionId",
                table: "ObserverPlayer",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentGameSessionGameSessionId",
                table: "Users",
                column: "CurrentGameSessionGameSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObserverPlayer");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameSessions");
        }
    }
}
