using StayHub.Data.DBModels;
using StayHub.Data.ViewModels;
using static Azure.Core.HttpHeader;
using System.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using System.Text;
using StayHub.Data.ResponseModel;

namespace StayHub.Services
{

    public class CartService
    {
        private StayHubContext db;
        public CartService(StayHubContext _db)
        {
            db = _db;
        }


        public bool AddBooking(TblBooking booking)
        {
            try
            {

                _ = db.tblBookings.Add(booking);
                bool result = db.SaveChanges() > 0;

                if (result)
                {
                    foreach (TblBookingEvent eventDetail in booking.TblBookingEvents)
                    {
                        _ = UpdateTicketNumber(eventDetail.Id);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
               
                // Check if there is an inner exception with more details
                if (ex.InnerException != null)
                {
                  
                
                }

                return false;
            }
        }


        private bool CancelTickets(long bookingEventID)
        {
            var tickets= db.tblBookingEventTickets.Where(b=>b.BookingEventId == bookingEventID).ToList();
            if (tickets != null && tickets.Count()>0)
            {
                foreach (var ticket in tickets) 
                {
                    ticket.Status = "Cancelled";
                    db.tblBookingEventTickets.Update(ticket);
                }
              
                return db.SaveChanges() > 0;
            }
            return false;
        }
        private bool UpdateTicketNumber(long bookingEventID)
        {
            var tickets = db.tblBookingEventTickets.Where(b => b.BookingEventId == bookingEventID).ToList();
            if (tickets != null && tickets.Count() > 0)
            {
                foreach (var ticket in tickets)
                {
                    ticket.TicketNumber = ticket.Id.ToString("D5");
                    db.tblBookingEventTickets.Update(ticket);
                }

                return db.SaveChanges() > 0;
            }
            return false;

        }
        public void DetachAllEntities()
        {
            var undetachedEntriesCopy = db.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in undetachedEntriesCopy)
                entry.State = EntityState.Detached;
        }
        public async Task UpdateBookingStatus(List<RoomModel> model)
        {

            foreach (RoomModel item in model)
            {
                List<TblRoomPrice> availabilities =db.tblRoomPrices.Where(s => s.RoomId == item.RoomId && s.Date >= item.CheckInDate && s.Date < item.CheckOutDate && s.Status == "A").ToList();
                if (availabilities.Count > 0 && availabilities.Count()>0)
                {
                    foreach(var r in availabilities)
                    {
                        r.Status = "B";
                        db.tblRoomPrices.Update(r);
                    }
                  
                    _ = db.SaveChanges();
                }
            }


        }
        public async Task<bool> UpdateBooking(TblBooking booking)
        {
            try
            {
                db.tblBookings.Update(booking);

               
                if (booking.TblBookingEvents != null && booking.TblBookingEvents.Count > 0)
                {
                    //update festival list
                    foreach (TblBookingEvent item in booking.TblBookingEvents)
                    {
                        _ = db.tblBookingEvents.Attach(item);
                        db.Entry(item).State = EntityState.Modified;
                    }
                }

                return db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return false;
            }

        }

        public string GetEventDateTime(long? eventId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            TblEvent tblEvent =db.tblEvents.Where(e => e.Id == eventId).FirstOrDefault();
            DateTime Date = (DateTime)tblEvent.EventDate;
            string eventdate = Date.ToString("dd MMMM yyyy").TrimStart(new Char[] { '0' });
            TimeSpan workingHoursFrom = (TimeSpan)tblEvent.StartTime;
            TimeSpan workingHoursTo = (TimeSpan)tblEvent.EndTime;
            DateTime FromTime = new DateTime(1, 1, 1, workingHoursFrom.Hours, workingHoursFrom.Minutes, 0);
            DateTime ToTime = new DateTime(1, 1, 1, workingHoursTo.Hours, workingHoursTo.Minutes, 0);


            _ = stringBuilder.Append(Date.ToString("ddd") + " " + eventdate + " " + FromTime.ToString("hh:mmtt") + "-" + ToTime.ToString("hh:mmtt"));

            return stringBuilder.ToString();
            //Sat 11 March 2023 11:45am-8:30pm
            //Sun 5 March 2023 11:45am-8:30pm

        }
        public ApiBaseResponse ValidateRoom(ValidationRequest request)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            if (request.Type == ResourceType.Accomodation)
            {
                try
                {
                    foreach (ValidationRequestData item in request.Data)
                    {
                        TblRoom room =db.tblRooms.Where(s=>s.Id==item.RoomId).FirstOrDefault();
                        List<TblRoomPrice> availabilities = db.tblRoomPrices
                            .Where(s => s.RoomId == item.RoomId && s.Date >= item.StartDate && s.Date <= item.EndDate)
                            .ToList();
                        double difference = (item.EndDate - item.StartDate).TotalDays;
                        if (difference > 0)
                        {
                            IEnumerable<TblRoomPrice> unavailableDates = availabilities.Where(s => s.Status == "A");
                            if (unavailableDates.Any())
                            {
                                difference = (unavailableDates.Max(s => s.Date) - unavailableDates.Min(s => s.Date)).TotalDays;
                                if (difference == item.NoOfNights)
                                {
                                    decimal? pricing = availabilities.Where(s => s.Status == "A").Sum(s => s.Price + (s.AddPersonPrice * item.AdditionalPersons));
                                   
                                    if (pricing == item.TotalPrice)
                                    {
                                        response.Success = true;
                                        response.Message = "Booking is available.";
                                    }
                                    else
                                    {
                                        response.Success = false;
                                        response.Message = "Pricing information is changed since you last added rooms in cart.";
                                    }
                                }
                                else
                                {
                                    response.Success = false;
                                    response.Message = "Booking information is changed since you last added rooms in cart.";
                                }
                            }
                            else
                            {
                                response.Success= false;
                                response.Message = "Booking information is changed since you last added rooms in cart.";
                            }
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Cannot book for same date.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Internal server error.";
                }
            }
            else
            {
                response.Success = true;
                response.Message = "Validated.";
            }
            return response;
        }


    }
}
