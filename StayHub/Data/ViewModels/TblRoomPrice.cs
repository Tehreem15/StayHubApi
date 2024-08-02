using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class RoomPriceViewModel
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        public decimal AddPersonPrice { get; set; }
        public long[] Rooms { get; set; }//resources
        public string[] Days { get; set; }
        public string Type { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Startdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public bool IsPriceSetting { get; set; }
        public int MaxAdditionalPerson { get; set; }
        public int NoofNights { get; set; }
        public string RoomName { get; set; }


        public string Detail
        {
            get
            {
                string additionalText = MaxAdditionalPerson > 0 ? $" with {MaxAdditionalPerson} additional person(s)." : ".";
                return ($"{RoomName}" +
                    $"<br/>" +
                    $"Check-in: {Startdate:dd-MMM-yyyy}, Check-out: {EndDate:dd-MMM-yyyy}, Total {NoofNights} night(s){additionalText}");
            }
        }
    }
    public class RoomPriceGeneralModel : RoomPriceViewModel
    {
        public RoomPriceGeneralModel()
        {
            DateWiseReasons = new List<DateWiseReasons>();
        }
       
        public List<DateWiseReasons> DateWiseReasons { get; set; }
        public IEnumerable<DateWiseReasonDetail> ReasonDetails => DateWiseReasons.GroupBy(s => s.Reason).Select(s => new DateWiseReasonDetail
        {
            Note = MessageByType(s.Key),
            Dates = string.Join(", ", s.Select(d => d.Date.ToString("dd-MMM-yyyy")))
        });
        public string ReasonDetailString => string.Join("<br>", ReasonDetails.Select(s => $"{s.Dates}: {s.Note}"));

        private string MessageByType(ReasonType type)
        {
            switch (type)
            {
                case ReasonType.NotAvailable:
                    return "These date(s) are not available for booking.";
                case ReasonType.AlreadyBooked:
                    return "These date(s) are already booked.";
                case ReasonType.NotReady:
                    return "These date(s) are not ready for booking.";
                default:
                    return string.Empty;
            }
        }
    }
    public class DateWiseReasons
    {
        public DateTime Date { get; set; }
        public ReasonType Reason { get; set; }
    }
    public class DateWiseReasonDetail
    {
        public string Dates { get; set; }
        public string Note { get; set; }
    }
    public enum ReasonType
    {
        NotAvailable,
        AlreadyBooked,
        NotReady
    }
}
