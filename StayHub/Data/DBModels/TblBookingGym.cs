using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblBookingGym
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public TblBooking TblBooking { get; set; }
        public long GymId { get; set; }
        
        [ForeignKey(nameof(GymId))]
        public TblGym TblGym { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
        public decimal Price { get; set; }
    }

    //GYM 1
    // 8:00AM 10:00AM   OPENING CLOSING TIME

    // GYM 2
    // 2:00 PM 4:00PM

    
    //FOR CAPACITY CHECK 
    // BY OPENING AND CLOSING TIME AND CHECK USER MONTHID 
    //CAPACITY 200 MONTH JAN-MAR GYM1 

}
