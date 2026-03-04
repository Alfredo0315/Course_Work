using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_work.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID_Games = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Jenre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Platform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Release_year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID_Games);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID_News = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date_of_publication = table.Column<DateTime>(type: "date", nullable: false),
                    Time_of_publication = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID_News);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    ID_Tournament = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Start_date = table.Column<DateTime>(type: "date", nullable: false),
                    End_date = table.Column<DateTime>(type: "date", nullable: true),
                    Prize_pool = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Location_of_the_event = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.ID_Tournament);
                });

            migrationBuilder.CreateTable(
                name: "Games_News",
                columns: table => new
                {
                    ID_Games_News = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Games = table.Column<int>(type: "int", nullable: false),
                    ID_News = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games_News", x => x.ID_Games_News);
                    table.ForeignKey(
                        name: "FK_Games_News_Games_ID_Games",
                        column: x => x.ID_Games,
                        principalTable: "Games",
                        principalColumn: "ID_Games",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_News_News_ID_News",
                        column: x => x.ID_News,
                        principalTable: "News",
                        principalColumn: "ID_News",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    ID_Matches = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Match_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID_Tournament = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.ID_Matches);
                    table.ForeignKey(
                        name: "FK_Matches_Tournament_ID_Tournament",
                        column: x => x.ID_Tournament,
                        principalTable: "Tournament",
                        principalColumn: "ID_Tournament",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    ID_Teams = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Founded_year = table.Column<int>(type: "int", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Prize_pool = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_Tournament = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID_Teams);
                    table.ForeignKey(
                        name: "FK_Teams_Tournament_ID_Tournament",
                        column: x => x.ID_Tournament,
                        principalTable: "Tournament",
                        principalColumn: "ID_Tournament",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID_Players = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Date_of_birth = table.Column<DateTime>(type: "date", nullable: true),
                    Prize_pool = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_Teams = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID_Players);
                    table.ForeignKey(
                        name: "FK_Players_Teams_ID_Teams",
                        column: x => x.ID_Teams,
                        principalTable: "Teams",
                        principalColumn: "ID_Teams",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Players_Games",
                columns: table => new
                {
                    ID_Players_Games = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Players = table.Column<int>(type: "int", nullable: false),
                    ID_Games = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players_Games", x => x.ID_Players_Games);
                    table.ForeignKey(
                        name: "FK_Players_Games_Games_ID_Games",
                        column: x => x.ID_Games,
                        principalTable: "Games",
                        principalColumn: "ID_Games",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Games_Players_ID_Players",
                        column: x => x.ID_Players,
                        principalTable: "Players",
                        principalColumn: "ID_Players",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_News_ID_Games",
                table: "Games_News",
                column: "ID_Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_News_ID_News",
                table: "Games_News",
                column: "ID_News");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ID_Tournament",
                table: "Matches",
                column: "ID_Tournament");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Country",
                table: "Players",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ID_Teams",
                table: "Players",
                column: "ID_Teams");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Nickname",
                table: "Players",
                column: "Nickname");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Games_ID_Games",
                table: "Players_Games",
                column: "ID_Games");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Games_ID_Players",
                table: "Players_Games",
                column: "ID_Players");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ID_Tournament",
                table: "Teams",
                column: "ID_Tournament");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Start_date",
                table: "Tournament",
                column: "Start_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games_News");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players_Games");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
