using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using Stripe;
using Stripe.Climate;
using Stripe.Issuing;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using System.Net.Mail;
using static Azure.Core.HttpHeader;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly CartService cartService;
        private readonly PaymentService paymentService;
        private readonly Services.EventService eventService;
        private readonly GymService gymService;
      
        private readonly BookingService bookingService;
        private readonly RoomService roomService;
        private readonly SpaService spaService;
        private readonly InRoomService inRoomService;
        private readonly Services.AccountService accountService;

        public CartController(CartService _cartService, PaymentService _paymentService, Services.EventService _eventService,
           GymService _gymService,Services.AccountService _accountService,
           BookingService _bookingService, RoomService _roomService, SpaService _spaService, InRoomService _inRoomService)
        {
            bookingService = _bookingService;
            cartService = _cartService;
            paymentService = _paymentService;
            eventService = _eventService;
            gymService = _gymService;
           
            roomService = _roomService;
            spaService = _spaService;   
            gymService=  _gymService;
            inRoomService = _inRoomService;
            accountService = _accountService;
        }

        [HttpPost("Checkout")]
        public IActionResult CheckOut([FromBody] CartModel model)
        {
            var response = new ResponseModel();
            //If all cart items lsit are empty then return error
            if (model.lstRoom == null && model.lstEvent == null && model.lstSpa==null &&
                model.lstGym==null && model.lstRoomService==null)
            {
                response.Success = false;
                response.Message = "Your cart is empty.";
                return Ok(response);
            }

            if (model.lstEvent != null && model.lstEvent.Count > 0)
            {
                foreach (EventModel item in model.lstEvent)
                {
                    ResponseModel result = eventService.ValidateBookingEvent(item);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstRoom != null && model.lstRoom.Count > 0)
            {
                foreach (RoomModel item in model.lstRoom)
                {
                    ResponseModel result = roomService.ValidateRoomPriceAndAvailability(item);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstGym != null && model.lstGym.Count > 0)
            {
                foreach (GymModel item in model.lstGym)
                {
                    ResponseModel result = gymService.ValidateGymCapacity(item.GymId, item.MonthRange, item.Name);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstSpa != null && model.lstSpa.Count > 0)
            {
                foreach (SpaModel item in model.lstSpa)
                {
                    ResponseModel result = spaService.ValidateSpaCapacity(item.SpaId, item.SpaDate, item.NoOfPersons, item.Name);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }


            CalculateItemsPrice(model);


            TblBooking booking = FillBookingData(model);
            model.BookingModel.ReferenceNumber =  GenerateReferenceNumber();
            booking.ReferenceNumber = model.BookingModel.ReferenceNumber;
            var isBookingAdd = cartService.AddBooking(booking);
            if (isBookingAdd)
            {
                    var guest = accountService.FindById(booking.GuestId);
                    var result = ChargePayment(model.BookingModel, model.PaymentDetail,guest.Email);
                    //for credit card if payment is done then we first update db then send emails
                   
                    if (result.Success)
                    {
                      
                        FillBookingForUpdate(booking, model);
                        bool UpdateBooking = cartService.UpdateBooking(booking);
                        if (UpdateBooking)
                        {

                        ///**** here will send confirmation email.
                        //But currently we will not send email
                        ///  response = SendConfirmationEmail(booking.Id, guest);
                        //  var message =  "A confirmation email has been sent to " + guest.Email + ". If it does not appear in your Inbox, please check Junk.<br /> You can also      or print the same information with the buttons that are on this page."
                        response.Success = true;
                        response.Message = "Thank you for your booking.";
                             return Ok(response);
                          
                        }
                    }
                    else
                    {
                    response.Success = false;
                    response.Message = result.Message;
                    return Ok(response);

                    }
                }
                //for bank , full discount case we only send booking confirmation email and passport details to tour passenger 
             
            
            response.Message = "Something went wrong. Please remove items from cart and order again.";
            response.Success = false;
            return Ok(response);
        }

        private void CalculateItemsPrice(CartModel model)
        {
            decimal totalPrice = 0;
            CartCalculateModel result =  CalculateBookingItems(model);
            model = result.CartModel;
            totalPrice = result.TotalPrice;
            model.BookingModel.BookingAmount = totalPrice;
        }
        private string GenerateReferenceNumber()
        {
            string newReferenceNumber = "STH-";
            int ReferenceNumber =  bookingService.GetBookingReferenceNumber();
            string Reference = Convert.ToString(ReferenceNumber + 1);
            newReferenceNumber += Reference.PadLeft(5, '0');
            return newReferenceNumber;
        }
        private CartCalculateModel CalculateBookingItems(CartModel model)
        {

            decimal totalPrice = 0;
         
            if (model.lstEvent != null && model.lstEvent.Count > 0)
            {
                foreach (var item in model.lstEvent)
                {
                    CalculateEventPrice(item);
                    totalPrice = totalPrice + item.ItemTotalPrice;
                }
               
            }

            if (model.lstRoom != null && model.lstRoom.Count > 0)
            {
                foreach (var item in model.lstRoom)
                {
                    CalculateRoomPrice(item);
                    totalPrice = totalPrice + item.ItemTotalPrice;
                }
              
            }
            if (model.lstGym != null && model.lstGym.Count > 0)
            {
                foreach (var item in model.lstGym)
                {
                    CalculateGymPrice(item);
                    totalPrice = totalPrice + item.Price;
                }

            }
            if (model.lstSpa != null && model.lstSpa.Count > 0)
            {
                foreach (var item in model.lstSpa)
                {
                    CalculateSpaPrice(item);
                    totalPrice = totalPrice + item.Price;
                }

            }
            if(model.lstRoomService!=null && model.lstRoomService.Count > 0)
            {
                foreach(var item in model.lstRoomService)
                {
                    CalculateInRoomService(item);
                    totalPrice = totalPrice + item.Price;
                }
            }


            return new CartCalculateModel
            {
                CartModel = model,
                TotalPrice=totalPrice

            };

        }

        [HttpPost("ValidateCartItem")]
        public IActionResult ValidateCartItem([FromBody] CartItemsModel model)
        {
            var response = new ResponseModel();
            if (model.lstEvent != null && model.lstEvent.Count > 0)
            {
                foreach (EventModel item in model.lstEvent)
                {
                    ResponseModel result = eventService.ValidateBookingEvent(item);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstRoom != null && model.lstRoom.Count > 0)
            {
                foreach (RoomModel item in model.lstRoom)
                {
                    ResponseModel result = roomService.ValidateRoomPriceAndAvailability(item);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstGym != null && model.lstGym.Count > 0)
            {
                foreach (GymModel item in model.lstGym)
                {
                    ResponseModel result = gymService.ValidateGymCapacity(item.GymId, item.MonthRange, item.Name);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            if (model.lstSpa != null && model.lstSpa.Count > 0)
            {
                foreach (SpaModel item in model.lstSpa)
                {
                    ResponseModel result = spaService.ValidateSpaCapacity(item.SpaId, item.SpaDate, item.NoOfPersons, item.Name);
                    if (result.Success == false)
                    {
                        return Ok(response);
                    }

                }
            }
            return Ok(response);
        }

        [HttpPost("CalculateCartItems")]
        public IActionResult CalculateItems([FromBody] CartModel model)
        {
            var response= new ResponseModel<CartCalculateModel>();
            response.Data = CalculateBookingItems(model);
            response.Success = true;
            return Ok(response);

        }
        [NonAction]
        private void CalculateEventPrice(EventModel model)
        {
            
            var eventDetail = eventService.GetEventDetail(model.EventId).Data;

            if (model != null)
            {
                decimal price = 0;
                price += model.AdultTickets * eventDetail.AdultTicketPrice;
                price += model.ChildTickets * eventDetail.ChildTicketPrice;
                string AmountFormat = "{0:$#,##0.00; -$#,##0.00;}";
                model.ItemTotalPrice = price;
                model.StrItemTotalPrice = String.Format(AmountFormat, Convert.ToInt32(price));
               
            }

        }
        [NonAction]
        public void CalculateRoomPrice(RoomModel obj)
        {

            RoomViewModel resourceData = roomService.GetRoomById(obj.RoomId).Data;
            List<TblRoomPrice> accomodationAvailablities = roomService.GetAvailablitiesRoomList(s => s.RoomId == obj.RoomId && (s.Date >= obj.CheckInDate && s.Date <= obj.CheckOutDate)).List;
            TimeSpan? datediff = obj.CheckOutDate - obj.CheckInDate;
            if (datediff.HasValue)
                obj.NoofNightStay = Convert.ToByte(datediff.Value.TotalDays);

            decimal Price = 0;

            if (obj.CheckInDate.HasValue && obj.CheckOutDate.HasValue)
            {
                for (DateTime i = obj.CheckInDate.Value.Date; i < obj.CheckOutDate.Value; i = i.AddDays(1))
                {
                    TblRoomPrice availabilityByDate = accomodationAvailablities.FirstOrDefault(s => s.Date.Date == i.Date.Date);
                    if (availabilityByDate != null && availabilityByDate.Status == "A")
                    {
                        Price += availabilityByDate.Price;
                        if (obj.MaxPerson > 0)
                            Price += (obj.MaxPerson * availabilityByDate.AddPersonPrice);
                    }

                }

                obj.ItemTotalPrice = Price;
                obj.NoofNightStay = obj.NoofNightStay;
               
            }

        }
        [NonAction]
        private void CalculateGymPrice(GymModel model)
        {
            var price = gymService.GetGymPrice(model.GymId).Data;
            model.Price = price;
          
        }
        private void CalculateSpaPrice(SpaModel model)
        {
            var price = spaService.GetSpaPrice(model.SpaId).Data;
            model.Price = price;

        }

        private void CalculateInRoomService(RoomServiceModel model)
        {
            var price = inRoomService.GetRoomServiceFee(model.ServiceName);
            model.Price = price;
        }
        private TblBooking FillBookingData(CartModel model)
        { //NZ Time
          
            TblBooking booking = new TblBooking
            {
                BookingAmount = model.BookingModel.BookingAmount,
               
                BookingDate = DateTime.Now,
                GuestId=model.BookingModel.GuestId,

                Id = model.BookingModel.Id,
               
                ReferenceNumber = model.BookingModel.ReferenceNumber,
              
                Status = "UnPaid",
               
            };
            if (model.PaymentDetail.CardNumber != null)
            {
                booking.CreditCard = model.PaymentDetail.CardNumber[^4..];
                booking.TxnRef = model.PaymentDetail.TransactionId;
            }

            booking.PaidDate = DateTime.Now;

            booking.TblBookingEvents = new List<TblBookingEvent>();
            booking.TblBookingRooms = new List<TblBookingRoom>();
            booking.TblBookingGyms = new List<TblBookingGym>();
            booking.TblBookingSpas = new List<TblBookingSpa>();
            booking.TblBookingRoomServices = new List<TblBookingRoomService>();
            if (model.lstEvent != null && model.lstEvent.Count > 0)
            {
                
                foreach ( EventModel item in model.lstEvent)
                {
                    TblBookingEvent bookingEvent = FillEventDetailsFromModel(item);
                    booking.TblBookingEvents.Add(bookingEvent);

                }
            }
            if (model.lstRoom != null && model.lstRoom.Count > 0)
            {

                foreach (RoomModel item in model.lstRoom)
                {
                    TblBookingRoom bookingRoom = FillRoomDetailsFromModel(item);
                    booking.TblBookingRooms.Add(bookingRoom);

                }
            }
            if (model.lstRoomService != null && model.lstRoomService.Count > 0)
            {

                foreach (RoomServiceModel item in model.lstRoomService)
                {
                    TblBookingRoomService bookingRoomService = FillRoomServiceDetailsFromModel(item);
                    booking.TblBookingRoomServices.Add(bookingRoomService);

                }
            }

            if (model.lstSpa != null && model.lstSpa.Count > 0)
            {

                foreach (SpaModel item in model.lstSpa)
                {
                    TblBookingSpa bookingSpa = FillSpaDetailsFromModel(item);
                    booking.TblBookingSpas.Add(bookingSpa);

                }
            }
            if (model.lstGym != null && model.lstGym.Count > 0)
            {

                foreach (GymModel item in model.lstGym)
                {
                    TblBookingGym bookingGym = FillGymDetailsFromModel(item);
                    booking.TblBookingGyms.Add(bookingGym);

                }
            }

            return booking;
        }

        private TblBookingEvent FillEventDetailsFromModel(EventModel model)
        {

            EventViewModel eventDetails = eventService.GetSpecificEvent(model.EventId).Data;
            if (eventDetails != null && model != null)
            {
                TblBookingEvent bookingEvent = new TblBookingEvent
                {
                    EventId = model.EventId,
                    TotalAmount=model.ItemTotalPrice,  
                    EventDate = eventDetails.EventDate,
                    AdultTicketPrice = eventDetails.AdultTicketPrice,                  
                    ChildTicketPrice = eventDetails.ChildTicketPrice,                   
                    NoOfAdultTicket = model.AdultTickets,                
                    NoOfChildTicket = model.ChildTickets,
                };

                bookingEvent.TblBookingEventTickets = FillTicketDetailsFromModel(model, bookingEvent.TblBookingEventTickets);
                return bookingEvent;
            }
            return null;
        }

        private ICollection<TblBookingEventTicket> FillTicketDetailsFromModel(EventModel model, ICollection<TblBookingEventTicket> lstTickets)
        {
           
            string ticketNumber = string.Empty;
            TblBookingEventTicket ticket;

            for (int i = 0; i < model.AdultTickets; i++)
            {
              
                ticket = new TblBookingEventTicket
                {
                    TicketNumber = ticketNumber,
                    Ticket =  "Adult Entry"
                };
                lstTickets.Add(ticket);
            }

            for (int i = 0; i < model.ChildTickets; i++)
            {

                ticket = new TblBookingEventTicket
                {
                    TicketNumber = ticketNumber,
                    Ticket = "Child Entry"
                };
                lstTickets.Add(ticket);
            }
            return lstTickets;

        }

        private TblBookingGym FillGymDetailsFromModel(GymModel model)
        {
            var gym = gymService.GetGymById(model.GymId).Data;
            if (gym != null)
            {
                TblBookingGym bookingGym = new TblBookingGym
                {
                    GymId = gym.Id,
                    Month = model.MonthRange,
                    Price = model.Price,
                    Year = DateTime.Now.Year,

                };
                return bookingGym;
            }
            return null;

        }
        private TblBookingSpa FillSpaDetailsFromModel(SpaModel model)
        {
            var spa = spaService.GetSpaById(model.SpaId).Data;
            if (spa != null)
            {
                TblBookingSpa bookingSpa = new TblBookingSpa
                {
                   NoOfPersons=model.NoOfPersons,
                   SpaDate=model.SpaDate,
                   SpaId=spa.Id,
                   PerPersonPrice=spa.Price,
                   TotalPrice=model.ItemTotalPrice,
           
                };
                return bookingSpa;
            }
            return null;

        }
        private TblBookingRoom FillRoomDetailsFromModel(RoomModel model)
        {
            var room = roomService.GetRoomById(model.RoomId).Data;
            if (room != null)
            {
                TblBookingRoom bookingRoom = new TblBookingRoom
                {
                    AdditionalPerson = model.MaxPerson,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    RoomId = model.RoomId,                  
                    RoomPrice = model.ItemTotalPrice,                 
                    TotalPrice = model.ItemTotalPrice,
                    TotalNights = model.NoofNightStay
                };
                return bookingRoom;
            }
            return null;

        }
        private TblBookingRoomService FillRoomServiceDetailsFromModel(RoomServiceModel model)
        {
            var room = roomService.GetRoomById(model.RoomId).Data;
            if (room != null)
            {
                TblBookingRoomService bookingRoomService = new TblBookingRoomService
                {
                  
                    RoomId = model.RoomId,
                   Description= model.Description,
                   ServiceName=model.ServiceName,
                   Price=model.Price,
                   RequestDate=model.RequestDate,
                   
                };
                return bookingRoomService;
            }
            return null;

        }
        private void FillBookingForUpdate(TblBooking booking, CartModel model)
        {
           
                booking.PaidAmount = model.BookingModel.BookingAmount;
                booking.Status = "Paid";             
                booking.TxnRef = model.PaymentDetail.TransactionId;
                booking.PaidDate = DateTime.Now;
           
        }


        private ResponseModel ChargePayment(BookingModel model,PaymentDetailModel card, string Email)
        {
            var response = new ResponseModel();
            StripeConfiguration.ApiKey = "sk_test_51L16LNE2f2OSnRLXZKGilzd99rgYlVzW042lTrjICjw9YTNDBAzTtr8d0A2OOInNO9tfDzuyxvcBERkMfBz3tqDy00xF7FQLUx";
           
            if (card.CVV != null)
            {
                try
                {
                    PayWithStripeViewModel payWithStripeViewModel = new PayWithStripeViewModel
                    {
                        NameOnCard = card.NameOnCard,
                        CardNumber = card.CardNumber,
                        ExpiryMonth = card.ExpiryMonth,
                        ExpiryYear = card.ExpiryYear,
                        CVV = card.CVV,
                        price = model.BookingAmount,
                        Currency = "USD",
                        Email = Email,
                        ReferenceNumber = model.ReferenceNumber
                    };
                    if (payWithStripeViewModel.price > 0)
                    {
                        Charge charge = paymentService.PayWithStripe(payWithStripeViewModel);
                        card.TransactionId = charge.BalanceTransactionId;
                    
                    }
                    model.Status = "Paid";
                    response.Success = true;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong. Please try again.";
            }
           
            return response;

        }
        [HttpGet("GetBookingReceipt")]
        public IActionResult BookingReceipt([FromQuery] long bookingId)
        {
            var response = new ResponseModel<TblBooking>();
            if (bookingId==0)
            { 
                response.Success = false;
                response.Message = "No booking found";
               
            }
            else
            {
                TblBooking tblBooking =  bookingService.GetAllBookingDetailsById(bookingId);
                if (tblBooking == null)
                {
                    response.Success = false;
                    response.Message = "No booking found";
                    
                }
                else if (tblBooking.PaidAmount == 0)
                {
                    response.Success = false;
                    response.Message = "No booking found";
                }
                else
                {
                    response.Success = true;
                    response.Data = tblBooking;
                }
            }
            return Ok(response);
         
          
        }

        [NonAction]
        public ResponseModel SendConfirmationEmail(long bookingId, TblUser guest)
        {
            var response= new ResponseModel();
           
            TblBooking booking = bookingService.GetAllBookingDetailsById(bookingId);
                
            string Subject = "Booking Confirmation";
            response.Success = true;

            
            return response;


        }

       

}
}
