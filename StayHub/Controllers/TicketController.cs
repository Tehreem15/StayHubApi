using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.ViewModels;
using StayHub.Services;
using static Azure.Core.HttpHeader;
using System.Globalization;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService ticketService;
        public TicketController(TicketService _ticketService)
        {
            ticketService = _ticketService;
        }



        [HttpGet("GetTicketByTicketNo")]
        public IActionResult GetTicketByTicketNo([FromQuery] string ticketNo)
        {
            var response= ticketService.GetTicketByTicketNumber(ticketNo);
            return Ok(response);
        }


        [HttpGet("UpdateTicketStatus")]
        public IActionResult UpdateTicketStatus([FromQuery] int id, [FromQuery] string status)
        {
            var response= ticketService.UpdateTicketStatus(id,status);
            return Ok(response);
       
        }


    }
}
