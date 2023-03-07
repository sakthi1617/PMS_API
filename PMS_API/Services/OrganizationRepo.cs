using Microsoft.AspNetCore.Mvc;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.ViewModels;

namespace PMS_API.Services
{

    public class OrganizationRepo : IOrganizationRepo
    {

        private readonly PMSContext _context;
        public OrganizationRepo(PMSContext context)
        {
            _context = context;
        }

        public string AddEmployee(EmployeeVM model)
        {
           
            EmployeeModule module = new EmployeeModule();

            var existingUser = _context.EmployeeModules.FirstOrDefault(x => x.EmailId== model.EmailId);
            if(existingUser == null)
            {
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
                module.CreatedOn = DateTime.Now;
                module.IsActivated = false;

                _context.EmployeeModules.Add(module);
            }
            else
            {
                return "User Already Exists";
            }

            return "Created";
        }

        public void AddSkill(SkillsVM model)
        {
            Skillset skillset = new Skillset();

            skillset.DesignationId = model.DesignationId;
            skillset.Skills = model.Skills;
            _context.Skillsets.Add(skillset);
        }

        public void AddDesignation(DepartmentVM model)
        {
            Department department = new Department();

            department.DesignationName = model.DesignationName;
            _context.Departments.Add(department);
        }


        public string UpdateEmployee(int id, EmployeeVM model)
        {
            try
            {
                var Emp = _context.EmployeeModules.Where(s => s.EmployeeId == id).FirstOrDefault();
                if (Emp != null)
                {
                    Emp.EmployeeName = model.EmployeeName;
                    Emp.DesignationId = model.DesignationId;
                    Emp.DateOfJoining = model.DateOfJoining;
                    Emp.DateOfBirth = model.DateOfBirth;
                    Emp.MobileNumber = model.MobileNumber;
                    Emp.AlternateNumber = model.AlternateNumber;
                    Emp.EmailId = model.EmailId;
                    Emp.SkypeId = model.SkypeId;
                    Emp.OverallExperiance = model.OverallExperiance;
                    Emp.WorkingLocation = model.WorkingLocation;
                    Emp.NativeLocation = model.NativeLocation;
                    Emp.FirstLevelReportingManager = model.FirstLevelReportingManager;
                    Emp.SecondLevelReportingManager = model.SecondLevelReportingManager;
                    Emp.ProfilePicture = model.ProfilePicture;
                    Emp.UpdatedOn = DateTime.Now;

                    _context.EmployeeModules.Update(Emp);
                    _context.SaveChanges();


                    return "Updated";
                }
                else
                {
                    return "User Not Exists";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


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
            return _context.Skillsets.Where(x => x.DesignationId == id).ToList();
        }

        public List<EmployeeModule> EmployeeByDesignation(int id)
        {
            return _context.EmployeeModules.Where(X => X.DesignationId == id).ToList();
        }


        //public IEnumerable<EmployeeModule> ShowEmployeelist()
        //{
        //    var groupedResult = from s in _context.EmployeeModules
        //                        group s by new { s.FirstLevelReportingManager, s.SecondLevelReportingManager } into abc
        //                        select new EmployeeModule()
        //                        {
        //                            FirstLevelReportingManager = abc.Key.FirstLevelReportingManager,
        //                            SecondLevelReportingManager = abc.Key.SecondLevelReportingManager,
        //                            employeeModules = abc.ToList()
        //                        };

        //    return groupedResult;
        //}


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
