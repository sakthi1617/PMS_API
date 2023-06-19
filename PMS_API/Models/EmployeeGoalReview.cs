using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class EmployeeGoalReview
    {
        public EmployeeGoalReview()
        {
            EmployeeAttachments = new HashSet<EmployeeAttachment>();
            ManagerGoalReviews = new HashSet<ManagerGoalReview>();
        }

        public int EmpReviewId { get; set; }
        public int? EmployeeId { get; set; }
        public int? AssingedManagerId { get; set; }
        public int? GoalId { get; set; }
        public string? EmpReview { get; set; }
        public decimal? GoalRating { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsCalculated { get; set; }

        public virtual EmployeeModule? AssingedManager { get; set; }
        public virtual EmployeeModule? Employee { get; set; }
        public virtual GoalModule? Goal { get; set; }
        public virtual ICollection<EmployeeAttachment> EmployeeAttachments { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviews { get; set; }
    }
}
