using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;

namespace PMS_API.Services
{
    public class OrganizationService : IOrganizationRepo
    {
        private readonly PMSContext _context;
        private readonly IEmailService _emailservice;
        public OrganizationService(PMSContext context, IEmailService emailService)
        {
            _context = context;
            _emailservice = emailService;
        }
        public int? AddEmployee(EmployeeVM model)
        {
            EmployeeModule module = new EmployeeModule();
            ManagersTbl managersTbl = new ManagersTbl();

            var existingUser = _context.EmployeeModules.FirstOrDefault(x => x.Email == model.Email);
            if (existingUser == null)
            {
                module.EmployeeIdentity = model.EmployeeIdentity;
                module.Name = model.Name;
                module.Email = model.Email;
                module.DepartmentId = model.DepartmentId;
                module.DesignationId = model.DesignationId;
                module.RoleId = model.RoleId;
                module.DateOfJoining = model.DateOfJoining;
                module.PriviousExperience = model.PriviousExperience;
               // module.FirstLevelReportingManager = model.FirstLevelReportingManager;
                module.FirstLevelReportingManager = _context.EmployeeModules.FirstOrDefault( m => m.EmployeeIdentity == model.FirstLevelReportingManager).EmployeeId;
                module.SecondLevelReportingManager = _context.EmployeeModules.FirstOrDefault(c => c.EmployeeIdentity == model.FirstLevelReportingManager).FirstLevelReportingManager;
                module.DateOfBirth = model.DateOfBirth;
                module.Age = model.Age;
                module.Gender = model.Gender;
                module.MaritalStatus = model.MaritalStatus;
                module.WorkPhoneNumber = model.WorkPhoneNumber;
                module.PersonalPhone = model.PersonalPhone;
                module.PersonalEmail = model.PersonalEmail;
                module.ProfilePicture = model.ProfilePicture;
                module.AddTime = DateTime.Now;
                module.IsDeleted = false;
                module.IsActivated = false;
                module.Salary = model.Salary;
                module.TeamId = model.TeamId;

               _context.EmployeeModules.Add(module);    
                _context.SaveChanges();

                 return module.EmployeeId;
            }
            else
            {
                return 0;
            }
        } 
        public string AddUserLevel(int? employeeId, int? DepartmentId, int? DesignationId ,int? TeamId)
        {
            if(TeamId == null)
            {
                var weightage = _context.Weightages.Where(x => x.DepartmentId.Equals(DepartmentId) && x.DesignationId.Equals(DesignationId)).ToList();

                foreach (var weight in weightage)
                {
                    UserLevel module = new UserLevel();
                    module.EmployeeId = employeeId;
                    module.SkillId = weight.SkillId;
                    module.Level = 0;
                    module.Weightage = weight.Weightage1;
                    _context.UserLevels.Add(module);
                    _context.SaveChanges();
                }
                return "Created";
            }

            var weightages = _context.Weightages.Where(x => x.DepartmentId.Equals(DepartmentId) && x.DesignationId.Equals(DesignationId) && x.TeamId.Equals(TeamId)).ToList();

            foreach (var weightage in weightages)
            {
                UserLevel module = new UserLevel();
                module.EmployeeId = employeeId;
                module.SkillId = weightage.SkillId;
                module.Level = 0;
                module.Weightage = weightage.Weightage1;
                _context.UserLevels.Add(module);
                _context.SaveChanges();
            }
            return "Created";

        }    
        public void AddDepartment(DepartmentVM model)
        {
            Department department = new Department();
            department.DepartmentName = model.DepartmentName;
            department.AddTime = DateTime.Now;
            _context.Departments.Add(department);
        } 
        public void AddDesignations(Designation1VM model)
        {
            Designation1 designation1 = new Designation1();
            designation1.DepartmentId= model.DepartmentId;
            designation1.DesignationName= model.DesignationName;
            designation1.AddTime = DateTime.Now;
            _context.Designations1.Add(designation1);
        }

        public void AddTeam(TeamVM team)
        {
            Team _team = new Team();    
            _team.DepartmentId= team.DepartmentId;  
            _team.TeamName = team.TeamName;
            _context.Teams.Add(_team);
            _context.SaveChanges();
           
        }

        public List<Teamlist> GetTeam(int DepartmentID)
        {
            List<Teamlist> teams = new List<Teamlist>();
            var a =  _context.Teams.Where(x => x.DepartmentId == DepartmentID).ToList(); 
         
                foreach(var item in a)
                {
                    Teamlist list = new Teamlist();
                    list.TeamId= item.DepartmentId;
                    list.TeamName= item.TeamName;
                    teams.Add(list);
                }
                return teams;
            
        }

        public List<getDesignation> GetDesignation(int DepartmentID)
        {
            List<getDesignation> designation = new List<getDesignation>();
            var a = _context.Designations1.Where(x => x.DepartmentId == DepartmentID).ToList();

            foreach (var item in a)
            {
                getDesignation list = new getDesignation();
                list.DesignationId = item.DesignationId;
                list.DesignationName = item.DesignationName;
                designation.Add(list);
            }
            return designation;
        }

        public string UpdateEmployee(string EmployeeIdentity, EmployeeVM model)
        {
            try
            {
               var Emp = _context.EmployeeModules.Where(s => s.EmployeeIdentity == EmployeeIdentity && s.IsDeleted != true).FirstOrDefault();
                if (Emp != null)
                {
                    Emp.Name = model.Name;
                    Emp.Email = model.Email;
                    Emp.DepartmentId = model.DepartmentId;
                    Emp.DesignationId = model.DesignationId;
                    Emp.RoleId = model.RoleId;
                    Emp.DateOfJoining = model.DateOfJoining;
                    Emp.PriviousExperience = model.PriviousExperience;
                    //Emp.FirstLevelReportingManager = model.FirstLevelReportingManager;
                    Emp.FirstLevelReportingManager = _context.EmployeeModules.First(m => m.EmployeeIdentity == model.FirstLevelReportingManager).EmployeeId;
                    Emp.SecondLevelReportingManager = _context.EmployeeModules.First(c => c.EmployeeIdentity == model.FirstLevelReportingManager).FirstLevelReportingManager;
                    Emp.DateOfBirth = model.DateOfBirth;
                    Emp.Age = model.Age;
                    Emp.Gender = model.Gender;
                    Emp.MaritalStatus = model.MaritalStatus;
                    Emp.WorkPhoneNumber = model.WorkPhoneNumber;
                    Emp.PersonalPhone = model.PersonalPhone;
                    Emp.PersonalEmail = model.PersonalEmail;
                    Emp.ProfilePicture = model.ProfilePicture;
                    Emp.Salary= model.Salary;
                    Emp.ModifiedTime = DateTime.Now;

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
        public string UpdateDepertment(int id, DepartmentVM department)
        {
            try
            {
                var Dept = _context.Departments.Where(s => s.DepartmentId == id).FirstOrDefault();
                if (Dept != null)
                {
                    Dept.DepartmentName = department.DepartmentName;
                    Dept.ModifiedTime = DateTime.Now;

                    _context.Departments.Update(Dept);
                    return "Updated";
                }
                else
                {
                    return "Department Not Exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string UpdateDesignation(int id, DesignationVM designation)
        {
            try
            {
                var Desig = _context.Designations.Where(s => s.DesignationId == id).FirstOrDefault();
                if (Desig != null)
                {
                    Desig.DesignationName = designation.DesignationName;
                    Desig.ModifiedTime = DateTime.Now;

                    _context.Designations.Update(Desig);
                    return "Updated";
                }
                else
                {
                    return "Designation Not Exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void EmailDelivery()
        {
            var job_1 = _context.ResponseEmails.Where(x => x.IsActive == true && x.IsDeliverd== false && x.Status== true).FirstOrDefault();
            var job_2 = _context.ResponseEmails.Where(x => x.IsActive == true && x.IsDeliverd == false && x.Status == false).FirstOrDefault();            
            var job_3 = _context.ResponseEmails.Where(x => x.IsActive == true && x.IsNotified == false && x.Status == true).FirstOrDefault();          
            var job_4 = _context.ResponseEmails.Where(x => x.IsActive == true && x.IsDeliverd == true && x.IsNotified == true).FirstOrDefault();

            if(job_1 != null)
            {
                var userlvl = _context.UserLevels.Where(x => x.EmployeeId == job_1.EmployeeId && x.SkillId == job_1.Skillid).FirstOrDefault();
                var msg = "(Req_ID " + job_1.ResponseId + ".)" + " " + "</br>" + "Hi " + job_1.FirstLvlManagerName + " Your Update Request(" + job_1.ReqId + ") has Approved";
                var message = new Message(new string[] { job_1.FirstlvlManagerMail }, "Approval Message", msg.ToString(),null);
                var a = _emailservice.SendEmail(message);
                if( a == "ok")
                {
                    job_1.IsDeliverd= true;
                    job_1.DeliverdAt= DateTime.Now;
                    _context.ResponseEmails.Update(job_1);
                    _context.SaveChanges();
                }              
                if(job_1.IsUpdated == false )
                {
                  var b = userlvl.Level+1; 
                 
                    userlvl.Level = b;   
                    job_1.DeliverdAt = DateTime.Now;
                    job_1.IsUpdated= true;
                    _context.ResponseEmails.Update(job_1);
                    _context.SaveChanges();
                    _context.UserLevels.Update(userlvl); 
                    _context.SaveChanges();
                    int? employeeId = job_1.EmployeeId;
                    SkillService skl = new SkillService(_context, _emailservice);
                    skl.PotentialCal(employeeId);
                }
            }
            if(job_2 != null)
            {
                var msg = "(Req_ID " + job_2.ResponseId + ".)" + " " + "</br>" + "Hi " + job_2.FirstLvlManagerName + " Your Update Request(" + job_2.ReqId + ") has been Rejected for some Reason";
                var message = new Message(new string[] { job_2.FirstlvlManagerMail }, "Approval Message", msg.ToString(), null);
                var a = _emailservice.SendEmail(message);
                if(a == "ok")
                { 
                    job_2.IsDeliverd = true;
                    job_2.DeliverdAt= DateTime.Now;
                    job_2.IsNotified= true;
                    _context.ResponseEmails.Update(job_2);
                    _context.SaveChanges();
                }

            }
            if(job_3 != null)
            {
                var msg = " Hi " + job_3.Employeename + " Your Skill Level in " + job_3.SkillName + " To The Next Level By " + job_3.FirstLvlManagerName;
                var message = new Message(new string[] { job_3.Employeemail }, "Skill Updated", msg.ToString(), null);
                var a = _emailservice.SendEmail(message);
                if (a == "ok")
                {
                    job_3.IsNotified = true;
                    job_3.NotifiedAt= DateTime.Now;
                    _context.ResponseEmails.Update(job_3);
                    _context.SaveChanges();
                }

            }

            if(job_4 != null)
            {
                job_4.IsActive = false;
                _context.ResponseEmails.Update(job_4);
                _context.SaveChanges();
            }
        }
        
        public dynamic EmployeeList()
        {
            List<TestEmployeeList> testlist = new List<TestEmployeeList>();
            TestEmployeeVM testemp = new TestEmployeeVM();

            var report1 = _context.EmployeeModules.Where(s => s.IsDeleted == false && s.IsActivated == true).ToList();
            // var toplevel = _context.TopManagements.FirstOrDefault(s => s.Id == 1);
            foreach (var report in report1)
            {
                TestEmployeeList test = new TestEmployeeList();
                var secondmanager = report1.Where(s => s.SecondLevelReportingManager == report.SecondLevelReportingManager).ToList();
                // EmployeeVM employeeVM = new EmployeeVM();
                test.FirstLevelReportingManager = report.FirstLevelReportingManager;
                test.FirstLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.FirstLevelReportingManager).Name;
                test.SecondLevelReportingManager = report.SecondLevelReportingManager;
                test.SecondLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.SecondLevelReportingManager).Name;
                testemp = new TestEmployeeVM();
                testemp.EmployeeId = report.EmployeeId;
                testemp.Name = report.Name;
                testemp.Age = report.Age;
                testemp.DateOfBirth = report.DateOfBirth;
                testemp.DateOfJoining = report.DateOfJoining;
                testemp.DepartmentId = report.DepartmentId;
                testemp.DepartmentName = _context.Departments.First(s => s.DepartmentId == report.DepartmentId).DepartmentName;
                testemp.DesignationId = report.DesignationId;
                testemp.DesignationName = _context.Designations.First(s => s.DesignationId == report.DesignationId).DesignationName;
                testemp.Gender = report.Gender;
                testemp.MaritalStatus = report.MaritalStatus;
                testemp.WorkPhoneNumber = report.WorkPhoneNumber;
                testemp.PersonalEmail = report.PersonalEmail;
                testemp.PersonalPhone = report.PersonalPhone;
                testemp.PriviousExperience = report.PriviousExperience;
                testemp.ProfilePicture = report.ProfilePicture;
                test.EmployeeVMs = testemp;
                testlist.Add(test);
            }
            return testlist;
        }
        public dynamic EmployeeHierachy(int employeeId)
        {
            ManagerVM manager = new ManagerVM();
            //  var toplevel = _context.TopManagements.FirstOrDefault(x => x.Id == 1);
            var emp = _context.EmployeeModules.FirstOrDefault(x => x.EmployeeId == employeeId);
            if (emp != null)
            {
                manager = new ManagerVM();
                manager.EmployeeId = emp.EmployeeId;
                manager.EmployeeName = emp.Name;
                manager.secondLevelManagerId = emp.SecondLevelReportingManager;
                manager.SecondLevelManagerName = _context.EmployeeModules.First(s => s.EmployeeId == emp.SecondLevelReportingManager).Name;
                manager.FirstLevelManagerId = emp.FirstLevelReportingManager;
                manager.FirstLevelManagerName = _context.EmployeeModules.First(s => s.EmployeeId == emp.FirstLevelReportingManager).Name;

            }
            return manager;
        }
        public EmployeeModule EmployeeById(string EmployeeIdentity)
        {
            return _context.EmployeeModules.Where(s => s.EmployeeIdentity == EmployeeIdentity && s.IsDeleted != true).FirstOrDefault();
        }
        public List<EmployeeModule> EmployeeByDepartment(int id)
        {
            return _context.EmployeeModules.Where(X => X.DepartmentId == id && X.IsDeleted != true).ToList();
        }
        public List<Department> DepartmentModule()
        {
            return _context.Departments.ToList();
        }
        public List<Designation> DesignationModule()
        {
            return _context.Designations.ToList();
        }
        public string DeleteEmployee(string EmployeeIdentity)
        {
            var DelEmp = _context.EmployeeModules.Where(s => s.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            if (DelEmp != null)
            {
                DelEmp.IsDeleted = true;
                _context.EmployeeModules.Update(DelEmp);
                return "Deleted";
            }

            return "Error";
        }   
        public dynamic FindRequiredEmployee(FindEmployee find)
        {
            List<filterEmployee> skills = new List<filterEmployee>();
            foreach (var skillId in find.Skillid)
            {
                var skill = from lvl in _context.UserLevels
                            join emp in _context.EmployeeModules
                            on lvl.EmployeeId equals emp.EmployeeId
                            join skl in _context.Skills
                            on lvl.SkillId equals skl.SkillId
                            where skl.SkillId.Equals(skillId) && lvl.Level.Equals(find.Level) && (emp.TotalExperience >= find.MinimumExperience) && (emp.TotalExperience <= find.MaximumExperience) && (emp.IsDeleted != true)
                            select new filterEmployee
                            {
                                EmpId = (int)emp.EmployeeId,
                                EmployeeName = emp.Name,
                                SkillId = skl.SkillId,
                                SkillName = skl.SkillName,
                                Level = lvl.Level

                            };
                skills.AddRange(skill);
            }
            return skills.ToList();
        }

        public List<ReportingPerson> GetReportingPerson()
        {
            List<ReportingPerson> testlist = new List<ReportingPerson>();
            TestEmployeeVM testemp = new TestEmployeeVM();
            List<TestEmployeeVM> testemplist = new List<TestEmployeeVM>();

            var employee = _context.EmployeeModules.ToList();
            foreach (var report in employee)
            {
                ReportingPerson reportingPerson = new ReportingPerson();
                reportingPerson.ReportingPersonId = report.EmployeeIdentity;
                reportingPerson.ReportingPersonName = report.Name;
                testlist.Add(reportingPerson);
            }
            return testlist.ToList();
        }



        public void Save()
        {
            _context.SaveChanges();
        }


        //public string AddUserLevel(int? designationId, int? departmentId, int? employeeId)
        //{
        //    var weightages = _context.Weightages.Where(x => x.DepartmentId.Equals(departmentId) && x.DesignationId.Equals(designationId)).ToList();

        //    foreach (var weightage in weightages)
        //    {
        //        UserLevel module = new UserLevel();
        //        module.EmployeeId = employeeId;
        //        module.SkillId = weightage.SkillId;
        //        module.Level = 0;
        //        module.Weightage = weightage.Weightage1;
        //        _context.UserLevels.Add(module);
        //        _context.SaveChanges();
        //    }
        //    return "Created";
        //}       

        //public string DeleteSkillbyEmp(string EmployeeIdentity, int SkillId)
        //{
        //    var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
        //    var DelskillbyEmp = _context.UserLevels.Where(s => s.EmployeeId == Id.EmployeeId && s.SkillId == SkillId).FirstOrDefault();
        //    if (DelskillbyEmp != null)
        //    {
        //        _context.UserLevels.Remove(DelskillbyEmp);
        //        return "Employee Skill Removed";
        //    }
        //    return "Error";
        //}

        //public string UpdateLevelForEmployee(UserLevelVM level)
        //{
        //    var user = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.EmployeeIdentity == level.EmployeeIdentity).FirstOrDefault();
        //    if (user == null)
        //    {
        //        return "User Not Exist";
        //    }

        //    var weightages = _context.UserLevels.Where(x => x.EmployeeId.Equals(user.EmployeeId) && x.SkillId.Equals(level.SkillId)).FirstOrDefault();
        //    if (weightages != null)
        //    {
        //        weightages.Level = level.Level;
        //        _context.UserLevels.Update(weightages);
        //        return "Updated";
        //    }
        //    return "Error";
        //}


        //public string UpdateSkillWeightage(WeightageVM weightage)
        //{
        //    try
        //    {
        //        var Weight = _context.Weightages.Where(s => s.SkillId == weightage.SkillId && s.DesignationId == weightage.DesignationId && s.DepartmentId == weightage.DepartmentId).FirstOrDefault();
        //        if (Weight != null)
        //        {
        //            Weight.Weightage1 = weightage.Weightage1;
        //            _context.Weightages.Update(Weight);
        //            return "Updated";
        //        }
        //        else
        //        {
        //            return "Skill Not Exists";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void AddDesignation(DesignationVM model)
        //{
        //    Designation designation = new Designation();
        //    designation.DesignationName = model.DesignationName;
        //    designation.AddTime = DateTime.Now;
        //    _context.Designations.Add(designation);
        //}

        //public void AddSkillWeightage(WeightageVM weightage)
        //{
        //    Weightage weightage1 = new Weightage();
        //    weightage1.DepartmentId = weightage.DepartmentId;
        //    weightage1.DesignationId = weightage.DesignationId;
        //    weightage1.SkillId = weightage.SkillId;
        //    weightage1.Weightage1 = weightage.Weightage1;
        //    _context.Weightages.Add(weightage1);
        //}
    }
}










