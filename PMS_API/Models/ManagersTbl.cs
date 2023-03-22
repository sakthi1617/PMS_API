namespace PMS_API.Models
{
    public partial class ManagersTbl
    {
        public ManagersTbl()
        {
            EmployeeModuleFirstLevelReportingManagerNavigations = new HashSet<EmployeeModule>();
            EmployeeModuleSecondLevelReportingManagerNavigations = new HashSet<EmployeeModule>();
            RequestForApproveds = new HashSet<RequestForApproved>();
        }

        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActivated { get; set; }
        public int? EmployeeId { get; set; }

        public virtual EmployeeModule? Employee { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModuleFirstLevelReportingManagerNavigations { get; set; }
        public virtual ICollection<EmployeeModule> EmployeeModuleSecondLevelReportingManagerNavigations { get; set; }
        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }

    }
}
