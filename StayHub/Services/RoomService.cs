using Microsoft.EntityFrameworkCore;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
namespace StayHub.Services
{
    public class RoomService
    {
        private StayHubContext db;
        public RoomService(StayHubContext _db)
        {
            db = _db;
        }
        public ResponseModel SaveRoomDetail(RoomViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                TblRoom tblRoom = new TblRoom();

                if (model.Id > 0)
                {
                    tblRoom = db.tblRooms.Include(s => s.TblRoomImages).AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefault();
                    if (tblRoom == null)
                    {
                        response.Success = false;
                        response.Message = "Room Not found againt " + model.Id;
                        return response;
                    }
                    //Add Room images url
                    List<RoomImageViewModel> addRoomImages = model.ImagesUrl.Where(i => !tblRoom.TblRoomImages.Any(o => o.Id == i.Id)).ToList();

                    TblRoomImage image = new TblRoomImage();
                    if (addRoomImages != null && addRoomImages.Count > 0)
                    {
                        foreach (RoomImageViewModel item in addRoomImages)
                        {
                            image = new TblRoomImage
                            {
                                ImageUrl = item.ImageUrl,
                                SortOrder = item.SortOrder,
                                RoomId = model.Id
                            };
                            _ = db.tblRoomImages.Add(image);
                        }
                    }

                    //Update Room image
                    List<RoomImageViewModel> updateRoomImages = model.ImagesUrl.Where(i => tblRoom.TblRoomImages.Any(o => o.Id == i.Id)).ToList();
                    if (updateRoomImages != null && updateRoomImages.Count > 0)
                    {
                        foreach (RoomImageViewModel item in updateRoomImages)
                        {
                            image = new TblRoomImage
                            {
                                Id = item.Id,
                                ImageUrl = item.ImageUrl,
                                SortOrder = item.SortOrder,
                                RoomId = model.Id
                            };
                            db.tblRoomImages.Update(image);
                        }
                    }
                    //Delete Room image
                    List<TblRoomImage> deleteRoomImages = tblRoom.TblRoomImages.Where(i => !model.ImagesUrl.Any(o => o.Id == i.Id)).ToList();
                    if (deleteRoomImages != null && deleteRoomImages.Count() > 0)
                    {
                        db.tblRoomImages.RemoveRange(deleteRoomImages);

                    }

                    tblRoom.ShortDescription = model.ShortDescription;
                    tblRoom.Name = model.Name;
                    tblRoom.Type = model.Type;
                    tblRoom.Status = model.Status;
                    tblRoom.Description = model.Description;
                    tblRoom.MaxAdditionalPerson = model.MaxAdditionalPerson;
                    db.tblRooms.Update(tblRoom);
                }

                if (model.Id == 0)
                {

                    tblRoom.ShortDescription = model.ShortDescription;
                    tblRoom.Name = model.Name;
                    tblRoom.Type = model.Type;
                    tblRoom.Status = model.Status;
                    tblRoom.Description = model.Description;
                    tblRoom.MaxAdditionalPerson = model.MaxAdditionalPerson;
                    db.tblRooms.Add(tblRoom);
                    
                    if (model.ImagesUrl != null && model.ImagesUrl.Count > 0)
                    {
                        foreach (RoomImageViewModel item in model.ImagesUrl)
                        {
                            TblRoomImage image = new TblRoomImage
                            {
                                ImageUrl = item.ImageUrl,
                                SortOrder = item.SortOrder,
                                RoomId = model.Id
                            };
                            tblRoom.TblRoomImages.Add(image);
                        }
                    }

                }

                response.Success = db.SaveChanges() > 0;
                response.Message = "";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ResponseListModel<RoomViewModel> GetRoomList()
        {
            ResponseListModel<RoomViewModel> response = new ResponseListModel<RoomViewModel>();
            List<TblRoom> rooms = db.tblRooms.Include(s => s.TblRoomImages).ToList();
            response.List = new List<RoomViewModel>();

            if (rooms != null && rooms.Count() > 0)
            {
                foreach (TblRoom item in rooms)
                {
                    RoomViewModel model = new RoomViewModel
                    {
                        Id = item.Id,
                        ShortDescription = item.ShortDescription,
                        Name = item.Name,
                        Type = item.Type,
                        Status = item.Status,
                        Description = item.Description,
                        MaxAdditionalPerson = item.MaxAdditionalPerson,
                        ImagesUrl = item.TblRoomImages.ToList().ConvertAll(s => new RoomImageViewModel { Id = s.Id, ImageUrl = s.ImageUrl, SortOrder = s.SortOrder })
                    };
                    response.List.Add(model);
                }
            }
            return response;
        }
        public ResponseModel<RoomViewModel> GetRoomById(long Id)
        {
            ResponseModel<RoomViewModel> response = new ResponseModel<RoomViewModel>();
            TblRoom tblRoom = db.tblRooms.Include(s => s.TblRoomImages).AsNoTracking().Where(s => s.Id == Id).FirstOrDefault();
            if (tblRoom == null)
            {
                return response;
            }
            var addedDates = db.tblRoomPrices
                .Where(s => s.RoomId == tblRoom.Id && s.Date >= DateTime.Today.AddDays(-1))
                .OrderBy(s => s.Date)
                .AsNoTracking()
                .Select(s => new { s.Date, s.Status })
                .ToList();
            DateTime min = DateTime.Today;
            DateTime max = addedDates.Max(s => s.Date);
            List<DateTime> allpossibleDates = Enumerable.Range(0, (max - min).Days + 1).Select(d => min.AddDays(d).Date).ToList();
            List<DateTime> disabledDates = new List<DateTime>();
            foreach (DateTime item in allpossibleDates)
            {
                var selectedDate = addedDates.Where(s => s.Date == item.Date).FirstOrDefault();
                if (selectedDate == null)
                    disabledDates.Add(item);
                else if (selectedDate.Status != "A")
                    disabledDates.Add(item);
            }

            RoomViewModel model = new RoomViewModel
            {
                Id = tblRoom.Id,
                ShortDescription = tblRoom.ShortDescription,
                Name = tblRoom.Name,
                Type = tblRoom.Type,
                Status = tblRoom.Status,
                Description = tblRoom.Description,
                MaxAdditionalPerson = tblRoom.MaxAdditionalPerson,
                ImagesUrl = tblRoom.TblRoomImages.Select(s => new RoomImageViewModel
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    SortOrder = s.SortOrder
                }).ToList(),
                DisabledDates = disabledDates.Select(s => DateTime.SpecifyKind(s.Date, DateTimeKind.Unspecified).ToString("O"))
            };
            response.Data = model;
            return response;
        }
        public ResponseModel DeleteRoom(long Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                TblRoom tblRoom = db.tblRooms.Where(a => a.Id == Id).FirstOrDefault();
                if (tblRoom != null)
                {
                    List<TblRoomImage> roomImages = db.tblRoomImages.Where(a => a.RoomId == Id).ToList();
                    if (roomImages != null && roomImages.Count > 0)
                    {
                        db.tblRoomImages.RemoveRange(roomImages);
                        db.SaveChanges();
                    }
                    List<TblRoomPrice> roomPrices = db.tblRoomPrices.Where(a => a.RoomId == Id).ToList();
                    if (roomPrices != null && roomPrices.Count > 0)
                    {
                        db.tblRoomPrices.RemoveRange(roomPrices);
                        db.SaveChanges();
                    }
                    response.Success = db.SaveChanges() > 0;
                }

            }
            catch (Exception)
            {
                response.Success = false;
            }
            return response;
        }
        public ResponseListModel<DropDownViewModel<long>> GetRoomByType(string Type)
        {
            ResponseListModel<DropDownViewModel<long>> response = new ResponseListModel<DropDownViewModel<long>>();
            List<DropDownViewModel<long>> list = db.tblRooms.Where(s => s.Type == Type).Select(s => new DropDownViewModel<long> { Value = s.Id, Text = s.Name }).ToList();
            response.List = list;
            response.Success = true;
            return response;
        }
        
        public ResponseModel SaveRoomPrice(RoomPriceViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var days = model.Days.Where(s => s != "on").Select(s => Enum.Parse(typeof(DayOfWeek), s));
                var availabilities = db.tblRoomPrices
                    .Where(s => model.Rooms.Contains(s.RoomId) &&

                    s.Date >= model.Startdate && s.Date <= model.EndDate)
                    .ToList();

                var unavailableDates = new List<TblRoomPrice>();
                foreach (var item in model.Rooms)
                {
                    for (DateTime i = model.Startdate.Value; i <= model.EndDate; i = i.AddDays(1))
                    {
                        var availability = availabilities.Where(s => s.RoomId == item && s.Date.Date == i.Date).FirstOrDefault();
                        if (availability != null)
                        {
                            if (days.Contains(availability.Date.DayOfWeek) && availability.Status != "B")
                            {

                                availability.Status = model.Status;
                                availability.AddPersonPrice = model.AddPersonPrice;
                                availability.Price = model.Price;
                            }
                        }
                        else
                        {
                            unavailableDates.Add(new TblRoomPrice
                            {
                                AddPersonPrice = model.AddPersonPrice,
                                RoomId = item,
                                Date = i.Date.Date,
                                Price = model.Price,
                                Status = model.Status,
                            });
                        }
                    }
                }
                if (unavailableDates.Count > 0)
                {
                    db.tblRoomPrices.AddRange(unavailableDates);
                }

                response.Success = db.SaveChanges() > 0;


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }
        public ResponseListModel<RoomAvailabilityViewModel> RoomAvailabilityList(RoomPriceViewModel model)
        {
            ResponseListModel<RoomAvailabilityViewModel> response = new ResponseListModel<RoomAvailabilityViewModel>();
            try
            {
                var StartDate = model.Startdate ?? DateTime.Now;
                var EndDate = model.EndDate;
                var status = model.Status;
                List<RoomAvailabilityViewModel> list = new List<RoomAvailabilityViewModel>();
                var resourceIds = model.Rooms.ToList();
                var days = model.Days.ToList();
                if (status == "L")
                {

                    response.List = db.tblRoomPrices.Include(r => r.TblRoom)
                                  .Where(a => resourceIds.Contains(a.RoomId)
                                                 && a.Date >= StartDate
                                        && a.Date <= EndDate
                                              && days.Contains(a.Date.DayOfWeek.ToString())).
                                  Select(a => new RoomAvailabilityViewModel
                                  {
                                      Status = status,
                                      AddPersonPrice = a.AddPersonPrice,
                                      Date = a.Date,
                                      Id = a.Id,
                                      Price = a.Price,
                                      RoomId = a.RoomId,
                                      Days = a.Date.DayOfWeek.ToString(),
                                      RoomName = a.TblRoom.Name
                                  }).OrderBy(x => x.Date).ToList();

                }
                else
                {
                    response.List = db.tblRoomPrices.Include(r => r.TblRoom)
                              .Where(a => resourceIds.Contains(a.RoomId)
                                             && a.Date >= StartDate
                                    && a.Date <= EndDate
                                     && a.Status == status.ToString()
                                          && days.Contains(a.Date.DayOfWeek.ToString())).
                              Select(a => new RoomAvailabilityViewModel
                              {
                                  Status = status,
                                  AddPersonPrice = a.AddPersonPrice,
                                  Date = a.Date,
                                  Id = a.Id,
                                  Price = a.Price,
                                  RoomId = a.RoomId,
                                  Days = a.Date.DayOfWeek.ToString(),
                                  RoomName = a.TblRoom.Name
                              }).OrderBy(x => x.Date).ToList();
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;


        }
        public ResponseModel<string> UpdateSinglePrice(long Id, decimal Price, decimal AddPersonPrice, string BookingStatus)
        {
            ResponseModel<string> response = new();
            try
            {
                TblRoomPrice availAbility = db.tblRoomPrices.Where(x => x.Id == Id).FirstOrDefault();
                if (availAbility != null)
                {
                    availAbility.Price = Price;
                    availAbility.AddPersonPrice = AddPersonPrice;

                    if (availAbility.Status != "B")
                    {
                        availAbility.Status = BookingStatus;

                        db.SaveChanges();
                    }
                    response.Success = true;
                    response.Data = availAbility.Status == "A" ? "Available" : availAbility.Status == "B" ? "Booked" : "Not Available";

                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = "Error";
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseModel<RoomAvailabilityPriceModel> GetSinglePrice(long Id)
        {
            ResponseModel<RoomAvailabilityPriceModel> response = new ResponseModel<RoomAvailabilityPriceModel>();
            try
            {
                TblRoomPrice availAbility = db.tblRoomPrices.Where(c => c.Id == Id).FirstOrDefault();

                if (availAbility != null)
                {
                    RoomAvailabilityPriceModel model = new RoomAvailabilityPriceModel();
                    model.price = availAbility.Price;
                    model.addPersonPrice = availAbility.AddPersonPrice;
                    model.status = availAbility.Status;
                    model.availabilityId = availAbility.Id;
                    response.Data = model;
                }


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ResponseModel<TblRoom> GetRoom(long id)
        {
            ResponseModel<TblRoom> response = new ResponseModel<TblRoom>();
            response.Data = db.tblRooms.Where(s => s.Id == id).FirstOrDefault();
            response.Success = true;
            return response;
        }
        public ResponseListModel<TblRoomPrice> GetAvailablitiesRoomList(Expression<Func<TblRoomPrice, bool>> expression)
        {
            ResponseListModel<TblRoomPrice> response = new ResponseListModel<TblRoomPrice>();
            response.List = db.tblRoomPrices.Where(expression).ToList();
            response.Success = true;
            return response;

        }

        public ResponseModel<RoomPriceGeneralModel> ValidateRoom(RoomPriceGeneralModel model)
        {
            ResponseModel<RoomPriceGeneralModel> response = new ResponseModel<RoomPriceGeneralModel>();
            RoomViewModel resourceData = GetRoomById(model.Id).Data;
            List<TblRoomPrice> accomodationAvailablities = GetAvailablitiesRoomList(s => s.RoomId == model.Id && (s.Date >= model.Startdate && s.Date <= model.EndDate)).List;
            TimeSpan? datediff = model.EndDate - model.Startdate;
            if (datediff.HasValue)
                model.NoofNights = Convert.ToInt32(datediff.Value.TotalDays);

            if (model.Startdate.HasValue && model.EndDate.HasValue)
            {
                for (DateTime i = model.Startdate.Value.Date; i < model.EndDate.Value; i = i.AddDays(1))
                {
                    var availabilityByDate = accomodationAvailablities.FirstOrDefault(s => s.Date.Date == i.Date.Date);
                    if (availabilityByDate != null && availabilityByDate.Status == "A")
                    {
                        model.Price += availabilityByDate.Price;
                        if (model.MaxAdditionalPerson > 0)
                        {
                            model.Price = model.Price + (model.MaxAdditionalPerson * availabilityByDate.AddPersonPrice);
                        }

                    }
                    else
                    {
                        if (availabilityByDate == null)
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.NotReady
                            });
                        else if (availabilityByDate.Status == "B")
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.AlreadyBooked
                            });
                        else if (availabilityByDate.Status == "N")
                            model.DateWiseReasons.Add(new DateWiseReasons()
                            {
                                Date = i,
                                Reason = ReasonType.NotAvailable
                            });
                    }
                }
            }

            response.Success = true;
            response.Data = model;
            return response;

        }


        public ResponseModel ValidateRoomPriceAndAvailability(RoomModel model)
        {
           var response= new ResponseModel();
           
            if (model.CheckInDate == null || model.CheckOutDate == null)
            {
                response.Success = false;
                response.Message = "Check-In or Check out date(s) are missing.";
                return response;
            }
            else
            {
                DateTime startDate = model.CheckInDate.Value;
                DateTime endDate = model.CheckOutDate.Value;

                List<TblRoomPrice> availabilities = db.tblRoomPrices.Where(s => s.Date >= startDate && s.Date <= endDate && s.RoomId== model.RoomId).ToList();
                TimeSpan datediff = endDate - startDate;
                if (datediff.TotalDays > 0)
                {
                    for (DateTime i = startDate.Date; i < endDate; i = i.AddDays(1))
                    {
                       TblRoomPrice availability = availabilities.FirstOrDefault(s => s.Date == i.Date);
                        if (availability == null)
                        {
                            response.Success = false;
                            response.Message = i.ToString("dd-MMM-yyyy")+" is not available for booking.";
                            return response;
                        }
                        else if (availability.Status != "A")
                        {
                            response.Success = false;
                            response.Message = i.ToString("dd-MMM-yyyy") + " is no longer available for booking.";
                            return response;
                          
                        }
                        else
                        {
                            response.Success = true;
                            response.Message = string.Empty;
                        }
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Check out date must be greater than check-in date.";
                    return response;
                }
            }
          
            return response;
        }


    }
}
