using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class GuestRegisterModel
    {
        public long Id { get; set; } = 0;
        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string Zipcode { get; set; }

        [StringLength(300)]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }
        public string? ImgPath { get; set; }
        
        [StringLength(100)]
        public string Country { get; set; }

        public IFormFile imageFile  { get; set; } 
}

    public class GuestViewModel
    {
        public long Id { get; set; } = 0;
        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string Zipcode { get; set; }

        [StringLength(300)]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }
        public string? ImgPath { get; set; }
        public IFormFile imageFile { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

    }


}
