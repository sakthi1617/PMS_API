using PMS_API.Models;
using System.ComponentModel.DataAnnotations;

namespace PMS_API.ViewModels
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }
        [Required]
        public string? EmployeeIdentity { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public int? DepartmentId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public int? DesignationId { get; set; }
        [Required]
        public int? RoleId { get; set; }
        [Required]
        public DateTime? DateOfJoining { get; set; }
        [Required]
        public decimal? PriviousExperience { get; set; }
        [Required]
        public string FirstLevelReportingManager { get; set; }
        // public int? SecondLevelReportingManager { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public int? Age { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        [Required]
        public string? WorkPhoneNumber { get; set; }
        public string? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }
        public decimal? Salary { get; set; }
        public string? ProfilePicture { get; set; }

     // public  List<UserSkillsVM> skills { get;set; }

    }
}
