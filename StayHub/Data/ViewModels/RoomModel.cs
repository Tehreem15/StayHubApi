namespace StayHub.Data.ViewModels
{
    public class RoomViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int MaxAdditionalPerson { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal? Price { get; set; }
        public List<RoomImageViewModel> ImagesUrl { get; set; }
        public IEnumerable<string> DisabledDates { get; set; }
    }
    public class RoomImageViewModel
    {
        public long Id { get; set; }
        public long? RoomId { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
    }
}
