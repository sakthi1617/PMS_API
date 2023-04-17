using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_API.Models;
using PMS_API.ViewModels;
using PMS_API.Models;

namespace PMS_API.Repository
{
    public interface IGoalRepo
    {
        public string AddGoalForEmployee(GoalVM model);
        public string UpdateGoalForEmployee(string EmployeeIdentity, int goalid, GoalVM model);
        //public string DeleteGoalForEmployee(string EmployeeIdentity, int goalid);
        public List<GoalVM> GetGoalbyEmpId(string EmployeeIdentity);
        public string SendMailToEmployee(GoalVM model);
        public string EmployeeGoalReview(EmployeeReviewVM model);
        public string UpdateEmployeeGoalReview(updateEmployeeReviewVM model);
        public string UpdateManagerGoalReview(ManagerReviewVM model);
        public string EmployeeExtentionRequest(string EmployeeIdentity, int GoalID);
        public string ManagerExtentionRequest(string EmployeeIdentity, int GoalID);
        public string AdminReqApproved(bool Approved, int delayedGoalId);
        public string EmpExtReqApprove(bool approved, int GoalId);
        public string ManagerExtReqApprove(bool approved, int GoalId);
        public string ManagerGoalReview(ManagerReviewVM model);
        public void GoalRatingCalculation(string EmployeeIdentity);
        public void ExtentionUpdate();
        public void sendMailtoAdmin();
        public void AssignToEmployee();
        public void Frezze();             
        public void Save();
    }
}
