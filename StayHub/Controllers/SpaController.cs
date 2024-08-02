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
        private readonly SpaService spaService;

        public SpaController(SpaService _spaService)
        {

            spaService = _spaService;
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetSpas")]
        public IActionResult GetSpas()
        {

            var response = spaService.SpasList();
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetSpaById")]
        public IActionResult GetSpaById([FromQuery] long SpaId)
        {
            var response = spaService.GetSpaById(SpaId);
            return Ok(response);
        }
        [Authorize]
        [HttpPost("AddEditSpa")]
        public IActionResult AddEdit([FromBody] SpaViewModel model)
        {

            var response = spaService.SaveSpa(model);
            return Ok(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("DeleteSpa")]
        public IActionResult Delete([FromQuery] long Id)
        {
            var response = spaService.DeleteSpa(Id);
            return Ok(response);
        }



        [HttpPost("ValidateSpaCapacity")]
        public IActionResult ValidateSpaCapacity([FromBody] SpaModel model)
        {

            var response = spaService.ValidateSpaCapacity(model.SpaId, model.SpaDate, model.NoOfPersons, model.Name);
            return Ok(response);
        }

     
    }
}
