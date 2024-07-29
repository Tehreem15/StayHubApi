using Microsoft.AspNetCore.Mvc;

namespace StayHub.Data.ViewModels
{
    public class RoomAvailabilityViewModel
    {
            public long Id { get; set; }
            public long RoomId { get; set; }
            public DateTime Date { get; set; }
            public decimal Price { get; set; }
            public string Status { get; set; }
            public decimal AddPersonPrice { get; set; }
            public string Days { get; set; }
            public string RoomName { get; set; }

        }

        public class RoomAvailabilityPriceModel
        {
            public long availabilityId { get; set; }
            public decimal? price { get; set; }
            public string status { get; set; }
            public decimal? addPersonPrice { get; set; }


        }
    public class UpdatePriceModel
    {
        public long id { get; set; }
        public decimal price { get; set; }
        public decimal addPersonPrice { get; set; }
        public string bookingStatus { get; set; }
    }
}
