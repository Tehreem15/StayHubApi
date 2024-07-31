using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.DBModels;
using StayHub.Data.ViewModels;
using StayHub.Services;
namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly CartService cartService;
        private readonly PaymentService paymentService;
        private readonly EventService eventService;
        private readonly GymService gymService;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BookingService bookingService;
        private readonly RoomService roomService;
        private readonly SpaService spaService;

        public CartController()
        {

        }
    }
}
