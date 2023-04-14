namespace PMS_API.Models
{
    public partial class ManagersTbl
    {
        public ManagersTbl()
        {
            EmployeeGoalReviews = new HashSet<EmployeeGoalReview>();
            EmployeeModuleFirstLevelReportingManagerNavigations = new HashSet<EmployeeModule>();
            EmployeeModuleSecondLevelReportingManagerNavigations = new HashSet<EmployeeModule>();
            GoalModules = new HashSet<GoalModule>();
            ManagerGoalReviews = new HashSet<ManagerGoalReview>();
            RequestForApproveds = new HashSet<RequestForApproved>();
            DelayedGoals = new HashSet<DelayedGoal>();
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
        public virtual ICollection<EmployeeGoalReview> EmployeeGoalReviews { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModuleFirstLevelReportingManagerNavigations { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModuleSecondLevelReportingManagerNavigations { get; set; }
        public virtual ICollection<GoalModule> GoalModules { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviews { get; set; }
        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }
        public virtual ICollection<DelayedGoal> DelayedGoals { get; set; }

    }
}
