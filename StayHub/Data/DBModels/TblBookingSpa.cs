using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblBookingSpa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public TblBooking TblBooking { get; set; }
        public long SpaId { get; set; }

        [ForeignKey(nameof(SpaId))]
        public TblSpa TblSpa { get; set; }
        public int NoOfPersons { get; set; }
        public decimal PerPersonPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SpaDate { get; set; }  
        //SESSION COVER NO OF DATES IN A MONTH 

        //DateTime cover
        //SPA 1
        // 8:00AM 10:00AM   OPENING CLOSING TIME

        // GYM 2
        // 2:00 PM 4:00PM


        //FOR CAPACITY CHECK 
        // BY OPENING AND CLOSING TIME AND CHECK USER MONTHID 
        //CAPACITY 200 MONTH JAN-MAR GYM1 
    }
}
