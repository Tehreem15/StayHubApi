using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class StaffActivityViewModel
    {
        public long Id { get; set; }

        [Required]
        public long StaffId { get; set; }
        [Required]
        public string ActivityName { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; }
        public string? ActivityDescription { get; set; }
    }
}
