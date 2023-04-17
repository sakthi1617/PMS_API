using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class DelayedGoal
    {
        public int DelayedGoalId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Goalname { get; set; }
        public string? GoalDescription { get; set; }
        public string? Assignedby { get; set; }
        public int? AssingedManagerId { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Priority { get; set; }
        public int? Progress { get; set; }
        public bool? IsNotified { get; set; }
        public bool? IsAdminApproved { get; set; }
        public DateTime? AdminApprovedAt { get; set; }
        public bool? IsAssignedtoEmployee { get; set; }

        public virtual ManagersTbl? AssingedManager { get; set; }
        public virtual EmployeeModule? Employee { get; set; }
    }
}
