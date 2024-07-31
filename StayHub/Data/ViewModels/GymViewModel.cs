using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class GymViewModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int Gender { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Fee { get; set; }
        public int Capacity { get; set; }
        public string? Rules { get; set; }
        public string Equiqment { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
    }

}
