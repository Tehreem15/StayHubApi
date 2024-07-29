using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.DBModels
{
    public class TblStaffActivity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long StaffId { get; set; }
        [Required]
        public string ActivityName { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; }
        public string? ActivityDescription  { get; set; }

        [ForeignKey(nameof(StaffId))]
        public TblUser TblUser { get; set; } // Assuming staff is also stored in TblUser with Role = "STAFF"
      
    }
}
