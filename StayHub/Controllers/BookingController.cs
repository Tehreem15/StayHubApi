using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StayHub.Data.ViewModels;
using StayHub.Services;


namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase 
    { 
        private readonly BookingService bookingService;
      
        public BookingController(BookingService _bookingService)
        {

            bookingService = _bookingService;
           
        }

        [HttpGet("GetBookings")]
        [Authorize]
        public IActionResult GetBookings([FromQuery] string status, [FromQuery] int guestId)
        {
            var response = bookingService.GetAllBookings(status, guestId);
            return Ok(response);
        }


        [HttpGet("CancelBooking")]
        [Authorize]
        public IActionResult Cancel([FromQuery] long bookingId)
        {
            var response = bookingService.CancelBooking(bookingId);
            return Ok(response);
        }

        [HttpGet("GetBookingDetail")]
        [Authorize]
        public IActionResult Details([FromQuery]  long BookingID)
        {
            var response = bookingService.GetBookingDetails(BookingID);
            return Ok(response);
        }

    }

}
