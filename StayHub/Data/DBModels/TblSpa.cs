using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace StayHub.Data.DBModels
{
    public class TblSpa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } //Room //    
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public  ICollection<TblBookingSpa> TblBookingSpas { get; set; } = new HashSet<TblBookingSpa>();

    }
}
