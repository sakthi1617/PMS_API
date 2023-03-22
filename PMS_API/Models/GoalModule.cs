using PMS_API.Models;

namespace PMS_API.Models
{
    public class GoalModule
    {
        public int GoalId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Goalname { get; set; }
        public string? GoalDescription { get; set; }
        public string? Assignedby { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public int? Progress { get; set; }
        public bool? IsDeleted { get; set; }

        public string? Modifyby { get; set; }

        public DateTime? ModifyAt { get; set; }

        public virtual EmployeeModule? Employee { get; set; }
    }
}
