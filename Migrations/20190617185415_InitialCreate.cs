using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehiklParkingApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RateLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true),
                    RateValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Customer = table.Column<string>(nullable: true),
                    IssuedOn = table.Column<DateTimeOffset>(nullable: false),
                    RateLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_RateLevels_RateLevelId",
                        column: x => x.RateLevelId,
                        principalTable: "RateLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RateLevels",
                columns: new[] { "Id", "Duration", "Name", "RateValue" },
                values: new object[] { 1, new TimeSpan(0, 1, 0, 0, 0), "1hr", 3.00m });

            migrationBuilder.InsertData(
                table: "RateLevels",
                columns: new[] { "Id", "Duration", "Name", "RateValue" },
                values: new object[] { 2, new TimeSpan(0, 3, 0, 0, 0), "3hr", 4.50m });

            migrationBuilder.InsertData(
                table: "RateLevels",
                columns: new[] { "Id", "Duration", "Name", "RateValue" },
                values: new object[] { 3, new TimeSpan(0, 6, 0, 0, 0), "6hr", 6.75m });

            migrationBuilder.InsertData(
                table: "RateLevels",
                columns: new[] { "Id", "Duration", "Name", "RateValue" },
                values: new object[] { 4, null, "ALL DAY", 10.125m });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Customer", "IssuedOn", "RateLevelId" },
                values: new object[] { 2, "Tim Berners-Lee", new DateTimeOffset(new DateTime(2019, 6, 17, 10, 54, 15, 93, DateTimeKind.Unspecified).AddTicks(7100), new TimeSpan(0, -4, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Customer", "IssuedOn", "RateLevelId" },
                values: new object[] { 4, "Gordon Freeman", new DateTimeOffset(new DateTime(2019, 6, 17, 12, 54, 15, 93, DateTimeKind.Unspecified).AddTicks(7100), new TimeSpan(0, -4, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Customer", "IssuedOn", "RateLevelId" },
                values: new object[] { 1, "Italo Di Renzo", new DateTimeOffset(new DateTime(2019, 6, 17, 4, 54, 15, 93, DateTimeKind.Unspecified).AddTicks(7100), new TimeSpan(0, -4, 0, 0, 0)), 3 });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Customer", "IssuedOn", "RateLevelId" },
                values: new object[] { 3, "Leon S. Kennedy", new DateTimeOffset(new DateTime(2019, 6, 17, 1, 54, 15, 93, DateTimeKind.Unspecified).AddTicks(7100), new TimeSpan(0, -4, 0, 0, 0)), 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RateLevelId",
                table: "Tickets",
                column: "RateLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "RateLevels");
        }
    }
}
