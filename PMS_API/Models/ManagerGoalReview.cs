using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class ManagerGoalReview
    {
        public ManagerGoalReview()
        {
            ManangerAttachments = new HashSet<ManangerAttachment>();
        }

        public int ManagerReviewId { get; set; }
        public int? EmployeeId { get; set; }
        public int? AssingedManagerId { get; set; }
        public int? GoalId { get; set; }
        public string? ManagerReview { get; set; }
        public decimal? GoalRating { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? EmpReviewId { get; set; }
        public bool? IsCalculated { get; set; }

        public virtual EmployeeModule? AssingedManager { get; set; }
        public virtual EmployeeGoalReview? EmpReview { get; set; }
        public virtual EmployeeModule? Employee { get; set; }
        public virtual GoalModule? Goal { get; set; }
        public virtual ICollection<ManangerAttachment> ManangerAttachments { get; set; }
    }
}
