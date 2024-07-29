using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace StayHub.Data.DBModels
{
    public partial class TblRoom
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        [StringLength(50)]
        [Required]  
        public string Type { get; set; } //Single //DOuble//Suite
        
        //no of adust //no of chuld 

        [StringLength(200)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public int MaxAdditionalPerson { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public  ICollection<TblRoomPrice> TblRoomPrices { get; set; } =  new HashSet<TblRoomPrice>();
        public  ICollection<TblRoomImage> TblRoomImages { get; set; } = new HashSet<TblRoomImage>();
        public  ICollection<TblBookingRoom> TblBookingRooms { get; set; } = new HashSet<TblBookingRoom>();
        public  ICollection<TblBookingRoomService> TblBookingRoomServices { get; set; } = new HashSet<TblBookingRoomService>();

    }
}
