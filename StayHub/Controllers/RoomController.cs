using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using Stripe;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private RoomService roomService;
    
        public RoomController(RoomService _roomService)
        {
           roomService = _roomService;
        }
        #region RoomCrud
        [HttpGet("GetRooms")]
        [Authorize]
        public IActionResult GetRooms()
        {
            var response = roomService.GetRoomList();
            return Ok(response);
        }
        [HttpGet("GetRoomDetail")]
        [Authorize]
        public IActionResult GetRoomDetail([FromQuery] long id)
        {
            var response = roomService.GetRoomById(id);
            return Ok(response);
        }

        [HttpPost("SaveRoom")]
    
        public IActionResult SaveRoom([FromBody] RoomViewModel model)
        {
            var response = roomService.SaveRoomDetail(model);
            return Ok(response);
        }
        [HttpGet("DeleteRoom")]
        [Authorize]
        public IActionResult DeleteRoom([FromQuery] long id)
        {
            var response = roomService.DeleteRoom(id);
            return Ok(response);
        }
        #endregion

        #region RoomPricesAndAvailability
       
        [HttpPost("SaveRoomPrice")] //Settings
        public IActionResult Settings([FromBody] RoomPriceViewModel model)
        {
            var response = roomService.SaveRoomPrice(model);
            return Ok(response);
        }

        [HttpPost("GetRoomByType")]
        public IActionResult GetRoomByType([FromQuery] string type)
        {
            var response = roomService.GetRoomByType(type);
            return Ok(response);
        }
        [HttpPost("GetAvailabilityRecord")]
        public IActionResult AvailabilityRecord([FromBody] RoomPriceViewModel model)
        {
           // ViewBag.IsPriceSetting = model.IsPriceSetting;
            var response = roomService.RoomAvailabilityList(model);
            return Ok(response);
           // return PartialView("_AvailabilityRecord", list);
        }
        [HttpPost("UpdateSinglePrice")]
        public IActionResult UpdateSinglePrice([FromBody] UpdatePriceModel model)
        {
            var response= roomService.UpdateSinglePrice(model.id, model.price, model.addPersonPrice, model.bookingStatus);
            return Ok(response);
        }

        [HttpGet("GetSinglePrice")]
        public IActionResult GetSinglePrice([FromQuery] long id)
        {
            var response = roomService.GetSinglePrice(id);
            return Ok(response);
           
        }
        [HttpPost("GetAvaliableRoom")]
        public IActionResult GetAvaliableRoom(RoomPriceViewModel model)
        {
            var response = new ResponseModel<object>();
            bool isValid = true;
            var result = roomService.GetRoom(model.Id);
            model.RoomName = result.Data.Name;
            response.Data= new
            {
                isValid,
                RoomId = model.Id,
                ItemTotalPrice = model.Price,
                NoofNightStay = model.NoofNights,
                CheckOutDate = model.EndDate,
                CheckInDate = model.Startdate,
                MaxPerson = model.MaxAdditionalPerson,
                ResourceName = model.RoomName,
                Details = model.Detail
            };
            return Ok(response);
        }
        [HttpGet("GetDetail")]
        public IActionResult Detail([FromQuery] long id, [FromQuery] double total)
        {
            var response = roomService.GetRoomById(id);
            response.Data.Price = Convert.ToDecimal(total);
            return Ok(response);
        }
        [HttpGet("GetRoomById")]
        public IActionResult GetRoomById([FromQuery] long id)
        {
            var response =  roomService.GetRoomById(id);
            return Ok(response);
        }
        [HttpPost("CalculateBookingPrice")]
        public IActionResult CalculateBookingPrice([FromBody] RoomPriceGeneralModel model)
        {
            var response= new ResponseModel<RoomPriceGeneralModel>();
            RoomViewModel resourceData = roomService.GetRoomById(model.Id).Data;
            List<TblRoomPrice> accomodationAvailablities = roomService.GetAvailablitiesRoomList(s => s.RoomId == model.Id && (s.Date >= model.Startdate && s.Date <= model.EndDate)).List;
            TimeSpan? datediff = model.EndDate - model.Startdate;
            if (datediff.HasValue)
                model.NoofNights = Convert.ToInt32(datediff.Value.TotalDays);

            if (model.Startdate.HasValue && model.EndDate.HasValue)
            {
                for (DateTime i = model.Startdate.Value.Date; i < model.EndDate.Value; i = i.AddDays(1))
                {
                    var availabilityByDate = accomodationAvailablities.FirstOrDefault(s => s.Date.Date == i.Date.Date);
                    if (availabilityByDate != null && availabilityByDate.Status == "A")
                    {
                        model.Price += availabilityByDate.Price;
                        if (model.MaxAdditionalPerson > 0)
                        {
                            model.Price = model.Price + (model.MaxAdditionalPerson * availabilityByDate.AddPersonPrice);
                        }
                           
                    }
                    else
                    {
                        if (availabilityByDate == null)
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.NotReady
                            });
                        else if (availabilityByDate.Status == "B")
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.AlreadyBooked
                            });
                        else if (availabilityByDate.Status == "N")
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.NotAvailable
                            });
                    }
                }

               
                }
            response.Success = true;
            response.Data = model;
            return Ok(response);
        }

        #endregion


    }
}
