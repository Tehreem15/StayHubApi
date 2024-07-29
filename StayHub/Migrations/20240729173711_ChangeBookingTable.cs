using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHub.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblRoomPrices_tblRooms_RoomId",
                table: "tblRoomPrices");

            migrationBuilder.DropColumn(
                name: "NoOfAdultTickets",
                table: "tblEvents");

            migrationBuilder.DropColumn(
                name: "NoOfChildTickets",
                table: "tblEvents");

            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "tblBookings");

            migrationBuilder.RenameColumn(
                name: "SpaTime",
                table: "tblBookingSpas",
                newName: "SpaDate");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "tblBookings",
                newName: "Status");

            migrationBuilder.AlterColumn<long>(
                name: "RoomId",
                table: "tblRoomPrices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tblRoomPrices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "tblRoomPrices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AddPersonPrice",
                table: "tblRoomPrices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "tblGyms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoOfPersons",
                table: "tblBookingSpas",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "PaidAmount",
                table: "tblBookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "BookingAmount",
                table: "tblBookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "tblBookingGyms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "tblBookingGyms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ScannedTime",
                table: "tblBookingEventTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblRoomPrices_tblRooms_RoomId",
                table: "tblRoomPrices",
                column: "RoomId",
                principalTable: "tblRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblRoomPrices_tblRooms_RoomId",
                table: "tblRoomPrices");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "tblBookingGyms");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "tblBookingGyms");

            migrationBuilder.RenameColumn(
                name: "SpaDate",
                table: "tblBookingSpas",
                newName: "SpaTime");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tblBookings",
                newName: "PaymentStatus");

            migrationBuilder.AlterColumn<long>(
                name: "RoomId",
                table: "tblRoomPrices",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tblRoomPrices",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "tblRoomPrices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "AddPersonPrice",
                table: "tblRoomPrices",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "tblGyms",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "NoOfAdultTickets",
                table: "tblEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOfChildTickets",
                table: "tblEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte>(
                name: "NoOfPersons",
                table: "tblBookingSpas",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "tblBookings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BookingAmount",
                table: "tblBookings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BookingStatus",
                table: "tblBookings",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScannedTime",
                table: "tblBookingEventTickets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_tblRoomPrices_tblRooms_RoomId",
                table: "tblRoomPrices",
                column: "RoomId",
                principalTable: "tblRooms",
                principalColumn: "Id");
        }
    }
}
