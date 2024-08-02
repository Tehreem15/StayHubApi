using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.ViewModels;
using StayHub.Services;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly GuestService guestService;
        public GuestController(GuestService _guestService)
        {
            guestService = _guestService;

        }

        [HttpGet("GetGuests")]
        public IActionResult GetGuests()
        {
            var response = guestService.GetAllGuests();
            return Ok(response);
        }

        [HttpPost("Register")]
        public IActionResult RegisterGuests([FromForm] GuestRegisterModel model)
        {
            var response = guestService.Register(model);
            return Ok(response);
        }

        [HttpGet("GetProfile")]
        public IActionResult GetProfile([FromQuery] int guestId)
        {
            var response = guestService.GetGuestDetailByID(guestId);
            return Ok(response);
        }

        [HttpPost("EditProfile")]
        public IActionResult EditProfile([FromForm] GuestViewModel model)
        {       
                var response = guestService.EditProfile(model);
                return Ok(response);
        }

        [HttpGet("Delete")]
        public IActionResult Delete([FromQuery] int guestId)
        {
            var response = guestService.DeleteGuestAccount(guestId);
            return Ok(response);
        }

    }
}
