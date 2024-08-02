using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using System;

namespace StayHub.Services
{
    public class SpaService
    {
        private readonly StayHubContext db;

        public SpaService(StayHubContext _db)
        {
            db = _db;
        }

        public ResponseModel SaveSpa(SpaViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                TblSpa spa = new TblSpa();
                if (model.Id > 0)
                {
                    spa= db.tblSpas.Where(s => s.Id == model.Id).FirstOrDefault();
                    if (spa != null)
                    {
                        spa.Id = model.Id;
                        spa.Name = model.Name;
                        spa.Description = model.Description;
                        spa.Capacity = model.Capacity;
                        spa.Price = model.Price;
                        spa.ClosingTime = model.ClosingTime;
                        spa.OpeningTime = model.OpeningTime;
                  
                        db.tblSpas.Update(spa);
                    }

                }

                if (model.Id == 0)
                {
                    spa = new TblSpa();
                    spa.Id = model.Id;
                    spa.Name = model.Name;
                    spa.Description = model.Description;
                    spa.Capacity = model.Capacity;
                    spa.Price = model.Price;
                    spa.ClosingTime = model.ClosingTime;
                    spa.OpeningTime = model.OpeningTime;
                    db.tblSpas.Add(spa);
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
        public ResponseListModel<SpaViewModel> SpasList()
        {
            ResponseListModel<SpaViewModel> response = new ResponseListModel<SpaViewModel>();
            response.Success = true;
            List<TblSpa> spas = db.tblSpas.ToList();
          
            List<SpaViewModel> modelList = new List<SpaViewModel>();

            if (spas != null && spas.Count() > 0)
            {

                foreach (TblSpa item in spas)
                {
                    SpaViewModel spa = new SpaViewModel();
                    spa.Id = item.Id;
                    spa.Name = item.Name;
                    spa.Description = item.Description;                
                    spa.Capacity = item.Capacity;
                    spa.Price=item.Price;
                    spa.ClosingTime = item.ClosingTime;
                    spa.OpeningTime = item.OpeningTime;
                  
                    modelList.Add(spa);
                }
                response.List = modelList;
            }
            return response;
        }
        public ResponseModel<SpaViewModel> GetSpaById(long Id)
        {
            ResponseModel<SpaViewModel> response = new ResponseModel<SpaViewModel>();
            TblSpa model = db.tblSpas.Where(s => s.Id == Id).FirstOrDefault();
            if (model != null)
            {
                SpaViewModel spa = new SpaViewModel();            
                spa.Id = model.Id;
                spa.Name = model.Name;
                spa.Description = model.Description;
                spa.Capacity = model.Capacity;
                spa.Price = model.Price;
                spa.ClosingTime = model.ClosingTime;
                spa.OpeningTime = model.OpeningTime;
                response.Data = spa;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return response;

        }

        public ResponseModel<decimal> GetSpaPrice(long Id)
        {
            ResponseModel<decimal> response = new ResponseModel<decimal>();
            TblSpa spa= db.tblSpas.Where(s => s.Id == Id).FirstOrDefault();
            if (spa != null)
            {
                response.Data = spa.Price;
                response.Success = true;
            }
            else
            {
                response.Data = 0;
                response.Success = false;
            }
            return response;

        }
        public ResponseModel<int> GetSpaCapacity(long Id)
        {
            ResponseModel<int> response = new ResponseModel<int>();
            TblSpa spa = db.tblSpas.Where(s => s.Id == Id).FirstOrDefault();
            if (spa != null) 
            { 
                response.Data = spa.Capacity;
                response.Success = true;
            }
            else
            {
                response.Data = 0;
                response.Success = false;
            }
            return response;

        }


        public ResponseModel DeleteSpa(long Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                TblSpa spa = db.tblSpas.Where(s => s.Id == Id).FirstOrDefault();
                if (spa != null)
                {
                    db.tblSpas.Remove(spa);
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

        public ResponseModel<int> ValidateSpa(long SpaId, DateTime SpaDate)
        {

            ResponseModel<int> response = new ResponseModel<int>();
            try
            {
                int RemainingCapacity = -1;
                TblSpa tblSpa = db.tblSpas.Where(g => g.Id == SpaId).FirstOrDefault();
                if (tblSpa != null)
                {
                    int MaxCapacity = tblSpa.Capacity;
                    int BookedPersons = db.tblBookingSpas.Where(g => g.Id == SpaId && g.SpaDate == SpaDate
                    ).Sum(s=>s.NoOfPersons);
                    RemainingCapacity = MaxCapacity - BookedPersons;
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

        public ResponseModel ValidateSpaCapacity(long SpaId, DateTime SpaDate, int NoOfPersons, string Name)
        {
            var response = new ResponseModel();
            var result = ValidateSpa(SpaId, SpaDate);
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
            return response;
        }


    }
}
