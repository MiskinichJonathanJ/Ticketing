using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ticketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EVENT",
                columns: new[] { "Id", "EventDate", "Name", "Status", "Venue" },
                values: new object[] { 1, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conferencia de Inteligencia Artificial 2026", "Active", "Auditorio Tecnológico - Sala Principal" });

            migrationBuilder.InsertData(
                table: "SECTOR",
                columns: new[] { "Id", "Capacity", "EventId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 50, 1, "Campo General", 95000m },
                    { 2, 50, 1, "Platea ", 120000m }
                });

            migrationBuilder.InsertData(
                table: "SEAT",
                columns: new[] { "Id", "RowIdentifier", "SeatNumber", "SectorId", "Status", "Version" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), "F1", 1, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), "F1", 2, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), "F1", 3, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000004"), "F1", 4, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000005"), "F1", 5, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), "F1", 6, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), "F1", 7, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), "F1", 8, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), "F1", 9, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000010"), "F1", 10, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000011"), "F2", 11, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000012"), "F2", 12, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000013"), "F2", 13, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000014"), "F2", 14, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000015"), "F2", 15, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000016"), "F2", 16, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000017"), "F2", 17, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000018"), "F2", 18, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000019"), "F2", 19, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000020"), "F2", 20, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000021"), "F3", 21, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000022"), "F3", 22, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000023"), "F3", 23, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000024"), "F3", 24, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000025"), "F3", 25, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000026"), "F3", 26, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000027"), "F3", 27, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000028"), "F3", 28, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000029"), "F3", 29, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000030"), "F3", 30, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000031"), "F4", 31, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000032"), "F4", 32, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000033"), "F4", 33, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000034"), "F4", 34, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000035"), "F4", 35, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000036"), "F4", 36, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000037"), "F4", 37, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000038"), "F4", 38, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000039"), "F4", 39, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000040"), "F4", 40, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000041"), "F5", 41, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000042"), "F5", 42, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000043"), "F5", 43, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000044"), "F5", 44, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000045"), "F5", 45, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000046"), "F5", 46, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000047"), "F5", 47, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000048"), "F5", 48, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000049"), "F5", 49, 1, "Available", 0 },
                    { new Guid("a0000001-0000-0000-0000-000000000050"), "F5", 50, 1, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000051"), "F1", 1, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000052"), "F1", 2, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000053"), "F1", 3, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000054"), "F1", 4, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000055"), "F1", 5, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000056"), "F1", 6, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000057"), "F1", 7, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000058"), "F1", 8, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000059"), "F1", 9, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000060"), "F1", 10, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000061"), "F2", 11, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000062"), "F2", 12, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000063"), "F2", 13, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000064"), "F2", 14, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000065"), "F2", 15, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000066"), "F2", 16, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000067"), "F2", 17, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000068"), "F2", 18, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000069"), "F2", 19, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000070"), "F2", 20, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000071"), "F3", 21, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000072"), "F3", 22, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000073"), "F3", 23, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000074"), "F3", 24, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000075"), "F3", 25, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000076"), "F3", 26, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000077"), "F3", 27, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000078"), "F3", 28, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000079"), "F3", 29, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000080"), "F3", 30, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000081"), "F4", 31, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000082"), "F4", 32, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000083"), "F4", 33, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000084"), "F4", 34, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000085"), "F4", 35, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000086"), "F4", 36, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000087"), "F4", 37, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000088"), "F4", 38, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000089"), "F4", 39, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000090"), "F4", 40, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000091"), "F5", 41, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000092"), "F5", 42, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000093"), "F5", 43, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000094"), "F5", 44, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000095"), "F5", 45, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000096"), "F5", 46, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000097"), "F5", 47, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000098"), "F5", 48, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000099"), "F5", 49, 2, "Available", 0 },
                    { new Guid("a0000002-0000-0000-0000-000000000100"), "F5", 50, 2, "Available", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000083"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000087"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000088"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000089"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000090"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000091"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000092"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000093"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000094"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000095"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000096"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000097"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000098"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000099"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000100"));

            migrationBuilder.DeleteData(
                table: "SECTOR",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SECTOR",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EVENT",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
