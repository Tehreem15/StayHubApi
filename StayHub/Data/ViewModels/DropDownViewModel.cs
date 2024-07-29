namespace StayHub.Data.ViewModels
{
    public class DropDownViewModel<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }

    public class KeyValueModel
    {
        public long Key { get; set; }

        public string Value { get; set; }
    }
}
