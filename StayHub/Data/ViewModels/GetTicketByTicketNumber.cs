namespace StayHub.Data.ViewModels
{
    public class GetTicketByTicketNumber
    {
        public long Id { get; set; }
        public long BookingEventId { get; set; }
        public string Name { get; set; }
        public string TicketNumber { get; set; }
        public string TicketType { get; set; }
        public string Ticket { get; set; }
        public DateTime? BookingDate { get; set; }
        public string Status { get; set; }
        public string qrCode { get; set; }
        public DateTime? EventDate { get; set; }
    }

    public class GetFestivalTicketByTicketNumber
    {
        public long Id { get; set; }
        public long BookingEventId { get; set; }
        public string Name { get; set; }
        public string TicketNumber { get; set; }
        public string TicketType { get; set; }
        public string Ticket { get; set; }
        public string Status { get; set; }
        public string ScannedTime { get; set; }
        public string EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
