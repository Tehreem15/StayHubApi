using StayHub.Data.DBModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayHub.Data.ViewModels
{
    public class CartModel
    {

        public BookingModel BookingModel { get; set; }
        public PaymentDetailModel PaymentDetail { get; set; }
        public List<EventModel> lstEvent { get; set; }
        public List<RoomModel> lstRoom { get; set; }
        public List<RoomServiceModel> lstRoomService { get; set; }
        public List<GymModel> lstGym { get; set; }
        public List<SpaModel> lstSpa { get; set; }
    }

    public class CartItemsModel
    {
        public List<EventModel> lstEvent { get; set; }
        public List<RoomModel> lstRoom { get; set; }
        public List<RoomServiceModel> lstRoomService { get; set; }
        public List<GymModel> lstGym { get; set; }
        public List<SpaModel> lstSpa { get; set; }
    }
    public class CartCalculateModel
    {
        public CartModel CartModel { get; set; }

        public decimal TotalPrice { get; set; }
    }

    public class BookingModel
    {

        public long Id { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal BookingAmount { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public long GuestId { get; set; }
    }

    public class PaymentDetailModel
    {
        public decimal PaidAmount { get; set; } = 0;
        public long BookingId { get; set; } = 0;
        public string PayType { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(0, long.MaxValue, ErrorMessage = "Invalid number")]
        [StringLength(16, ErrorMessage = "Max 16 digits allowed")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Required")]
        public string NameOnCard { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpiryYear { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpiryMonth { get; set; }
     
        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid number")]
        [StringLength(3, ErrorMessage = "Max 3 digits allowed")]
        public string CVV { get; set; }
        public string TransactionId { get; set; }
       
    }

    public class EventModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public int AdultTickets { get; set; } = 0;
        public int ChildTickets { get; set; } = 0;
        public decimal ItemTotalPrice { get; set; }
        public string StrItemTotalPrice { get; set; }
      
        //event details
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string eventBookingDate { get; set; }
    }
}

    public class RoomModel
    {
        public long Id { get; set; }
        public long RoomId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckInDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckOutDate { get; set; }
        public decimal ItemTotalPrice { get; set; }

        public bool isValid { get; set; }
        public string Details { get; set; }
        public string RoomName { get; set; }
        public int MaxPerson { get; set; }
        public int NoofNightStay { get; set; }
        public int Index { get; set; }


    }

    public class RoomServiceModel 
    {
        public int Index { get; set; }
        public long RoomId { get; set; }
        public string ServiceName { get; set; } //list 
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal Price { get; set; }
}

    public class GymModel
    {
    public int Index { get; set; }
    public long GymId { get; set; }

    public string Name { get; set; }
    public int MonthRange { get; set; }
    public decimal Price { get; set; }
  
}

    public class SpaModel
    {
    public int Index { get; set; }
    public long SpaId { get; set; }
    public decimal ItemTotalPrice { get; set; }
    public int NoOfPersons { get; set; }
    public string Name { get; set; }
    public DateTime SpaDate { get; set; }
    public decimal Price { get; set; }
}

