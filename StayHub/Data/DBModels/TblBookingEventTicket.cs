using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayHub.Data.DBModels
{
    public class TblBookingEventTicket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingEventId { get; set; }
        [Required]

        public string TicketNumber { get; set; }
        [Required]
        public string TicketType { get; set; } //ADULT//CHILD

        [Required]
        public string Ticket { get; set; } //??

        [StringLength(20)]
        [Required]
        public string Status { get; set; }//used
        public string ScannedTime { get; set; }

        [ForeignKey(nameof(BookingEventId))]
        public  TblBookingEvent TblBookingEvent { get; set; }
    }
}
