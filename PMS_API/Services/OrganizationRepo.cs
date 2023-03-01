using Microsoft.AspNetCore.Mvc;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.ViewModels;

namespace PMS_API.Services
{

    public class OrganizationRepo:IOrganizationRepo
    {

        private readonly PMSContext _context;
        public OrganizationRepo(PMSContext context)
        {
            _context = context;
        }

        public void AddEmployee(EmployeeVM model)
        {
            EmployeeModule module= new EmployeeModule();
            module.EmployeeName = model.EmployeeName;
            module.DesignationId = model.DesignationId;
            module.DateOfJoining = model.DateOfJoining;
            module.DateOfBirth = model.DateOfBirth;
            module.MobileNumber = model.MobileNumber;
            module.AlternateNumber = model.AlternateNumber;
            module.EmailId = model.EmailId;
            module.SkypeId = model.SkypeId;
            module.OverallExperiance = model.OverallExperiance;
            module.WorkingLocation = model.WorkingLocation;
            module.NativeLocation = model.NativeLocation;
            module.FirstLevelReportingManager = model.FirstLevelReportingManager;
            module.SecondLevelReportingManager = model.SecondLevelReportingManager;
            module.ProfilePicture = model.ProfilePicture;
           
            _context.EmployeeModules.Add(module);
        }

        public void AddSkill(SkillsVM model)
        {
            Skillset skillset= new Skillset();

            skillset.DesignationId= model.DesignationId;    
            skillset.Skills= model.Skills;  
            _context.Skillsets.Add(skillset);
        }

        public List<EmployeeModule> EmployeeList()
        {
            return _context.EmployeeModules.ToList();   
            
        }

        
        public List<Skillset> SkilsList()
        {
           return _context.Skillsets.ToList();
        }

        public List<Skillset> SkillbyID(int id)
        {
           return _context.Skillsets.Where(x=>x.DesignationId==id).ToList();
        }

        public List<EmployeeModule> EmployeeByDesignation(int id)
        {
            return _context.EmployeeModules.Where(X=>X.DesignationId==id).ToList();
        }

        public void AddDesignation(DepartmentVM model)
        {
            Department department= new Department();    

            department.DesignationName= model.DesignationName;
            _context.Departments.Add(department);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
