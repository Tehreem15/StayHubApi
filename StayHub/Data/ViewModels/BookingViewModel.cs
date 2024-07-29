using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{

    public class BookingViewModel
    {
        public long Id { get; set; }
        public long GuestId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Location { get; set; }

        public string ReferenceNumber { get; set; }
        public string BookingAmount { get; set; }
        public string BookingDate { get; set; }
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
        public string CreditCard { get; set; }
        public string TxnRef { get; set; }
        public string PaidAmount { get; set; }
        public string PaidDate { get; set; }
       
        public string Notes { get; set; }

    }
    public class BookingVM
    {

        public BookingViewModel Booking { get; set; }

        public List<BookingTypeModel> BookingType { get; set; }

    }
    public class BookingTypeModel
    {
        public long TypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
    }

 }
