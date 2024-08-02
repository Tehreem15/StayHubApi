using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub.Data.ViewModels;
using StayHub.Services;

namespace StayHub.Controllers
{
    public class StaffController : ControllerBase
    {
        private readonly StaffService staffService;
        public StaffController(StaffService _staffService)
        {
            staffService = _staffService;

        }

        [HttpGet("GetStaffs")]
        public IActionResult GetStaffs()
        {
            var response = staffService.GetAllStaffs();
            return Ok(response);
        }

        [HttpPost("AddEditStaff")]
        public IActionResult AddEditStaff([FromForm] StaffViewModel model)
        {
            var response = staffService.AddEditStaff(model);
            return Ok(response);
        }

        [HttpGet("GetById")]
        public IActionResult GetStaff([FromQuery] int id)
        {
            var response = staffService.GetStaffDetailByID(id);
            return Ok(response);
        }
        [HttpGet("Delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            var response = staffService.Delete(id);
            return Ok(response);
        }

        [HttpGet("GetStaffActivities")]
        public IActionResult GetStaffActivities([FromQuery] int id)
        {
            var response = staffService.GetStaffAllActivities(id);
            return Ok(response);
        }
        [HttpGet("GetStaffActivity")]
        public IActionResult GetStaffActivity([FromQuery] int id)
        {
            var response = staffService.GetStaffActivityById(id);
            return Ok(response);
        }
        [HttpPost("SaveActivity")]
        public IActionResult SaveActivity([FromForm] StaffActivityViewModel model)
        {
            var response = staffService.AddEditActivity(model);
            return Ok(response);
        }

        [HttpGet("DeleteActivity")]
        public IActionResult DeleteActivity([FromQuery] int id)
        {
            var response = staffService.DeleteActivity(id);
            return Ok(response);
        }
    }
}
