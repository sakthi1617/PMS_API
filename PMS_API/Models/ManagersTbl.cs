namespace PMS_API.Models
{
    public partial class ManagersTbl
    {
        public ManagersTbl()
        {
            DelayedGoals = new HashSet<DelayedGoal>();
            RequestForApproveds = new HashSet<RequestForApproved>();
        }

        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActivated { get; set; }
        public int? EmployeeId { get; set; }
        public int? Reporting1Person { get; set; }
        public int? Reporting2Person { get; set; }

        public virtual EmployeeModule? Employee { get; set; }
        public virtual ICollection<DelayedGoal> DelayedGoals { get; set; }
        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }

    }
}
