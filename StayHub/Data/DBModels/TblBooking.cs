using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblBooking
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long GuestId { get; set; }

        [ForeignKey(nameof(GuestId))]
        public TblUser TblUser { get; set; }
        public string ReferenceNumber { get; set; }
        public string BookingAmount { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
        public string CreditCard { get; set; }
        public string TxnRef { get; set; }
        public string PaidAmount { get; set; } 
        public DateTime? PaidDate { get; set; }
       
        public string Notes { get; set; }
        public  ICollection<TblBookingRoom> TblBookingRooms { get; set; }= new HashSet<TblBookingRoom>();
        public  ICollection<TblBookingEvent> TblBookingEvents { get; set; } = new HashSet<TblBookingEvent>();
        public  ICollection<TblBookingGym> TblBookingGyms { get; set; } = new HashSet<TblBookingGym>();
        public  ICollection<TblBookingRoomService> TblBookingRoomServices { get; set; } = new HashSet<TblBookingRoomService> ();
        public  ICollection<TblBookingSpa> TblBookingSpas { get; set; } = new HashSet<TblBookingSpa>();

    }
}
