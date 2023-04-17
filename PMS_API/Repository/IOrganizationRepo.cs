using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using PMS_API.Models;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Net.Mail;

namespace PMS_API.Repository
{
    public interface IOrganizationRepo
    {
        public string? AddEmployee(EmployeeVM model);       
        public string AddUserLevel(string employeeidentity, EmployeeVM employee);
        public void AddDepartment(DepartmentVM model);   
        public void AddDesignations(Designation1VM model);    
        public string UpdateEmployee(string EmployeeIdentity, EmployeeVM model);
        public string UpdateDepertment(int id , DepartmentVM department);
        public string UpdateDesignation(int id , DesignationVM designation);      
       // public List<EmployeeModule> EmployeeList();        
        public dynamic EmployeeList();        
        public dynamic EmployeeHierachy(int employeeId);        
        public EmployeeModule EmployeeById(string EmployeeIdentity);
        public List<EmployeeModule> EmployeeByDepartment(int id);
        public List<Department> DepartmentModule();       
        public List<Designation> DesignationModule();
        public string DeleteEmployee(string EmployeeIdentity); 
        public void EmailDelivery();     
        public dynamic FindRequiredEmployee(FindEmployee find);  
        public string AddDeveloper(Developer developer);
        public string AddTester(Tester tester);

        public List<Developer> GetDevelpoer();
        public List<Tester> GetTester();
        public void Save();

        //  public string AddUserLevel(int? designationId, int? departmentId, int? employeeId);
        //  public void AddDesignation(DesignationVM model);
        //  public string UpdateLevelForEmployee(UserLevelVM level);
        //  public string DeleteSkillbyEmp(string EmployeeIdentity, int SkillId);
        //  public string UpdateSkillWeightage(WeightageVM weightage);
        // public void AddSkillWeightage(WeightageVM weightage);  
    }
}
