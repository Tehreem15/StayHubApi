using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using static Azure.Core.HttpHeader;
using System;
using System.Text;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase { 
   
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly EventService eventService;
        private readonly BookingService bookingService;
        public EventController(EventService _eventService, IWebHostEnvironment _webHostEnvironment,
            BookingService _bookingService)
        {
            eventService = _eventService;
            webHostEnvironment = _webHostEnvironment;
            bookingService = _bookingService;   
          
        }
        //ADMIN SIDE
        #region AdminSide
        [Authorize("ADMIN")]
        [HttpGet("GetEvents")]
        public IActionResult GetEvents()
        {
            var response = eventService.GetAllEvent();
            return Ok(response);
        }
        
        [HttpGet("GetEventById")]

        public IActionResult GetEventById(long id)
        {
           
            var response = eventService.GetEventModel(id);           
            return Ok(response);
 
        }


        [HttpPost("AddEditEvent")]
        [Authorize]
        public IActionResult AddEdit([FromForm] EventVM model)
        {
           
            model.EventImage = (model.Id > 0) ? model.EventImage : "";
            if (model.EventFile != null)
            {
                model.EventImage = eventService.UploadEventImages(model.EventFile);
            }

            var response = eventService.AddEditEvent(model);
            return Ok(response);

        }
        //delete event
        [Authorize]
        [HttpGet("DeleteEvent")]
        public IActionResult Delete([FromQuery] long Id)
        {
            var response = eventService.Delete(Id);
            return Ok(response);
        }
        #endregion
        #region GuestSide
       
        [HttpGet("GetEventsForBooking")]
        public IActionResult GetEventsForBooking()
        {
            var response = eventService.GetStayHubEvents();
            return Ok(response);
        }
        [HttpGet("GetEventDetail")]
        public IActionResult GetEventForBooking([FromQuery] long id)
        {
            var response = eventService.GetSpecificEvent(id);
            return Ok(response);
        }

        [HttpPost("CalculateEventPrice")]
        public IActionResult CalculatePrice([FromBody] EventModel model)
        {
            var response= new ResponseModel<EventModel>();  
            var eventDetail = eventService.GetEventDetail(model.eventId).Data;

            if (model != null)
            {
                decimal? price = 0;
                price += model.adultTickets == null ? 0 : (model.adultTickets * eventDetail.AdultTicketPrice);
                price += model.childTickets == null ? 0 : (model.childTickets * eventDetail.ChildTicketPrice);
                string AmountFormat = "{0:$#,##0.00; -$#,##0.00;}";
                model.itemTotalPrice = price;
                model.strItemTotalPrice = String.Format(AmountFormat, Convert.ToInt32(price));
                response.Data = model;
                response.Success = true;
            }
         
            response.Success = false;
            return Ok(response);

        }

        [HttpPost("ValidateBookingEvent")]
        public IActionResult ValidateBookingEvent([FromBody] EventModel model)
        {
            var response = new ResponseModel();
           
            if (model == null)
            {
                response.Success = false;
                return Ok(response);
            }
            bool IsNoTickets =  model.adultTickets == 0 && model.childTickets == 0;
            if (IsNoTickets == true)
            {
                response.Success = false;
                response.Message = "No tickets are selected.";
                return Ok(response);
            }
            var result = eventService.CheckBookingEventTickets(model.eventId);
            if (result != null)
            {
                int RemainingTickets = result.Data;
                int? totalEntryTicket = model.adultTickets + model.childTickets;
               
                if (IsNoTickets == true)
                {
                    if (RemainingTickets <= -1 || RemainingTickets == 0)
                    {
                        response.Success = false;
                        response.Message= "Currently no ticket(s) are available.";
                    }
                    else if (RemainingTickets > 0 && totalEntryTicket > RemainingTickets)
                    {
                        response.Success = false;
                        response.Message = "Currently "+ RemainingTickets + " ticket(s) are available.";
                    }
                    else
                    {
                        response.Success = true;
                    }
                }
            }
            return Ok(response);
        }

        [HttpGet("GetTicketList")]
        public async Task<IActionResult> GetTicketList([FromQuery] int bookingId)
        {

            var response = new ResponseListModel<SingleEventTicketModel>();
            TblBooking booking = bookingService.GetAllBookingDetailsById(bookingId);
           
            List<SingleEventTicketModel> lstTicket = FillSingleTicketModelFromDB(booking);
            lstTicket = lstTicket.OrderBy(i => i.Id).ToList();
            if (lstTicket != null && lstTicket.Count > 0)
            {
                response.List=lstTicket;
                response.Success = true;

            }
            else
            {
                response.Success = false;
            }
            return Ok(response);

        }

        public List<SingleEventTicketModel> FillSingleTicketModelFromDB(TblBooking booking)
        {
            List<SingleEventTicketModel> lstModel = new List<SingleEventTicketModel>();
            SingleEventTicketModel model = new SingleEventTicketModel();

            if (booking == null)
            {
                return lstModel;
            }

            string firstName = booking.TblUser.FirstName;
            string lastName = booking.TblUser.LastName;

            DateTime bookingDate;
            foreach (TblBookingEvent item in booking.TblBookingEvents)
            {
                bookingDate = booking.BookingDate;
                foreach (TblBookingEventTicket innerItem in item.TblBookingEventTickets)
                {
                    model = new SingleEventTicketModel
                    {
                        EventName = item.TblEvent.Name,
                        FirstName = firstName,
                        LastName = lastName,
                        RefNo = booking.ReferenceNumber,
                        BookingDate = bookingDate,
                        EventDateTime = GetEventDateTime(item.TblEvent),
                        Id = innerItem.Id,
                        Ticket = innerItem.Ticket,
                        TicketNumber = innerItem.TicketNumber,
                         
                    };

                  
                    lstModel.Add(model);
                }

            }
            return lstModel;
        }
        public string GetEventDateTime(TblEvent tblEvent)
        {
            StringBuilder stringBuilder = new StringBuilder();
          
            var FromTime = eventService.ConvertTimeSpantoAM_PM(tblEvent.StartTime);
            var ToTime = eventService.ConvertTimeSpantoAM_PM(tblEvent.StartTime);

            _ = stringBuilder.Append(tblEvent.EventDate.ToString("ddd MMM yyyy") + " " + FromTime + "-" + ToTime);

            return stringBuilder.ToString();
            //Sat 17 July2023 11:45am-8:30pm
          
        }
        #endregion


    }
}
