using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
namespace StayHub.Services
{
    public class StaffService
    {
        private readonly StayHubContext db;
        private readonly EmailService emailService;
        public StaffService(StayHubContext _db, EmailService _emailService)
        {
            db = _db;
           emailService =_emailService;
        }

        public ApiBaseResponse AddEditStaff(StaffViewModel model)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            TblUser tblStaff = new TblUser();
            if (model.Id > 0)
            {
                tblStaff = db.tblUsers.Where(x => x.Id == model.Id).FirstOrDefault();
                if (tblStaff!=null)
                {
                    tblStaff.Id = model.Id;
                    tblStaff.FirstName = model.FirstName;
                    tblStaff.LastName = model.LastName;
                    tblStaff.Email = model.Email;
                    tblStaff.PhoneNumber = model.PhoneNumber;
                    if (model.IsAdmin)
                    {
                        tblStaff.IsActive = model.IsActive;
                    }
                    db.tblUsers.Update(tblStaff);
                }
             
            }
            else
            {
                
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                tblStaff.FirstName = model.FirstName;
                tblStaff.LastName = model.LastName;
                tblStaff.Email = model.Email;
                tblStaff.PhoneNumber = model.PhoneNumber;
                tblStaff.Password = hashPassword;
                tblStaff.Id = 0;
                tblStaff.IsActive = model.IsActive;
                db.tblUsers.Add(tblStaff);
            }
            response.Success= db.SaveChanges()>0;
            return response;
        }

        public ApiBaseResponse Delete(long Id)
        {
            ApiBaseResponse response = new ApiBaseResponse();
           
            try
            {
                TblUser staff = db.tblUsers.Where(s => s.Id == Id).FirstOrDefault();
                if (staff != null)
                {
                    var staffActivities = db.tblStaffActivities.Where(s => s.StaffId == Id).ToList();
                    if(staffActivities!=null && staffActivities.Count > 0)
                    {
                        db.tblStaffActivities.RemoveRange(staffActivities);
                    }
                    db.tblUsers.Remove(staff);
                    response.Success = db.SaveChanges() > 0;
                }
                else
                {

                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ApiResponse<StaffViewModel> GetStaffDetailByID(long Id)
        {
            ApiResponse<StaffViewModel> response = new ApiResponse<StaffViewModel>();
            StaffViewModel model = new StaffViewModel();
            TblUser tblStaff = db.tblUsers.Where(x => x.Id == Id).FirstOrDefault();
            if (tblStaff != null)
            {
                model.Id = tblStaff.Id;
                model.FirstName = tblStaff.FirstName;
                model.LastName = tblStaff.LastName;
                model.Email = tblStaff.Email;
                model.PhoneNumber = tblStaff.PhoneNumber;
                model.IsActive = tblStaff.IsActive;
                response.Success = true;
            }

            response.Success = false;
            return response;
        }

        public ApiListResponse<StaffViewModel> GetAllStaffs()
        {
            ApiListResponse<StaffViewModel> response = new ApiListResponse<StaffViewModel>();
            response.Success = true;
            List<TblUser> staffs = db.tblUsers.Where(s=>s.Role.Trim()=="STAFF").ToList();
            List<StaffViewModel> modelList = new List<StaffViewModel>();

            if (staffs != null && staffs.Count() > 0)
            {

                foreach (TblUser item in staffs)
                {
                    StaffViewModel staff = new StaffViewModel()
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        Id = item.Id,
                        IsActive = item.IsActive,
                        LastName = item.LastName,
                        Password = item.Password,
                        PhoneNumber = item.PhoneNumber,
                    };
                   
                    modelList.Add(staff);
                }
                response.List = modelList;
            }
            return response;
        }

        //Staff Activity
        public ApiBaseResponse AddEditActivity(StaffActivityViewModel model)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            TblStaffActivity tblStaffActivity = new TblStaffActivity();
            var staff = db.tblUsers.Where(s=>s.Id== model.StaffId).FirstOrDefault();
            if (staff == null) { 
                response.Success = false;
                response.Message = "Staff Does not exist";
            } 
            else if(staff.IsActive==false) {
                response.Success = false;
                response.Message = "Please active staff account before assigning or updating activity";
            }
            else
            {


                if (model.Id > 0)
                {
                    tblStaffActivity = db.tblStaffActivities.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (tblStaffActivity != null)
                    {
                        tblStaffActivity.ActivityDate = model.ActivityDate;
                        tblStaffActivity.ActivityName = model.ActivityName;
                        tblStaffActivity.ActivityDate = model.ActivityDate;
                        tblStaffActivity.ActivityDescription = model.ActivityDescription;
                        tblStaffActivity.StaffId = model.StaffId;
                        tblStaffActivity.Id = model.Id;
                        db.tblStaffActivities.Update(tblStaffActivity);
                    }

                }
                else
                {
                    tblStaffActivity.ActivityDate = model.ActivityDate;
                    tblStaffActivity.ActivityName = model.ActivityName;
                    tblStaffActivity.ActivityDate = model.ActivityDate;
                    tblStaffActivity.ActivityDescription = model.ActivityDescription;
                    tblStaffActivity.Id = 0;
                    tblStaffActivity.StaffId = model.StaffId;
                    db.tblStaffActivities.Add(tblStaffActivity);
                    string logo = string.Empty;
                    string body = @$"<!DOCTYPE html>
                                        <html>
                                        <head>
                                            <meta charset=""utf-8"" />
                                            <title></title>
                                            
                                        </head>
                                        <body>
                                            <p>Hello {staff.FirstName + " " + staff.LastName},</p>
                                            <p>You have assign a new Activity.</p>
                                            <p>Name: {model.ActivityName}<p>
                                            <p>Description: <br/> {model.ActivityDescription}<p>
                                            <p>Date: <br/> {model.ActivityDate}<p>
                                            <p>StayHub Hotel</p>
                                            <p>Please ,</p>
                                        </body>
                                        </html>";

                  var emailResponse= emailService.SendEmail(body, staff.Email, "Assign New Activity", null, "");
                }
            }
            response.Success = db.SaveChanges() > 0;
            return response;
        }

        public ApiBaseResponse DeleteActivity(long Id)
        {
            ApiBaseResponse response = new ApiBaseResponse();

            try
            {
                TblStaffActivity staffActivity = db.tblStaffActivities.Where(s => s.Id == Id).FirstOrDefault();
                if (staffActivity != null)
                {
                   
                    db.tblStaffActivities.Remove(staffActivity);               
                    response.Success = db.SaveChanges() > 0;
                }
                else
                {

                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ApiResponse<StaffActivityViewModel> GetStaffActivityById(long Id)
        {
            ApiResponse<StaffActivityViewModel> response = new ApiResponse<StaffActivityViewModel>();
            StaffActivityViewModel model = new StaffActivityViewModel();
            TblStaffActivity tblStaffActivity = db.tblStaffActivities.Where(x => x.Id == Id).FirstOrDefault();
            if (tblStaffActivity != null)
            {
                model.Id = tblStaffActivity.Id;
                model.ActivityName = tblStaffActivity.ActivityName;
                model.ActivityDescription = tblStaffActivity.ActivityDescription;
                model.ActivityDate = tblStaffActivity.ActivityDate;
                model.StaffId= tblStaffActivity.StaffId;              
                response.Success = true;
            }

            response.Success = false;
            return response;
        }

        public ApiListResponse<StaffActivityViewModel> GetStaffAllActivities(long staffId)
        {
            ApiListResponse<StaffActivityViewModel> response = new ApiListResponse<StaffActivityViewModel>();
            response.Success = true;
            List<TblStaffActivity> staffs = db.tblStaffActivities.Where(s => s.StaffId== staffId).ToList();
            List<StaffActivityViewModel> modelList = new List<StaffActivityViewModel>();

            if (staffs != null && staffs.Count() > 0)
            {

                foreach (TblStaffActivity item in staffs)
                {
                    StaffActivityViewModel staffActivity = new StaffActivityViewModel()
                    {
                        ActivityDate= item.ActivityDate,
                        ActivityName= item.ActivityName,
                        Id= item.Id,
                        StaffId=item.StaffId,
                        ActivityDescription=item.ActivityDescription,
                    };

                    modelList.Add(staffActivity);
                }
                response.List = modelList;
            }
            return response;
        }

    }
}
