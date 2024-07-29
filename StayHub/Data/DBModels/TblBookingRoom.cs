using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblBookingRoom
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public TblBooking TblBooking { get; set; }
        public long RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public TblRoom TblRoom { get; set; }
        public decimal RoomPrice { get; set; }
        public int AdditionalPerson { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
