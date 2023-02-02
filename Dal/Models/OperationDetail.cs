namespace Dal.Models
{
    public class OperationDetail
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool IsError { get; set; }
    }
}