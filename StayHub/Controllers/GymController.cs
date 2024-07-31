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
        public IActionResult GetGymById([FromQuery] long gymId)
        {
            var response = gymService.GetGymById(gymId);
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

            var response = ValidateGymCapacity(model.GymId,model.MonthRange, model.Name);
            return Ok(response);
        }

        public IActionResult ValidateGymCapacity(long gymId, int MonthRange,string Name)
        {
            var response = new ResponseModel();
            var result = gymService.ValidateGym(gymId, MonthRange);
            if (result.Success)
            {
                string MonthNames = GetMonthRange(MonthRange);
                int remainingCapacity = result.Data;
                if (remainingCapacity <= -1 || remainingCapacity == 0)
                {
                    response.Success = false;
                    response.Message =Name+ " has no more capacity for "+MonthNames;
                }
                else
                {
                    response.Success = true;
                }
            }
            return Ok(response);
        }
  
       private string GetMonthRange(int Month)
        {
            string sessionMonth = "";
            switch (Month)
            {
                case 1:
                    sessionMonth = "Jan-Mar.";
                    break;
                case 2:
                    sessionMonth = "Apr-June.";
                    break;
                case 3:
                    sessionMonth = "July-Sept.";
                    break;
                case 4:
                    sessionMonth = "Oct-Dec.";
                    break;
            }
            return sessionMonth;
        }
    }
}
