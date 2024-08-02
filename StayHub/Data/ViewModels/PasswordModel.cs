using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class ChangePasswordModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required, Compare(nameof(NewPassword))]
        public string RepeatPassword { get; set; }


    }


    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }


    public class ResetPasswordModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string NewPassword { get; set; }
        [Required, Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }

    }

}
