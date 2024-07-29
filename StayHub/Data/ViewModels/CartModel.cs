using System.ComponentModel.DataAnnotations;

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

    public class BookingModel
    {

        public long Id { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? BookingAmount { get; set; }
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
        public string Status { get; set; }
    }

    public class EventModel
    {

    }

    public class RoomModel
    {
        public long Id { get; set; }
        public long RoomId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckInDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckOutDate { get; set; }
        public decimal? ItemTotalPrice { get; set; }

        public bool isValid { get; set; }
        public string Details { get; set; }
        public string RoomName { get; set; }
        public byte? MaxPerson { get; set; }
        public byte? NoofNightStay { get; set; }
        public int Index { get; set; }


    }

    public class RoomServiceModel 
    {
        
    }

    public class GymModel
    {

    }

    public class SpaModel
    {

    }
}
