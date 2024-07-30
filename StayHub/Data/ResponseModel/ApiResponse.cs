namespace StayHub.Data.ResponseModel
{
    public class ResponseModel<T>
    {
        public  T Data { get; set; }
        public  bool Success { get; set; } = false;
        public  string Message { get; set; }
    
    }
}
