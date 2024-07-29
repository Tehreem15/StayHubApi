using Microsoft.EntityFrameworkCore;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using System.Data;

namespace StayHub.Services
{
    public class GymService 
    {
        private readonly StayHubContext db;

        public GymService(StayHubContext _db)
        {
            db = _db;
        }

        public ApiBaseResponse SaveGym(GymViewModel model)
        {
            ApiBaseResponse response = new ApiBaseResponse();   
            try
            {
                TblGym gym = new TblGym();
                if (model.Id > 0)
                {
                    gym= db.tblGyms.Where(s => s.Id == model.Id).FirstOrDefault();
                    if (gym != null)
                    {
                        gym.Id = model.Id;
                        gym.Name = model.Name;
                        gym.Description = model.Description;
                        gym.Rules = model.Rules;
                        gym.Equiqment = model.Equiqment;
                        gym.Capacity = model.Capacity;
                        gym.Fee = model.Fee;
                        gym.ClosingTime = model.ClosingTime;
                        gym.OpeningTime = model.OpeningTime;
                        gym.Gender = model.Gender;
                        db.tblGyms.Update(gym);
                    }
    
                }

                if (model.Id == 0)
                {
                    gym = new TblGym();
                    gym.Id = model.Id;
                    gym.Name = model.Name;
                    gym.Description = model.Description;
                    gym.Rules = model.Rules;
                    gym.Equiqment = model.Equiqment;
                    gym.Capacity = model.Capacity;
                    gym.Fee = model.Fee;
                    gym.ClosingTime = model.ClosingTime;
                    gym.OpeningTime = model.OpeningTime;
                    gym.Gender = model.Gender;
                    db.tblGyms.Add(gym);
                }

                response.Success = db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        //used inm admin and frontend
        public ApiListResponse<GymViewModel> GymsList(string gender)
        {
            ApiListResponse<GymViewModel> response = new ApiListResponse<GymViewModel>();
            response.Success = true;
            List<TblGym> gyms = db.tblGyms.ToList();
            if (string.IsNullOrEmpty(gender))
            {
                int GenderNo = gender == "Male" ? 1 : 2;
                gyms=gyms.Where(g=>g.Gender== GenderNo).ToList();
            }
            List<GymViewModel> modelList = new List<GymViewModel>();
          
            if (gyms!=null && gyms.Count() > 0)
            {

                foreach (TblGym item in gyms)
                {
                    GymViewModel gym = new GymViewModel();
                    gym.Id = item.Id;
                    gym.Name = item.Name;
                    gym.Description = item.Description;
                    gym.Rules = item.Rules;
                    gym.Equiqment = item.Equiqment;
                    gym.Capacity = item.Capacity;
                    gym.Fee = item.Fee;
                    gym.ClosingTime = item.ClosingTime;
                    gym.OpeningTime = item.OpeningTime;
                    gym.Gender = item.Gender;
                    modelList.Add(gym);
                }
                response.List=modelList;
            }
            return response;
        }
        public ApiResponse<GymViewModel> GetGymById(long Id)
        {
            ApiResponse<GymViewModel> response= new ApiResponse<GymViewModel>();
            TblGym model= db.tblGyms.Where(s => s.Id == Id).FirstOrDefault();
            if (model != null)
            {
                GymViewModel gym = new GymViewModel();
                gym.Id = model.Id;
                gym.Name = model.Name;
                gym.Description = model.Description;
                gym.Rules = model.Rules;
                gym.Equiqment = model.Equiqment;
                gym.Capacity = model.Capacity;
                gym.Fee = model.Fee;
                gym.ClosingTime = model.ClosingTime;
                gym.OpeningTime = model.OpeningTime;
                gym.Gender = model.Gender;
                response.Data = gym;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return response;

        }

        public ApiResponse<decimal> GetGymPrice(long Id)
        {
            ApiResponse<decimal> response= new ApiResponse<decimal>();  
            TblGym gym = db.tblGyms.Where(s => s.Id == Id).FirstOrDefault();
            if (gym != null)
            {
                response.Data = gym.Fee;
               response.Success = true;
            }
            else
            {
                response.Data = 0;
                response.Success = false;
            }
            return response;

        }
        public ApiResponse<int> GetGymCapacity(long Id)
        {
            ApiResponse<int> response = new ApiResponse<int>();
            TblGym gym = db.tblGyms.Where(s => s.Id == Id).FirstOrDefault();
            if (gym != null)
            {
                response.Data = gym.Capacity;
                response.Success = true;
            }
            else
            {
                response.Data = 0;
                response.Success = false;
            }
            return response;

        }


        public ApiBaseResponse DeleteGym(long Id)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            try
            {
                TblGym gym = db.tblGyms.Where(s => s.Id == Id).FirstOrDefault();
                if (gym != null)
                {
                    db.tblGyms.Remove(gym);
                    response.Success = db.SaveChanges()>0;
                }
                else
                {
                  
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message = ex.Message;
            }
            return response;

        }

        public ApiResponse<int> ValidateGym(long GymId, int Month)
        {

            ApiResponse<int> response= new ApiResponse<int>();
            try
            {
                int RemainingCapacity = -1;
                TblGym tblGym= db.tblGyms.Where(g => g.Id == GymId).FirstOrDefault();
                if (tblGym != null) {
                    int MaxCapacity = tblGym.Capacity;
                    int Booked = db.tblBookingGyms.Where(g => g.Id == GymId && g.Month == Month
                    && g.Year== DateTime.Now.Year).Count();
                    RemainingCapacity = MaxCapacity - Booked;
                    response.Data = RemainingCapacity;
                    response.Success = true;
                }
               
            }

            catch (Exception ex)
            {
                string message = ex.Message;
                response.Success = false;
                response.Data = -1;
               
            }
            return response;

        }
    }
}
