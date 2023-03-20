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

        public void Save();
    }
}
