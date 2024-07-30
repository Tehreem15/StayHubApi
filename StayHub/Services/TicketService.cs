using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace StayHub.Services
{
    public class TicketService
    {
        private readonly StayHubContext db;
        private readonly IHttpContextAccessor contextAccessor;
        public TicketService(StayHubContext _db, IHttpContextAccessor _contextAccessor)
        {
            db = _db;
            contextAccessor = _contextAccessor;
        }

        public ResponseModel<GetFestivalTicketByTicketNumber> GetTicketByTicketNumber(string TicketNumber)
        {
            ResponseModel<GetFestivalTicketByTicketNumber> response = new ResponseModel<GetFestivalTicketByTicketNumber>();
            GetFestivalTicketByTicketNumber model = new GetFestivalTicketByTicketNumber();
            try
            {
                var Ticket = db.tblBookingEventTickets.Where(t => t.TicketNumber == TicketNumber).FirstOrDefault();
                if (Ticket != null)
                {
                    model.TicketNumber = Ticket.TicketNumber;
                    model.Ticket = Ticket.Ticket;
                    model.TicketType = Ticket.TicketType;
                    model.ScannedTime = Ticket.ScannedTime;
                    model.Status = Ticket.Status;
                    model.Id = Ticket.Id;
                    var BookingEvent = db.tblBookingEvents.Where(e => e.Id == Ticket.BookingEventId).FirstOrDefault();
                    if (BookingEvent != null)
                    {
                        model.BookingEventId = BookingEvent.Id;
                        var TblEvent = db.tblEvents.Where(e => e.Id == BookingEvent.EventId).FirstOrDefault();
                        if (TblEvent != null)
                        {
                            model.EventDate = TblEvent.EventDate;
                            model.EventName = TblEvent.Name;
                            model.StartTime = TblEvent.StartTime;
                            model.EndTime = TblEvent.EndTime;
                        }
                        long GuestId = db.tblBookings.Where(s => s.Id == BookingEvent.BookingId).Select(s => s.GuestId).FirstOrDefault();
                        TblUser guest = db.tblUsers.Where(s => s.Role == "GUEST" && s.Id == GuestId).FirstOrDefault();
                        if (guest != null)
                        {
                            model.Name = guest.FirstName + " " + guest.LastName;
                        }
                    }
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public ResponseModel UpdateTicketStatus(long ID, string Status)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                TblBookingEventTicket Ticket = db.tblBookingEventTickets.Where(x => x.Id == ID).FirstOrDefault();
                if (Ticket != null)
                {
                    Ticket.Status = Status;
                    Ticket.ScannedTime = Status == "Scanned" ? DateTime.Now.ToString() : "";
                    db.tblBookingEventTickets.Update(Ticket);
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
     
    }
}
