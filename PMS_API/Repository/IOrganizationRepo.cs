using PMS_API.Models;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IOrganizationRepo
    {
        public string AddEmployee(EmployeeVM model);
        public void AddSkill(SkillsVM model);
        public string UpdateEmployee(int id , EmployeeVM model);
        public List<EmployeeModule> EmployeeList();
        public List<Skillset> SkilsList();
        public List<Skillset> SkillbyID(int id);
        //public IEnumerable<EmployeeModule> ShowEmployeelist();


        public void AddDesignation(DepartmentVM model);
        public List<EmployeeModule> EmployeeByDesignation(int id);

        public void Save();
    }
}
