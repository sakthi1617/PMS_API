namespace PMS_API.ViewModels
{
    public class EmployeeReviewVM
    {
        public int? EmpReviewId { get; set; }   
        public int? EmployeeId { get; set; }
        public int? GoalId { get; set; }
        public string? EmpReview { get; set; }
        public IFormFileCollection? Attachment { get; set; }
        public int? GoalRating { get; set; }


    }

    public class updateEmployeeReviewVM
    {
        public int? EmployeeId { get; set; }
        public int GoalId { get; set; }
        public string? EmpReview { get; set; }
        public IFormFileCollection? Attachment { get; set; }
        public int? GoalRating { get; set; }
    }
}
