namespace StayHub.Data.ViewModels
{
    public class SingleEventTicketModel
    {
            public long Id { get; set; }
            public DateTime BookingDate { get; set; }
            public string RefNo { get; set; }
            public string EventName { get; set; }
            public string FirstName { get; set; }  
            public string LastName { get; set; }
            public string Ticket { get; set; }
            public string EventDateTime { get; set; }
            public string Location { get; set; }
            public string TicketNumber { get; set; }
          
        }

        public class TicketAddress
        {
            public DateTime Date { get; set; }
            public long ItemID { get; set; }
        }

    
}
