using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblBookingRoomService
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public TblBooking TblBooking { get; set; }
        public long RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public TblRoom TblRoom { get; set; }
        public string ServiceName { get; set; } //list 
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
      //  public string RequestStatus { get; set; } //Completed //Pending//
        public decimal Price { get; set; }
      
    }
}
