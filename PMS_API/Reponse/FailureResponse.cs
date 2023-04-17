namespace PMS_API.Reponse
{
    public class FailureResponse<T>
    {
        public string? Error { get; set; }
        public bool IsreponseSuccess { get; set; } = false;
    }
}
