using PMS_API.Models;

namespace PMS_API.Models
{
    public partial class GoalModule
    {
        public GoalModule()
        {
            EmployeeGoalReviews = new HashSet<EmployeeGoalReview>();
            ManagerGoalReviews = new HashSet<ManagerGoalReview>();
        }

        public int GoalId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Goalname { get; set; }
        public string? GoalDescription { get; set; }
        public string? Assignedby { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public int? Progress { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Modifyby { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? AssingedManagerId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsGoalNotified { get; set; }
        public bool? IsReviewNotified { get; set; }
        public bool? IsFreezedEmp { get; set; }
        public bool? IsFreezedManager { get; set; }
        public bool? IsEmpExtentionRequested { get; set; }
        public bool? IsManagerExtentionRequested { get; set; }
        public bool? IsEmpExtentionApproved { get; set; }
        public DateTime? IsEmpExtentionApprovedAt { get; set; }
        public bool? IsManagerExtentionApproved { get; set; }
        public DateTime? IsManagerExtentionApprovedAt { get; set; }
        public bool? IsIgnored { get; set; }

        public virtual EmployeeModule? AssingedManager { get; set; }
        public virtual EmployeeModule? Employee { get; set; }
        public virtual ICollection<EmployeeGoalReview> EmployeeGoalReviews { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviews { get; set; }
    }
}
