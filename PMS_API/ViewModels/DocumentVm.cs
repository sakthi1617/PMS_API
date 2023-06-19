namespace PMS_API.ViewModels
{
    public class DocumentVm
    {
        public int DocumentId { get; set; }
       
        public string DocumentURL { get; set; }
        public int EmployeeId { get; set; }
        public int? QuestionId { get; set; }
    }
}
