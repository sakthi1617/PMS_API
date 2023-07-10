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
            EmployeeGoalReviewAssingedManagers = new HashSet<EmployeeGoalReview>();
            EmployeeGoalReviewEmployees = new HashSet<EmployeeGoalReview>();
            GoalModuleAssingedManagers = new HashSet<GoalModule>();
            GoalModuleEmployees = new HashSet<GoalModule>();
            GoalRatings = new HashSet<GoalRating>();
            InverseFirstLevelReportingManagerNavigation = new HashSet<EmployeeModule>();
            InverseSecondLevelReportingManagerNavigation = new HashSet<EmployeeModule>();
            ManagerGoalReviewAssingedManagers = new HashSet<ManagerGoalReview>();
            ManagerGoalReviewEmployees = new HashSet<ManagerGoalReview>();
            ManagersTbls = new HashSet<ManagersTbl>();
            RequestForApproveds = new HashSet<RequestForApproved>();
            UserLevels = new HashSet<UserLevel>();
        }

        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
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
        public string? EmployeeIdentity { get; set; }
        public int? TeamId { get; set; }
        public decimal? PerformanceLevel { get; set; }
        public int? PotentialStage { get; set; }
        public int? PerformanceStage { get; set; }
        public bool? IsWantToPublish { get; set; }
        public bool? RatingIsaccepted { get; set; }
        public bool? IsPublished { get; set; }
        public bool? InNoticePeriod { get; set; }
        public bool? IsResigned { get; set; }
        public DateTime? ResingnedAt { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Designation? Designation { get; set; }
        public virtual EmployeeModule? FirstLevelReportingManagerNavigation { get; set; }
        public virtual Role? Role { get; set; }
        public virtual EmployeeModule? SecondLevelReportingManagerNavigation { get; set; }
        public virtual Team? Team { get; set; }
        public virtual ICollection<DelayedGoal> DelayedGoals { get; set; }
        public virtual ICollection<EmployeeGoalReview> EmployeeGoalReviewAssingedManagers { get; set; }
        public virtual ICollection<EmployeeGoalReview> EmployeeGoalReviewEmployees { get; set; }
        public virtual ICollection<GoalModule> GoalModuleAssingedManagers { get; set; }
        public virtual ICollection<GoalModule> GoalModuleEmployees { get; set; }
        public virtual ICollection<GoalRating> GoalRatings { get; set; }
        public virtual ICollection<EmployeeModule> InverseFirstLevelReportingManagerNavigation { get; set; }
        public virtual ICollection<EmployeeModule> InverseSecondLevelReportingManagerNavigation { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviewAssingedManagers { get; set; }
        public virtual ICollection<ManagerGoalReview> ManagerGoalReviewEmployees { get; set; }
        public virtual ICollection<ManagersTbl> ManagersTbls { get; set; }
        public virtual ICollection<RequestForApproved> RequestForApproveds { get; set; }
        public virtual ICollection<UserLevel> UserLevels { get; set; }
        
    }
}
