using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehiklParkingApi.Migrations
{
    public partial class ConvertTimeToUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 7, 3, 18, 19, 7, 38, DateTimeKind.Unspecified).AddTicks(9955), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 7, 4, 0, 19, 7, 38, DateTimeKind.Unspecified).AddTicks(9955), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 7, 3, 15, 19, 7, 38, DateTimeKind.Unspecified).AddTicks(9955), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 7, 4, 2, 19, 7, 38, DateTimeKind.Unspecified).AddTicks(9955), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 6, 21, 5, 11, 34, 948, DateTimeKind.Unspecified).AddTicks(1160), new TimeSpan(0, -4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 6, 21, 11, 11, 34, 948, DateTimeKind.Unspecified).AddTicks(1160), new TimeSpan(0, -4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 6, 21, 2, 11, 34, 948, DateTimeKind.Unspecified).AddTicks(1160), new TimeSpan(0, -4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                column: "IssuedOn",
                value: new DateTimeOffset(new DateTime(2019, 6, 21, 13, 11, 34, 948, DateTimeKind.Unspecified).AddTicks(1160), new TimeSpan(0, -4, 0, 0, 0)));
        }
    }
}
