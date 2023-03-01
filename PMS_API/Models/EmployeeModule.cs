using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class EmployeeModule
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }
        public string? AlternateNumber { get; set; }
        public string? EmailId { get; set; }
        public string? SkypeId { get; set; }
        public int? OverallExperiance { get; set; }
        public string? WorkingLocation { get; set; }
        public string? NativeLocation { get; set; }
        public string? FirstLevelReportingManager { get; set; }
        public string? SecondLevelReportingManager { get; set; }
        public string? ProfilePicture { get; set; }

        public virtual Department? Designation { get; set; }
    }
}
