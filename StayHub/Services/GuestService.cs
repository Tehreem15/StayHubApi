using Microsoft.IdentityModel.Tokens;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;

namespace StayHub.Services
{
    public class GuestService
    {
        private readonly StayHubContext db;
        private readonly IWebHostEnvironment environment;
        public GuestService(StayHubContext _db, IWebHostEnvironment _environment)
        {

            db = _db;
            environment = _environment;
        }

        public string UploadedFile(IFormFile ProfileImage)
        {
            string uniqueFileName = null;
            if (ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(environment.WebRootPath, "guestprofile");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public ResponseModel Register(GuestRegisterModel model)
        {
            ResponseModel response = new ResponseModel();
            if (model != null)
            {
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                TblUser tblGuest = new TblUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                    Id = 0,
                    IsActive = true,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    Role = "GUEST",
                    Title = model.Title,
                    Zipcode = model.Zipcode,
                    GuestNumber = GenerateGuestNumber()
                };
                if (model.imageFile != null)
                {
                    tblGuest.ImgPath = UploadedFile(model.imageFile);
                }

                response.Success = db.SaveChanges() > 0;        
            }
            return response;
        }

        public ResponseModel EditProfile(GuestViewModel model)
        {
            ResponseModel response = new ResponseModel();
            TblUser tblGuest = db.tblUsers.Where(g=>g.Id == model.Id).FirstOrDefault();   
            if (tblGuest!=null)
            {

                tblGuest.FirstName = model.FirstName;
                tblGuest.LastName = model.LastName;
                tblGuest.Email = model.Email;
                tblGuest.PhoneNumber = model.PhoneNumber;
                tblGuest.Id = model.Id;
                tblGuest.Address = model.Address;
                tblGuest.City = model.City;
                tblGuest.Country = model.Country;
                tblGuest.Title = model.Title;
                tblGuest.Zipcode = model.Zipcode;
                
                };
                if (model.imageFile != null)
                {
                    tblGuest.ImgPath = UploadedFile(model.imageFile);
                }

                response.Success = db.SaveChanges() > 0;
            
            return response;
        }

        private string GenerateGuestNumber()
        {
            string newReferenceNumber = "SHG-";
            int ReferenceNumber = GetGuestNumber();
            string Reference = Convert.ToString(ReferenceNumber + 1);
            newReferenceNumber += Reference.PadLeft(5, '0');
            return newReferenceNumber;
        }

        public int GetGuestNumber()
        {

            int Reference = 0;
            var guest = db.tblUsers.Where(r=>r.Role=="GUEST").OrderByDescending(i => i.Id).FirstOrDefault();
            if (guest != null && !string.IsNullOrEmpty(guest.GuestNumber))
            {
                string[] subs = guest.GuestNumber.Split(' ', '-');
                Reference = Convert.ToInt32(subs[1]);
            }
            return Reference;
        }
        public ResponseModel DeleteGuestAccount(long Id)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                TblUser guest = db.tblUsers.Where(s => s.Id == Id).FirstOrDefault();
                if (guest != null)
                {
                    var bookings = db.tblBookings.Where(s => s.GuestId == Id).ToList();
                    if (bookings != null && bookings.Count > 0)
                    {
                        foreach (var book in bookings) 
                        {
                            var bookingEvents = db.tblBookingEvents.Where(b => b.BookingId == book.Id).ToList();
                            if (bookingEvents != null && bookingEvents.Count > 0)
                            {
                                foreach (var bookEvent in bookingEvents)
                                {
                                    var tickets = db.tblBookingEventTickets.Where(b => b.BookingEventId == bookEvent.Id).ToList();
                                    if (tickets != null && tickets.Count > 0)
                                    {
                                        db.tblBookingEventTickets.RemoveRange(tickets);
                                    }
                                }
                                db.tblBookingEvents.RemoveRange(bookingEvents);

                                var bookingSpas = db.tblBookingSpas.Where(b => b.BookingId == book.Id).ToList();
                                if (bookingSpas != null && bookingSpas.Count > 0)
                                {
                                    db.tblBookingSpas.RemoveRange(bookingSpas);
                                }
                                var bookingGyms = db.tblBookingGyms.Where(b => b.BookingId == book.Id).ToList();
                                if (bookingGyms != null && bookingGyms.Count > 0)
                                {
                                    db.tblBookingGyms.RemoveRange(bookingGyms);
                                }
                                var bookingRooms = db.tblBookingRooms.Where(b => b.BookingId == book.Id).ToList();
                                if (bookingRooms != null && bookingRooms.Count > 0)
                                {
                                    db.tblBookingRooms.RemoveRange(bookingRooms);
                                }
                                var bookingRoomServices = db.tblBookingRoomServices.Where(b => b.BookingId == book.Id).ToList();
                                if (bookingRoomServices != null && bookingRoomServices.Count > 0)
                                {
                                    db.tblBookingRoomServices.RemoveRange(bookingRoomServices);
                                }
                                db.tblBookings.RemoveRange(bookings);
                                db.SaveChanges();
                            }
                        }
                       
                       
                    }
                    db.tblUsers.Remove(guest);
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

        public ResponseModel<GuestViewModel> GetGuestDetailByID(long Id)
        {
            ResponseModel<GuestViewModel> response = new ResponseModel<GuestViewModel>();
            GuestViewModel model = new GuestViewModel();
            TblUser tblGuest = db.tblUsers.Where(x => x.Id == Id).FirstOrDefault();
            if (tblGuest != null)
            {
                model.Id = Id;
                model.FirstName = tblGuest.FirstName;
                model.LastName = tblGuest.LastName;
                model.Email = tblGuest.Email;
                model.PhoneNumber = tblGuest.PhoneNumber;
                model.Zipcode = tblGuest.Zipcode;
                model.Address = tblGuest.Address;
                model.ImgPath= tblGuest.ImgPath;
                model.Title = tblGuest.Title;
                model.City = tblGuest.City;
                model.Country = tblGuest.Country;
                
                response.Success = true;
            }

            response.Success = false;
            return response;
        }

        public ResponseListModel<GuestViewModel> GetAllGuests()
        {
            ResponseListModel<GuestViewModel> response = new ResponseListModel<GuestViewModel>();
            response.Success = true;
            List<TblUser> guests = db.tblUsers.Where(s => s.Role.Trim() == "GUEST" ).ToList();
            List<GuestViewModel> modelList = new List<GuestViewModel>();

            if (guests != null && guests.Count() > 0)
            {

                foreach (TblUser item in guests)
                {
                    GuestViewModel staff = new GuestViewModel()
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        Id = item.Id,
                        ImgPath=item.ImgPath,
                        City=item.City,
                        Country=item.Country,
                        Title=item.Title,
                        Zipcode=item.Zipcode,
                        LastName = item.LastName,
                        Address=item.Address,
                        PhoneNumber = item.PhoneNumber,
                    };

                    modelList.Add(staff);
                }
                response.List = modelList;
            }
            return response;
        }

    

    }
}
