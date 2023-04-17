namespace PMS_API.Reponse
{
    public class SuccessResponse<T>
    {
        public string? Error { get; set; }
        public bool IsreponseSuccess { get; set; } = false;
        public T? ModelData { get; set; }
        public T? items { get; set; }
        public IList<T>? Listdata { get; set; }
        public object? otherData { get; set; }
        public string? Response { get; set; }
        public string? statusCode { get; set; }
    }
}
