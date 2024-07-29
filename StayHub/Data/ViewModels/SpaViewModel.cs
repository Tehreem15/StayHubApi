using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class SpaViewModel
    { 
        public long Id { get; set; }
        public string Name { get; set; } //Room //    
        public string Description { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
    }
}
