using Microsoft.EntityFrameworkCore;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using Stripe;
using System.Data;
using System.Text;

namespace StayHub.Services
{
    public class EventService 
    {
  
        private StayHubContext db;
        private readonly IWebHostEnvironment environment;
        public EventService(StayHubContext _db, IWebHostEnvironment _environment)
        {
            db = _db;
            environment = _environment;
        }

        public ResponseModel AddEditEvent(EventVM model)
        {
            ResponseModel response= new ResponseModel();
            TblEvent tblevent = new TblEvent();

            try
            {
                tblevent.Id = model.Id;
                tblevent.Location = model.Location;
                tblevent.BookingStartDate = model.BookingEndDate;
                tblevent.Name = model.Name;
                tblevent.ShortDescription = model.ShortDescription;
                tblevent.BookingEndDate = model.BookingEndDate;
                tblevent.EventDate = model.EventDate;
                tblevent.StartTime = model.StartTime;
                tblevent.EndTime = model.EndTime;
                tblevent.Description = model.Description;
                tblevent.EventImageUrl = model.EventImage;
                tblevent.MaxTicket = model.MaxTicket;
                tblevent.AdultTicketPrice = model.AdultTicketPrice;
                tblevent.ChildTicketPrice=model.ChildTicketPrice;
                   
                if (tblevent.Id == 0)
                {
                    _ = db.tblEvents.Add(tblevent);


                }
                else if (tblevent.Id > 0)
                {
                    db.tblEvents.Update(tblevent);

                }
                response.Success= db.SaveChanges()>0;
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message;

            }
            return response;
        }

        public string UploadEventImages(IFormFile ProfileImage)
        {
            string uniqueFileName = null;
            if (ProfileImage != null)
            {

                string uploadsFolder = Path.Combine(environment.WebRootPath, "eventimages");
                if (!Directory.Exists(uploadsFolder))
                {
                    _ = Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public ResponseModel Delete(long Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var tblevent = db.tblEvents.Where(x => x.Id == Id).FirstOrDefault();
                if (tblevent!=null)
                {
                    db.tblEvents.Remove(tblevent);
                    response.Success = db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }
   
        public ResponseModel<EventVM> GetEventModel(long id)
        {
            ResponseModel<EventVM> response = new ResponseModel<EventVM>();
            try
            {
                var tblevent = db.tblEvents.Where(x => x.Id == id).FirstOrDefault();
                if (tblevent != null)
                {
                    EventVM eventVM = new EventVM()
                    {
                        EventFile = null,
                        BookingStartDate = tblevent
                        .BookingStartDate,
                        AdultTicketPrice = tblevent.AdultTicketPrice,
                        ShortDescription = tblevent.ShortDescription,
                        Description = tblevent.Description,
                        BookingEndDate = tblevent.BookingEndDate,
                        ChildTicketPrice = tblevent.ChildTicketPrice,
                        EndTime = tblevent.EndTime,
                        EventDate = tblevent.EventDate,
                        EventImage = tblevent.EventImageUrl,
                        StartTime = tblevent.StartTime,
                        Id = tblevent.Id
                        ,
                        Location = tblevent.Location,
                        MaxTicket=tblevent.MaxTicket,
                        Name = tblevent.Name,
                        
                    };
                    response.Success = true;
                    response.Data = eventVM;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }

        public ResponseListModel<TblEvent> GetAllEvent()
        {
            ResponseListModel<TblEvent> response = new ResponseListModel<TblEvent>();
            try
            {
                var tblevents = db.tblEvents.OrderBy(x => x.BookingStartDate).ToList();
             
                response.Success = true;
                response.List = tblevents;
             
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }
        public ResponseListModel<TblEvent> GetAllEvents()
        {
            ResponseListModel<TblEvent> response = new ResponseListModel<TblEvent>();
            try
            {
                var tblevents = db.tblEvents.OrderBy(x => x.EventDate).ToList();

                response.Success = true;
                response.List = tblevents;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }




        public ResponseModel<int> CheckBookingEventTickets(long EventId)
        {

            ResponseModel<int> response= new ResponseModel<int>();
            TblEvent EventDetail = db.tblEvents.Where(i => i.Id == EventId).FirstOrDefault();
            try
            {
                int RemainingAddmissionTicket = -1;
                if (EventDetail != null)
                {
                    int MaxAdmissionTicket = EventDetail.MaxTicket;
                    int BookedAdmissionTicket = db.tblBookingEvents.Include(e => e.TblBooking).Where(e => e.EventId == EventId && e.TblBooking.Status == "Paid")
                        .Sum(e => e.NoOfAdultTicket + e.NoOfChildTicket);
                    RemainingAddmissionTicket = MaxAdmissionTicket - BookedAdmissionTicket;
                    response.Success = true;
                }
           
                //Case 1:
                //result is greater than zero show remaining tickets

                //Case 2:
                //result is -1 , less than -1 or Zero than show no ticket available

            
                //if result return -1 then tickets are zero
                response.Data = RemainingAddmissionTicket;
               
              
            }


            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Data = -1;
                response.Success = false;
            }
            return response;


        }

        public string ConvertTimeSpantoAM_PM(TimeSpan? modeltime)
        {


            TimeSpan timeSpan = new TimeSpan();
            TimeSpan value = modeltime ?? timeSpan;
            DateTime time = DateTime.Today.Add(value);
            string STime = time.ToString("hh:mmtt");
            string[] strlist = STime.Split(' ');
            StringBuilder stime = new StringBuilder();
            _ = stime.Append(strlist[0] + ' ' + strlist[1].ToUpper());
            return stime.ToString();
        }
        public ResponseModel<EventViewModel> GetSpecificEvent(long eventId)
        {
            var obj = db.tblEvents.Where(e => e.Id == eventId && e.BookingEndDate >= DateTime.Now).FirstOrDefault();
            ResponseModel<EventViewModel> response = new ResponseModel<EventViewModel>();
            if (obj == null)
            {
                response.Success = false;
                return response;
            }
            EventViewModel emodel = new EventViewModel()
            {
                Id = obj.Id,
                Name = obj.Name,
                EventImage = obj.EventImageUrl,
                Location = obj.Location,
                EventDate = obj.EventDate,
                EndDate = obj.BookingEndDate,
                StartDate = obj.BookingStartDate,
                Description = obj.Description,
                ShortDescription = obj.ShortDescription,
                strStartTime = ConvertTimeSpantoAM_PM(obj.StartTime) ,
                strEndTime = ConvertTimeSpantoAM_PM(obj.EndTime),         
                AdultTicketPrice = obj.AdultTicketPrice,             
                ChildTicketPrice = obj.ChildTicketPrice,             
                MaxTicket = obj.MaxTicket,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,

            };
    
            int BookedAdmissionTicket = db.tblBookingEvents.Include(e => e.TblBooking).Where(e => e.EventId == obj.Id && e.TblBooking.Status == "Paid")
                .Sum(e => e.NoOfAdultTicket + e.NoOfChildTicket);
            emodel.RemainingTicket = obj.MaxTicket - BookedAdmissionTicket; ;
            response.Data = emodel;
            response.Success = true;
            return response;
        }



        public ResponseListModel<EventViewModel> FillEventModelToEventViewModel(List<TblEvent> model)
        {
            ResponseListModel<EventViewModel> response = new ResponseListModel<EventViewModel>();
            List<EventViewModel> eventViewModels = new List<EventViewModel>();
            foreach (TblEvent item in model)
            {
                EventViewModel eventViewModel = new EventViewModel
                {
                    Id = item.Id,
                    Location = item.Location,
                    EndDate = item.BookingEndDate,
                    Name = item.Name,
                    EventDate = item.EventDate,
                    ShortDescription = item.ShortDescription,
                    StartDate = item.BookingStartDate,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Description = item.Description??"",
                
                  
                };
              
                eventViewModel.EventImage = item.EventImageUrl??"";
                eventViewModel.MaxTicket = item.MaxTicket;
                eventViewModel.AdultTicketPrice = item.AdultTicketPrice;
                eventViewModel.ChildTicketPrice = item.ChildTicketPrice;
              
                eventViewModel.strStartTime = ConvertTimeSpantoAM_PM(item.StartTime);
                eventViewModel.strEndTime = ConvertTimeSpantoAM_PM(item.EndTime);
             
                eventViewModels.Add(eventViewModel);
            }
            response.List= eventViewModels;
            response.Success = true;
            return response;
        }

        public ResponseListModel<KeyValueModel> GetStayHubEvents()
        {
            ResponseListModel<KeyValueModel> response = new ResponseListModel<KeyValueModel>() ;
            try
            {
                response.List = db.tblEvents
                    .OrderBy(s => s.Id)
                    .Select(s => new KeyValueModel
                    {
                        Key=s.Id,Value=s.Name,
                    }).ToList();
                response.Success = true;
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
        public ResponseModel<TblEvent> GetEventDetail(long id)
        {
            ResponseModel<TblEvent> response = new ResponseModel<TblEvent>();
            try
            {
                var tblevent = db.tblEvents.Where(x => x.Id == id).FirstOrDefault();
                if (tblevent != null)
                {
                    response.Success = true;
                    response.Data = tblevent;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }

      
        public ResponseListModel<TblEvent> GetEvents()
        {
            ResponseListModel<TblEvent> response = new ResponseListModel<TblEvent>();
            try
            {
                var tblevents = db.tblEvents.ToList();

                response.Success = true;
                response.List = tblevents;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;

        }




    }
}
