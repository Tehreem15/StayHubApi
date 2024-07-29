namespace StayHub.Data.ResponseModel
{
    public class ApiListResponse<T>
    {
        public List<T> List { get; set; }= new List<T>();
        public List<int> kList { get; set; } = new List<int>();
        public  bool Success { get; set; } = false;
        public  string Message { get; set; }
        
    }

    //LIST<INT>
    //lIAT<STRING>
    //LIST<TBLUSERS>

    //PUBLIC CLASS {

}
