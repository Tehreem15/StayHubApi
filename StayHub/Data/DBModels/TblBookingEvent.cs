using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayHub.Data.DBModels
{
    public class TblBookingEvent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        public TblBooking TblBooking { get; set; }

        public long EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public TblEvent TblEvent { get; set; }
        public decimal TotalAmount { get; set; }
        public int NoOfAdultTicket { get; set; } = 0;
        public decimal AdultTicketPrice { get; set; }
        public int NoOfChildTicket { get; set; } = 0;
        public decimal ChildTicketPrice { get; set; }
        public DateTime EventDate { get; set; }
        public  ICollection<TblBookingEventTicket> TblBookingEventTickets { get; set; }= new HashSet<TblBookingEventTicket>();   
    }
}
