using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHub.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfAdultTickets = table.Column<int>(type: "int", nullable: false),
                    AdultTicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfChildTickets = table.Column<int>(type: "int", nullable: false),
                    ChildTicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxTicket = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblGyms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Rules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equiqment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxAdditionalPerson = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSpas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSpas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRoomImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoomImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRoomImages_tblRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "tblRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblRoomPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AddPersonPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoomPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRoomPrices_tblRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "tblRooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tblBookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreditCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxnRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookings_tblUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblStaffActivities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStaffActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblStaffActivities_tblUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfAdultTicket = table.Column<int>(type: "int", nullable: false),
                    AdultTicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfChildTicket = table.Column<int>(type: "int", nullable: false),
                    ChildTicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingEvents_tblBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBookingEvents_tblEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "tblEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingGyms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    GymId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingGyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingGyms_tblBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBookingGyms_tblGyms_GymId",
                        column: x => x.GymId,
                        principalTable: "tblGyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    RoomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalPerson = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalNights = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingRooms_tblBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBookingRooms_tblRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "tblRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingRoomServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingRoomServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingRoomServices_tblBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBookingRoomServices_tblRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "tblRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingSpas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    SpaId = table.Column<long>(type: "bigint", nullable: false),
                    NoOfPersons = table.Column<byte>(type: "tinyint", nullable: false),
                    PerPersonPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpaTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingSpas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingSpas_tblBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBookingSpas_tblSpas_SpaId",
                        column: x => x.SpaId,
                        principalTable: "tblSpas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBookingEventTickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingEventId = table.Column<long>(type: "bigint", nullable: false),
                    TicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ScannedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookingEventTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBookingEventTickets_tblBookingEvents_BookingEventId",
                        column: x => x.BookingEventId,
                        principalTable: "tblBookingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingEvents_BookingId",
                table: "tblBookingEvents",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingEvents_EventId",
                table: "tblBookingEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingEventTickets_BookingEventId",
                table: "tblBookingEventTickets",
                column: "BookingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingGyms_BookingId",
                table: "tblBookingGyms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingGyms_GymId",
                table: "tblBookingGyms",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingRooms_BookingId",
                table: "tblBookingRooms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingRooms_RoomId",
                table: "tblBookingRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingRoomServices_BookingId",
                table: "tblBookingRoomServices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingRoomServices_RoomId",
                table: "tblBookingRoomServices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookings_GuestId",
                table: "tblBookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingSpas_BookingId",
                table: "tblBookingSpas",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBookingSpas_SpaId",
                table: "tblBookingSpas",
                column: "SpaId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoomImages_RoomId",
                table: "tblRoomImages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoomPrices_RoomId",
                table: "tblRoomPrices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tblStaffActivities_StaffId",
                table: "tblStaffActivities",
                column: "StaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBookingEventTickets");

            migrationBuilder.DropTable(
                name: "tblBookingGyms");

            migrationBuilder.DropTable(
                name: "tblBookingRooms");

            migrationBuilder.DropTable(
                name: "tblBookingRoomServices");

            migrationBuilder.DropTable(
                name: "tblBookingSpas");

            migrationBuilder.DropTable(
                name: "tblRoomImages");

            migrationBuilder.DropTable(
                name: "tblRoomPrices");

            migrationBuilder.DropTable(
                name: "tblStaffActivities");

            migrationBuilder.DropTable(
                name: "tblBookingEvents");

            migrationBuilder.DropTable(
                name: "tblGyms");

            migrationBuilder.DropTable(
                name: "tblSpas");

            migrationBuilder.DropTable(
                name: "tblRooms");

            migrationBuilder.DropTable(
                name: "tblBookings");

            migrationBuilder.DropTable(
                name: "tblEvents");

            migrationBuilder.DropTable(
                name: "tblUsers");
        }
    }
}
