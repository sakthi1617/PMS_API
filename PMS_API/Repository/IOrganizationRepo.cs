using PMS_API.Models;
using PMS_API.SupportModel;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IOrganizationRepo
    {
        public int? AddEmployee(EmployeeVM model);
        public string AddUserLevel(int? designationId, int? departmentId, int? employeeId);
        public void AddDepartment(DepartmentVM model);
        public void AddDesignation(DesignationVM model);
        public void AddSkill(SkillsVM model);
        public string AddAdditionalSkills(UserLevelVM level);
        public string UpdateEmployee(int id , EmployeeVM model);
        public string UpdateDepertment(int id , DepartmentVM department);
        public string UpdateDesignation(int id , DesignationVM designation);
        public string UpdateSkill(int id , SkillsVM skill);
        public string UpdateSkillWeightage(WeightageVM weightage);
        public List<EmployeeModule> EmployeeList();
        public IQueryable<GetEmployeeSkillsByIdVM> GetEmployeeSkillsById(int id);
        public EmployeeModule EmployeeById(int id);
        public List<EmployeeModule> EmployeeByDepartment(int id);
        public List<Department> DepartmentModule();
        public List<Skill> SkilsList();
        public List<Weightage> SkillbyDepartmentID(int id);
        public void AddSkillWeightage(WeightageVM weightage);
        public List<Designation> DesignationModule();
        public string DeleteEmployee(int EmployeeId);
        public string ReqForUpdateLvl(UserLevelVM level);
        public string UpdateLevelForEmployee(UserLevelVM level);
        public string DeleteSkillbyEmp(int EmployeeId, int SkillId);
        public dynamic FindRequiredEmployee(FindEmployee find);
        public void Save();
    }
}
