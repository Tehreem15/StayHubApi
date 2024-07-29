// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public partial class TblRoomImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public long RoomId { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
        [ForeignKey(nameof(RoomId))]
        public TblRoom TblRoom { get; set; }
    }
}
