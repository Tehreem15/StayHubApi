namespace StayHub.Data.ResponseModel
{
    public class ApiResponse<T>
    {
        public  T Data { get; set; }
        public  bool Success { get; set; } = false;
        public  string Message { get; set; }
    
    }
}
