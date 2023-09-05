namespace Da7ee7_Academy.Helper
{
    public class BaseResponse
    {
        public string Message { get; set; } = "One or more errors occur.";
        public bool IsSuccess { get; set; } = false;
        public int StatusCode { get; set; } = 200;
        public List<string> Errors { get; set; } = new();
    }

    public class ResponseModel<T>: BaseResponse
    {
        public T Result { get; set; }
    }
    public class ResponseModel : BaseResponse 
    {
        public string Result { get; } = null;
    }
}
