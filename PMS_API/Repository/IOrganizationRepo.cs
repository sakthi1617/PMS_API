using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IOrganizationRepo
    {
        public void AddEmployee(EmployeeVM model);
        public void AddSkill(SkillsVM model);

        public List<EmployeeModule> EmployeeList();
        public List<Skillset> SkilsList();
        public List<Skillset> SkillbyID(int id);

        public void AddDesignation(DepartmentVM model);
        public List<EmployeeModule> EmployeeByDesignation(int id);

        public void Save();
    }
}
