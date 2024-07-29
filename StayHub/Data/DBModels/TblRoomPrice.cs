using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace StayHub.Data.DBModels
{
    public class TblRoomPrice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public long RoomId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        [StringLength(10)]
        [Required]
        public string Status { get; set; }
        public decimal AddPersonPrice { get; set; }
        [ForeignKey(nameof(RoomId))]
        public TblRoom TblRoom { get; set; }
    }
}
