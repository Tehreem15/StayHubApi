using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class StaffViewModel
    {
        public long Id { get; set; }
    
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
       
        public string? Password { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } 

        public bool IsAdmin { get; set; }   
    
    }
}
