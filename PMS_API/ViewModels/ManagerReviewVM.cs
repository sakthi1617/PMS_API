namespace PMS_API.ViewModels
{
    public class ManagerReviewVM
    {
       
        public int? EmpReviewId { get; set; }
        public string? ManagerReview { get; set; }

        public IFormFileCollection? Attachment { get; set; }
        public int? GoalRating { get; set; }
    }
  
}
