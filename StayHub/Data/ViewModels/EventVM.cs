using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class EventViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        [DataType(DataType.Date)] 
        public DateTime? EventDate { get; set; }

        [DataType(DataType.Date)]     
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
      
       [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        public string strStartTime { get; set; }
        public string strEndTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        [Display(Name = "Event Image")]
        public string EventImage { get; set; }
        public decimal AdultTicketPrice { get; set; }
        public int MaxTicket { get; set; }
        public decimal ChildTicketPrice { get; set; }
        public long RemainingTicket { get; set; } = 0;
    }

    public class TicketPrice
    {
        public string Date { get; set; }
        public DateTime FullDate { get; set; }
        public string Admission { get; set; }
    }
    public class EventVM
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [DataType(DataType.Date)] 
        public DateTime BookingStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime BookingEndDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        public decimal? PicnicBasketRefundPrice { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }

        [Display(Name = "Event Image")]
        public string EventImage { get; set; }
        public String editor { get; set; }
        public string oldimage { get; set; }
        public int AdultTickets { get; set; }
        public decimal AdultTicketPrice { get; set; }
        public int ChildTickets { get; set; }
        public decimal ChildTicketPrice { get; set; }
        public int MaxTicket { get; set; } = 0;

    }

    public class EventDates
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

}
