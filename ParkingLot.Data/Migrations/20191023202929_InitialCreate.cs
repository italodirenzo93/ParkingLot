using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingLot.Data.Migrations
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                values: new object[,]
                {
                    { 1, 36000000000L, "1hr", 3.00m },
                    { 2, 108000000000L, "3hr", 4.50m },
                    { 3, 216000000000L, "6hr", 6.75m },
                    #nullable disable
                    { 4, null, "ALL DAY", 10.125m }
                    #nullable restore
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Customer", "IssuedOn", "RateLevelId" },
                values: new object[,]
                {
                    { 2, "Tim Berners-Lee", new DateTimeOffset(new DateTime(2019, 10, 23, 16, 29, 28, 862, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 4, "Gordon Freeman", new DateTimeOffset(new DateTime(2019, 10, 23, 18, 29, 28, 862, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 1, "Italo Di Renzo", new DateTimeOffset(new DateTime(2019, 10, 23, 10, 29, 28, 862, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)), 3 },
                    { 3, "Leon S. Kennedy", new DateTimeOffset(new DateTime(2019, 10, 23, 7, 29, 28, 862, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)), 4 }
                });

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
