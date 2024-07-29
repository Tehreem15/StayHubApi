using System.ComponentModel.DataAnnotations;

namespace StayHub.Data.ViewModels
{
    public class ValidationRequest
    {
        public ResourceType Type { get; set; }
        public List<ValidationRequestData> Data { get; set; }
    }
    public class ValidationRequestData
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int NoOfNights { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        public long RoomId { get; set; }
        public int AdditionalPersons { get; set; }
    }
    public class ValidationResponse
    {
        public bool Status { get; set; }
        public string Reason { get; set; }
    }
    public enum ResourceType
    {
        None,
        Accomodation
    }
}
