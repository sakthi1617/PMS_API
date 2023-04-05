using Microsoft.AspNetCore.Mvc;
using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IGoalRepo
    {
        public string AddGoalForEmployee(GoalVM model);
        public string UpdateGoalForEmployee(int empid, int goalid, GoalVM model);
        public string DeleteGoalForEmployee(int empid, int goalid);
        public List<GoalVM> GetGoalbyEmpId(int empid);
        public string SendMailToEmployee(GoalVM model);
        public string EmployeeGoalReview(EmployeeReviewVM model);
        public string UpdateEmployeeGoalReview(updateEmployeeReviewVM model);
        public string UpdateManagerGoalReview(ManagerReviewVM model);
        public string ExtentionRequest(int EmployeeID, int GoalID);
        public string ManagerGoalReview(ManagerReviewVM model);
        public void GoalRatingCalculation(int EmpID);
        public void Save();
    }
}
