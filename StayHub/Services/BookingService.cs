using Microsoft.EntityFrameworkCore;
using StayHub.Data.DBModels;
using Stripe;
using System.Linq.Expressions;
using StayHub.Data.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using StayHub.Data.ResponseModel;
using System.Text;

namespace StayHub.Services
{
    public class BookingService 
    {
        private StayHubContext db;
        private readonly IConfiguration _configuration;
        public string AmountFormat = "{0:$#,##0.00; -$#,##0.00;}";
        public string AmountFormatWithoutSign = "{0:#,##0.00; -#,##0.00;}";

        public BookingService(StayHubContext _db, IConfiguration configuration)
        {
            db = _db;
            _configuration = configuration;

        }

        public ResponseListModel<BookingViewModel> GetAllBookings(string SearchPaymentStatus)
        {

            ResponseListModel<BookingViewModel> response = new ResponseListModel<BookingViewModel>();
            List<TblBooking> list = db.tblBookings.Include(t=>t.TblUser).ToList();
            if (!string.IsNullOrEmpty(SearchPaymentStatus)) 
            {
                list = list.Where(s=>s.Status==SearchPaymentStatus).ToList();
            }
            if (list != null && list.Count > 0) {
              response.List= list.Select(b => new BookingViewModel
                {
                    Id = b.Id,                   
                    Title = b.TblUser.Title??"",
                    FirstName = b.TblUser.FirstName,
                    LastName = b.TblUser.LastName,
                    BookingDate = b.BookingDate.ToString("dd-MM-yyyy hh:mm tt"),
                    BookingAmount  = string.Format(AmountFormat, b.BookingAmount),                  
                    PaidAmount = string.Format(AmountFormat, b.PaidAmount),                
                    Email = b.TblUser.Email,                 
                    Phone = b.TblUser.PhoneNumber,                
                    Location = b.TblUser.Address+", "+b.TblUser.City+", "+b.TblUser.Zipcode+", "+b.TblUser.Country,
                    ReferenceNumber = b.ReferenceNumber,
                    Notes=b.Notes,
                    PaidDate= b.PaidDate!=null ? b.PaidDate.Value.ToString("dd-MM-yyyy hh:mm tt"):"",
                    Status = b.Status.Trim(),
                    CreditCard= b.CreditCard,
                    GuestId=b.GuestId,
                    TxnRef=b.TxnRef
                }).ToList();
            }

            response.Success = true;
            return response;
        }

      
      
        public ResponseModel<BookingVM> GetBookingDetails(long bookingId)
        {
            ResponseModel<BookingVM> response = new ResponseModel<BookingVM>();
            var booking = db.tblBookings.Where(b => b.Id == bookingId).FirstOrDefault();
            if (booking != null) {
               var detail = new BookingViewModel
                {
                    Id = booking.Id,
                    Title = booking.TblUser.Title ?? "",
                    FirstName = booking.TblUser.FirstName,
                    LastName = booking.TblUser.LastName,
                    BookingDate = booking.BookingDate.ToString("dd-MM-yyyy hh:mm tt"),
                    BookingAmount = string.Format(AmountFormat, booking.BookingAmount),
                    PaidAmount = string.Format(AmountFormat, booking.PaidAmount),
                    Email = booking.TblUser.Email,
                    Phone = booking.TblUser.PhoneNumber??"",
                    Location = booking.TblUser.Address + ", " + booking.TblUser.City + ", " + booking.TblUser.Zipcode + ", " + booking.TblUser.Country,
                    ReferenceNumber = booking.ReferenceNumber,
                    Notes = booking.Notes,
                    PaidDate = booking.PaidDate != null ? booking.PaidDate.Value.ToString("dd-MM-yyyy hh:mm tt") : "",
                    Status = booking.Status.Trim(),
                    CreditCard = booking.CreditCard,
                    GuestId = booking.GuestId,
                    TxnRef = booking.TxnRef

                };

                List<BookingTypeModel> list = GetBookingItemsDetails(booking.Id).List;
                BookingVM model = new BookingVM()
                {
                    Booking = detail,
                    BookingType = list
                };
                response.Data= model;
                response.Success = true;
                return response;
            }

            return response;
        }

        public ResponseListModel<BookingTypeModel> GetBookingItemsDetails(long bookingId)
        {
            ResponseListModel<BookingTypeModel> response = new ResponseListModel<BookingTypeModel>();
            List<TblBookingEvent> events = db.tblBookingEvents.Include(m => m.TblEvent).Where(b => b.BookingId == bookingId).ToList();
            List<TblBookingGym> gyms = db.tblBookingGyms.Include(g=>g.TblGym).Where(b => b.BookingId == bookingId).ToList();
            List<TblBookingSpa> spas = db.tblBookingSpas.Include(s=>s.TblSpa).Where(b => b.BookingId == bookingId).ToList();
            List<TblBookingRoom> rooms = db.tblBookingRooms.Where(b => b.BookingId == bookingId).ToList();
            List<TblBookingRoomService> roomservices = db.tblBookingRoomServices.Include(r=>r.TblRoom).Where(b => b.BookingId == bookingId).ToList();

            List<BookingTypeModel> list = new List<BookingTypeModel>();
            if (rooms != null && rooms.Count() > 0)
            {
                foreach (TblBookingRoom item in rooms)
                {
                    BookingTypeModel b = new BookingTypeModel
                    {
                        TypeId = item.Id,
                        TypeName = "Room",
                        Description = FillRoomDetails(item),
                        Amount = string.Format(AmountFormat, item.TotalPrice)
                    };
                    list.Add(b);
                }
            }
            if (roomservices != null && roomservices.Count() > 0)
            {
                foreach (TblBookingRoomService item in roomservices)
                {
                    BookingTypeModel b = new BookingTypeModel
                    {
                        TypeId = item.Id,
                        TypeName = "RoomService",
                        Description = FillRoomServiceDetails(item),
                        Amount = string.Format(AmountFormat, item.Price)
                    };
                    list.Add(b);
                }
            }
            if (events!=null && events.Count > 0)
            {
                foreach (TblBookingEvent item in events)
                {
                    BookingTypeModel b = new BookingTypeModel();
                    b.TypeId = item.Id;                  
                    b.TypeName = "Event";
                    b.Amount = string.Format(AmountFormat, item.TotalAmount);
                    b.Description = FillEventDetails(item);
                    list.Add(b);
                }
            }
            if (gyms != null && gyms.Count > 0)
            {
                foreach (TblBookingGym item in gyms)
                {
                    BookingTypeModel b = new BookingTypeModel();
                    b.TypeId = item.Id;
                    b.TypeName = "Gym";
                    b.Amount = string.Format(AmountFormat, item.Price);
                    b.Description = FillGymDetails(item);
                    list.Add(b);
                }
            }
            if (spas != null && spas.Count > 0)
            {
                foreach (TblBookingSpa item in spas)
                {
                    BookingTypeModel b = new BookingTypeModel();
                    b.TypeId = item.Id;
                    b.TypeName = "Spa";
                    b.Amount = string.Format(AmountFormat, item.TotalPrice);
                    b.Description = FillSpaDetails(item);
                    list.Add(b);
                }
            }
          
            response.List=list;
            response.Success = true;
            return response;

        }



        public ResponseModel<TblBooking> GetBookingDetailsById(long Id)
        {
            ResponseModel<TblBooking> response = new ResponseModel<TblBooking>();

           var booking= db.tblBookings.Where(i => i.Id == Id).FirstOrDefault();
            if (booking != null) { 
            
                response.Data = booking;
                response.Success = true;
            }
            return response;
        }

        public int GetBookingReferenceNumber()
        {
          
            int Reference = 0;
            TblBooking booking = db.tblBookings.OrderByDescending(i => i.Id).FirstOrDefault();
            if (booking != null && !string.IsNullOrEmpty(booking.ReferenceNumber))
            {
                string[] subs = booking.ReferenceNumber.Split(' ', '-');
                Reference = Convert.ToInt32(subs[1]);
            }
            return Reference;
        }

        

        public ResponseModel<TblBooking> GetAllBookingDetailsById(long Id)
        {
            ResponseModel<TblBooking> response = new ResponseModel<TblBooking>();
            var booking = db.tblBookings.Where(i => i.Id == Id)
                .Include(i => i.TblBookingRooms).ThenInclude(i=>i.TblRoom)
                .Include(i => i.TblBookingGyms).ThenInclude(i => i.TblGym)            
                .Include(i => i.TblBookingSpas).ThenInclude(i => i.TblSpa)
                .Include(i => i.TblBookingRoomServices).ThenInclude(i => i.TblRoom)
                .Include(i => i.TblBookingEvents).ThenInclude(i => i.TblBookingEventTickets)
                .Include(i => i.TblBookingEvents).ThenInclude(i => i.TblEvent)            
                .AsNoTracking().FirstOrDefault();
            if (booking != null)
            {
               response.Data= booking;
                response.Success = true;
            }
            else
            {
                response.Success=false;

            }
            return response;
        }


        public ResponseModel CancelBooking(long bookingId)
        {
            ResponseModel response = new ResponseModel();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var booking = db.tblBookings
                        .Include(s => s.TblBookingRooms)
                        .Where(s => s.Id == bookingId).FirstOrDefault();
                    if (booking != null)
                    {
                        booking.Status = "Cancelled";
                        if (booking.TblBookingRooms.Any())
                        {
                            foreach (var item in booking.TblBookingRooms)
                            {
                                var rooms = db.tblRoomPrices
                                    .Where(s => s.RoomId == item.RoomId && s.Date >= item.CheckInDate && s.Date < item.CheckOutDate && s.Status == "B").ToList();
                                if(rooms!=null && rooms.Count() > 0)
                                {
                                    foreach(var room in rooms)
                                    {
                                       room.Status = "A";
                                    }
                                }

                            }
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Success = false;
                }
                return response;
            }
        }
     
        public string FillEventDetails(TblBookingEvent model)
        {

            StringBuilder builder = new StringBuilder();


            _ = builder.Append(model.TblEvent.Name + "</br>");
            if (model.NoOfAdultTicket > 0)
            {
                _ = builder.Append("Adult Ticket(s): <span class=''>" + model.NoOfAdultTicket + "</span>");
            }

            if (model.NoOfChildTicket > 0)
            {
                _ = builder.Append((model.NoOfAdultTicket > 0 ? ", " : "") + "Child Admission Ticket(s): <span class=''>" + model.NoOfChildTicket + "</span>");
            }

            return builder.ToString();
        }


        public string FillRoomDetails(TblBookingRoom model)
        {
            string additionalText = model.AdditionalPerson > 0 ? $" with {model.AdditionalPerson} additional person(s)." : ".";
            return ($"{model.TblRoom.Name}" +
                $"<br/>" +
                $"Check-in: {model.CheckInDate:dd-MMM-yyyy}, Check-out: {model.CheckOutDate:dd-MMM-yyyy}, Total {model.TotalNights} night(s){additionalText}");


        }

        public string FillRoomServiceDetails(TblBookingRoomService model)
        {


            StringBuilder builder = new StringBuilder();

            _ = builder.Append(model.TblRoom.Name+", Service Request"+
            model.ServiceName + "<br/> Request Date" +model.RequestDate.ToString("dd-MMM-yyyy"));

            return builder.ToString();


        }

        public string FillGymDetails(TblBookingGym model)
        {
            var sessionMonth = "";
            var gender = model.TblGym.Gender == 1 ? "Male" : "Female";
            switch (model.Month)
            {
                case 1:
                    sessionMonth = "Jan-Mar";
                    break;
                case 2:
                    sessionMonth = "Apr-June";
                    break;
                case 3:
                    sessionMonth = "July-Sept";
                    break;
                case 4:
                    sessionMonth = "Oct-Dec";
                    break;
            }
            StringBuilder builder = new StringBuilder();

            _ = builder.Append(model.TblGym.Name + "For, "+ gender+ ", Month: " +sessionMonth);

            return builder.ToString();


        }

        public string FillSpaDetails(TblBookingSpa model)
        {
            var time = model.TblSpa.OpeningTime;
           
            StringBuilder builder = new StringBuilder();

            _ = builder.Append(model.TblSpa.Name + ",Total Persons: " + model.NoOfPersons+",  Date "+ model.SpaDate.ToString("dd-MMM-yyyy")+", Timing "+time);

            return builder.ToString();


        }


    }
}
