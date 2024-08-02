using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using System;
using System.Drawing;
namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymController : Controller
    {
        private readonly GymService gymService;
      
        public GymController(GymService gymService)
        {
           
            this.gymService = gymService;
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetGyms")]
        public IActionResult GetGyms([FromQuery] string gender)
        {

            var response = gymService.GymsList(gender);
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetGymById")]
        public IActionResult GetGymById([FromQuery] long id)
        {
            var response = gymService.GetGymById(id);
            return Ok(response);
        }
        [Authorize]
        [HttpPost("AddEditGym")]
        public IActionResult AddEdit([FromBody] GymViewModel model)
        {
            
            var response = gymService.SaveGym(model);
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("DeleteGym")]
        public IActionResult Delete([FromQuery] long Id)
        {
            var response = gymService.DeleteGym(Id);
            return Ok(response);
        }



        [HttpPost("ValidateGymCapacity")]
        public IActionResult ValidateGymCapacity([FromBody] GymModel model)
        {

            var response = gymService.ValidateGymCapacity(model.GymId,model.MonthRange, model.Name);
            return Ok(response);
        }

    }
}
