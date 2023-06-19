namespace PMS_API.ViewModels
{
    public class ValidateVM
    {
        public int QuestionPaperId { get; set; }
        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public string Files { get; set; }
    }
    public class MyLists
    {
        public List<int>? Correct { get; set; }
        public List<int>? Wrong { get; set; }
    }

}
