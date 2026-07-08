namespace RealTimeChatApp.Api.Commons.Responses
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Meta { get; set; }
    }
}
