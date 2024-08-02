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

        public ResponseModel SaveGym(GymViewModel model)
        {
            ResponseModel response = new ResponseModel();   
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
        public ResponseListModel<GymViewModel> GymsList(string gender)
        {
            ResponseListModel<GymViewModel> response = new ResponseListModel<GymViewModel>();
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
        public ResponseModel<GymViewModel> GetGymById(long Id)
        {
            ResponseModel<GymViewModel> response= new ResponseModel<GymViewModel>();
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

        public ResponseModel<decimal> GetGymPrice(long Id)
        {
            ResponseModel<decimal> response= new ResponseModel<decimal>();  
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
        public ResponseModel<int> GetGymCapacity(long Id)
        {
            ResponseModel<int> response = new ResponseModel<int>();
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


        public ResponseModel DeleteGym(long Id)
        {
            ResponseModel response = new ResponseModel();
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

        public ResponseModel<int> ValidateGym(long GymId, int Month)
        {

            ResponseModel<int> response= new ResponseModel<int>();
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


        public ResponseModel ValidateGymCapacity(long gymId, int MonthRange, string Name)
        {
            var response = new ResponseModel();
            var result = ValidateGym(gymId, MonthRange);
            if (result.Success)
            {
                string MonthNames = GetMonthRange(MonthRange);
                int remainingCapacity = result.Data;
                if (remainingCapacity <= -1 || remainingCapacity == 0)
                {
                    response.Success = false;
                    response.Message = Name + " has no more capacity for " + MonthNames;
                }
                else
                {
                    response.Success = true;
                }
            }
            return response;
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
