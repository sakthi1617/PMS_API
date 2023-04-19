using PMS_API.Models;
using System;
using System.Collections.Generic;

namespace PMS_API.Models
{
    public partial class EmployeeModule
    {
        public EmployeeModule()
        {
            DelayedGoals = new HashSet<DelayedGoal>();
            EmployeeGoalReviews = new HashSet<EmployeeGoalReview>();
            GoalModules = new HashSet<GoalModule>();
            GoalRatings = new HashSet<GoalRating>();
            ManagerGoalReviews = new HashSet<ManagerGoalReview>();
            ManagersTbls = new HashSet<ManagersTbl>();
            RequestForApproveds = new HashSet<RequestForApproved>();
            UserLevels = new HashSet<UserLevel>();
        }

        public int EmployeeId { get; set; }

        public string? EmployeeIdentity { get; set; }    
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? TeamId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public decimal? PriviousExperience { get; set; }
        public decimal? CurrentExperience { get; set; }
        public decimal? TotalExperience { get; set; }
        public int? FirstLevelReportingManager { get; set; }
        public int? SecondLevelReportingManager { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }
        public string? ProfilePicture { get; set; }
        public int? PotentialLevel { get; set; }
        public string? AddBy { get; set; }
        public DateTime? AddTime { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public bool? IsActivated { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? Salary { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Designation? Designation { get; set; }
        public virtual Designation1? DesignationNavigation { get; set; }
        public virtual ManagersTbl? FirstLevelReportingManagerNavigation { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Team? Team { get; set; }
        public virtual ManagersTbl? SecondLevelReportingManagerNavigation { get; set; }
        public virtual ICollection<DelayedGoal> DelayedGoals { get; set; }
        public virtual ICollection<EmployeeGoalReview> EmployeeGoalReviews { get; set; }
        public virtual ICollection<GoalModule> GoalModules { get; set; }
        public virtual ICollection<GoalRating> GoalRatings { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviews { get; set; }
        public virtual ICollection<ManagersTbl> ManagersTbls { get; set; }
        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
    }
}
