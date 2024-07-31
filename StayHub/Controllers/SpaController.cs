using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using static QRCoder.PayloadGenerator;

namespace StayHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SpaController : Controller
    {
        private readonly SpaService SpaService;

        public SpaController(SpaService SpaService)
        {

            this.SpaService = SpaService;
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetSpas")]
        public IActionResult GetSpas()
        {

            var response = SpaService.SpasList();
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetSpaById")]
        public IActionResult GetSpaById([FromQuery] long SpaId)
        {
            var response = SpaService.GetSpaById(SpaId);
            return Ok(response);
        }
        [Authorize]
        [HttpPost("AddEditSpa")]
        public IActionResult AddEdit([FromBody] SpaViewModel model)
        {

            var response = SpaService.SaveSpa(model);
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("DeleteSpa")]
        public IActionResult Delete([FromQuery] long Id)
        {
            var response = SpaService.DeleteSpa(Id);
            return Ok(response);
        }



        [HttpPost("ValidateSpaCapacity")]
        public IActionResult ValidateSpaCapacity([FromBody] SpaModel model)
        {

            var response = ValidateSpaCapacity(model.SpaId, model.SpaDate, model.NoOfPersons, model.Name);
            return Ok(response);
        }

        public IActionResult ValidateSpaCapacity(long SpaId, DateTime SpaDate, int NoOfPersons, string Name)
        {
            var response = new ResponseModel();
            var result = SpaService.ValidateSpa(SpaId,SpaDate);
            if (result.Success)
            {
              
                int remainingCapacity = result.Data;
                if (remainingCapacity <= -1 || remainingCapacity == 0)
                {
                    response.Success = false;
                    response.Message = Name + " has no more capacity for " + SpaDate.ToString("dd MMM yyyy");
                }
                else if (remainingCapacity > 0 && NoOfPersons > remainingCapacity)
                {
                    response.Success = false;
                    response.Message = "Currently " + remainingCapacity + "capacity for " + SpaDate.ToString("dd MMM yyyy");
                }
                else
                {
                    response.Success = true;
                }
            }
            return Ok(response);
        }

    }
}
