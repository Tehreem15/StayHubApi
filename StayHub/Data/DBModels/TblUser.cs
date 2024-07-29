using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayHub.Data.DBModels
{
    public class TblUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(10)]
        public string? Zipcode { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ImgPath { get; set; }
        public string? GuestNumber { get; set; }
        [StringLength(100)]
        public string? Country { get; set; }
        [StringLength(10)]
        [Required]
        public string Role { get; set; } = ""; //ADMIN //GUEST//STAFF

        public  ICollection<TblBooking> TblBookings { get; set; } = new HashSet<TblBooking>();
        public  ICollection<TblStaffActivity> TblStaffActivities { get; set; } = new HashSet<TblStaffActivity>();
    }
}
