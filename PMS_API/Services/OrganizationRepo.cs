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

        public void AddSkill(Skillset model)
        {
            _context.Skillsets.Add(model);
        }

        public List<EmployeeModule> EmployeeList()
        {
            throw new NotImplementedException();
        }

        
        public List<Skillset> SkilsList()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
