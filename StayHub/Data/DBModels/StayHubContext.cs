using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace StayHub.Data.DBModels
{
   
     public class StayHubContext : DbContext
    {
        public StayHubContext(DbContextOptions<StayHubContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
        public  DbSet<TblUser> tblUsers { get; set; }
        public  DbSet<TblStaffActivity> tblStaffActivities { get; set; }
        public  DbSet<TblRoom> tblRooms { get; set; }
        public  DbSet<TblRoomPrice> tblRoomPrices { get; set; }
        public  DbSet<TblRoomImage> tblRoomImages { get; set; }
        public  DbSet<TblEvent> tblEvents { get; set; }
        public  DbSet<TblGym> tblGyms { get; set; }
        public  DbSet<TblSpa> tblSpas { get; set; }
        public  DbSet<TblBooking> tblBookings { get; set; }
        public  DbSet<TblBookingEvent> tblBookingEvents { get; set; }
        public  DbSet<TblBookingEventTicket> tblBookingEventTickets { get; set; }
        public  DbSet<TblBookingGym> tblBookingGyms { get; set; }  
        public  DbSet<TblBookingSpa> tblBookingSpas { get; set; }
        public  DbSet<TblBookingRoom> tblBookingRooms { get; set; }
        public  DbSet<TblBookingRoomService> tblBookingRoomServices { get; set; }
     
    }
}
