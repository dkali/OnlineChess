using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChess.Migrations
{
    public partial class Cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GameSessions_CurrentGameSessionGameSessionId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ObserverPlayer");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentGameSessionGameSessionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentGameSessionGameSessionId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentGameSessionGameSessionId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlackPlayerId = table.Column<string>(type: "TEXT", nullable: true),
                    WhitePlayerId = table.Column<string>(type: "TEXT", nullable: true)
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
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: true),
                    playerId = table.Column<string>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentGameSessionGameSessionId",
                table: "Users",
                column: "CurrentGameSessionGameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ObserverPlayer_GameSessionId",
                table: "ObserverPlayer",
                column: "GameSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GameSessions_CurrentGameSessionGameSessionId",
                table: "Users",
                column: "CurrentGameSessionGameSessionId",
                principalTable: "GameSessions",
                principalColumn: "GameSessionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
