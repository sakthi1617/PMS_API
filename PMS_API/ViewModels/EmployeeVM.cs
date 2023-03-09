using PMS_API.Models;

namespace PMS_API.ViewModels
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public decimal? PriviousExperience { get; set; }

        public string? FirstLevelReportingManager { get; set; }
        public string? SecondLevelReportingManager { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }

        public string? ProfilePicture { get; set; }

    }
}
