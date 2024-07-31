using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayHub.Data.DBModels
{
    public class TblEvent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
  

        [Required]
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
    
        [DataType(DataType.Date)]
        [Required]
        public DateTime BookingStartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime BookingEndDate { get; set; }

        [DataType(DataType.Time)]
        [Required]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        [Required]
        public TimeSpan EndTime { get; set; }
        public string? Description { get; set; }

        public string? EventImageUrl { get; set; }
        public decimal AdultTicketPrice { get; set; } = 0;
        public decimal ChildTicketPrice { get; set; } = 0;
        public int MaxTicket { get; set; } = 0;
        [Required]
        public string Location { get; set; }
        public  ICollection<TblBookingEvent> TblBookingEvents { get; set; } = new HashSet<TblBookingEvent>();
    }
}
