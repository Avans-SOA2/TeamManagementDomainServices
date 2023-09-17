using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.TMEF.Migrations
{
    public partial class TM_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Opponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayingAddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opponents_Address_PlayingAddressId",
                        column: x => x.PlayingAddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamHeadCoachId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Coaches_TeamHeadCoachId",
                        column: x => x.TeamHeadCoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerNumber = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareTakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    HasCar = table.Column<bool>(type: "bit", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareTakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareTakers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHomeGame = table.Column<bool>(type: "bit", nullable: false),
                    CoachId = table.Column<int>(type: "int", nullable: true),
                    LaundryDutyId = table.Column<int>(type: "int", nullable: true),
                    OpponentId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_CareTakers_LaundryDutyId",
                        column: x => x.LaundryDutyId,
                        principalTable: "CareTakers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Opponents_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Opponents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GamePlayer",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayer", x => new { x.GamesId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_GamePlayer_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayer_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "AddressId", "City", "Extension", "Number", "Street" },
                values: new object[,]
                {
                    { 1, "Los Angeles", null, 1111, "S. Figueroa St." },
                    { 2, "Indianapolis", null, 125, "South Pennsylvania Street" }
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Louis van Gaal" },
                    { 2, "Ronald Koeman" }
                });

            migrationBuilder.InsertData(
                table: "Opponents",
                columns: new[] { "Id", "Name", "PlayingAddressId" },
                values: new object[,]
                {
                    { 1, "LA Lakers", 1 },
                    { 2, "Indiana Pacers", 2 }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name", "TeamHeadCoachId" },
                values: new object[,]
                {
                    { 1, "Chicago Bulls senioren", 1 },
                    { 2, "Chicago Bulls junioren", 2 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "EmailAddress", "Name", "PlayerNumber", "TeamId" },
                values: new object[,]
                {
                    { 1, null, "Michael Jordan", 23, 1 },
                    { 2, null, "Scotty Pippen", 33, 1 },
                    { 3, null, "Lebron James", 1, 1 },
                    { 4, null, "Johan Smarius", 2, 1 },
                    { 5, null, "Marcel de Groot", 3, 1 },
                    { 6, null, "Pascal van Gastel", 4, 1 },
                    { 7, null, "Erco Argante", 5, 1 },
                    { 8, null, "Ruud Hermans", 6, 1 },
                    { 9, null, "Arno Broeders", 7, 1 },
                    { 10, null, "Eefje Gijzen", 8, 1 },
                    { 11, null, "Dion Koeze", 9, 1 },
                    { 12, null, "Peter Gerards", 10, 1 },
                    { 13, null, "Ger Oosting", 11, 1 },
                    { 14, null, "Robin Schellius", 12, 2 }
                });

            migrationBuilder.InsertData(
                table: "CareTakers",
                columns: new[] { "Id", "EMailAddress", "GameId", "HasCar", "Name", "PhoneNumber", "PlayerId" },
                values: new object[,]
                {
                    { 1, null, null, true, "F. Jacobse", 0, 1 },
                    { 2, null, null, false, "Ted van Es", 0, 2 },
                    { 3, null, null, true, "Hans van der Vaart", 0, 3 },
                    { 4, null, null, false, "Tjolk Hekking", 0, 4 },
                    { 5, null, null, true, "Remco Clavan", 0, 5 },
                    { 6, null, null, false, "Otto den Beste", 0, 6 },
                    { 7, null, null, true, "Edgar", 0, 7 },
                    { 8, null, null, false, "Jos", 0, 8 },
                    { 9, null, null, true, "Storm", 0, 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareTakers_GameId",
                table: "CareTakers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CareTakers_PlayerId",
                table: "CareTakers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayer_PlayersId",
                table: "GamePlayer",
                column: "PlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CoachId",
                table: "Games",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_LaundryDutyId",
                table: "Games",
                column: "LaundryDutyId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OpponentId",
                table: "Games",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamId",
                table: "Games",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Opponents_PlayingAddressId",
                table: "Opponents",
                column: "PlayingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamHeadCoachId",
                table: "Teams",
                column: "TeamHeadCoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareTakers_Games_GameId",
                table: "CareTakers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareTakers_Games_GameId",
                table: "CareTakers");

            migrationBuilder.DropTable(
                name: "GamePlayer");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "CareTakers");

            migrationBuilder.DropTable(
                name: "Opponents");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
