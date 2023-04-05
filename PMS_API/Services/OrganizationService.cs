using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
                module.Name = model.Name;
                module.Email = model.Email;
                module.DepartmentId = model.DepartmentId;
                module.DesignationId = model.DesignationId;
                module.RoleId = model.RoleId;
                module.DateOfJoining = model.DateOfJoining;
                module.PriviousExperience = model.PriviousExperience;
                module.FirstLevelReportingManager = model.FirstLevelReportingManager;
                module.SecondLevelReportingManager = model.SecondLevelReportingManager;
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

                _context.EmployeeModules.Add(module);
                _context.SaveChanges();
                if (module.DesignationId == 2005)
                {
                    managersTbl.EmployeeId = module.EmployeeId;
                    managersTbl.ManagerName = module.Name;
                    managersTbl.Email = module.Email;
                    managersTbl.ContactNumber = module.WorkPhoneNumber;
                    managersTbl.IsDeleted = false;
                    managersTbl.IsActivated = true;
                    _context.ManagersTbls.Add(managersTbl);
                    _context.SaveChanges();
                }

                 return module.EmployeeId;
            }
            else
            {
                return 0;
            }
        }
        public string AddUserLevel(int? designationId, int? departmentId, int? employeeId)
        {
            var weightages = _context.Weightages.Where(x => x.DepartmentId.Equals(departmentId) && x.DesignationId.Equals(designationId)).ToList();

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
        public void AddDesignation(DesignationVM model)
        {
            Designation designation = new Designation();
            designation.DesignationName = model.DesignationName;
            designation.AddTime = DateTime.Now;
            _context.Designations.Add(designation);
        }
        public void AddSkill(SkillsVM model)
        {
            Skill skill = new Skill();
            skill.SkillName = model.SkillName;
            _context.Skills.Add(skill);
        }
        public string AddAdditionalSkills(UserLevelVM level)
        {
            var users = _context.EmployeeModules.Where(x => x.EmployeeId == level.EmployeeId && x.IsDeleted != true).FirstOrDefault();
            if (users != null)
            {
                var lvl = _context.UserLevels.Where(x => x.SkillId == level.SkillId && x.EmployeeId == level.EmployeeId).FirstOrDefault();
                if (lvl != null)
                {
                    return "Skill Already Exist";
                }
                UserLevel user = new UserLevel();
                user.EmployeeId = level.EmployeeId;
                user.SkillId = level.SkillId;
                user.Level = 0;
                user.Weightage = level.Weightage;
                _context.UserLevels.Add(user);
                return "Success";
            }
            return "User Not exists";

        }
        public void AddSkillWeightage(WeightageVM weightage)
        {
            Weightage weightage1 = new Weightage();
            weightage1.DepartmentId = weightage.DepartmentId;
            weightage1.DesignationId = weightage.DesignationId;
            weightage1.SkillId = weightage.SkillId;
            weightage1.Weightage1 = weightage.Weightage1;
            _context.Weightages.Add(weightage1);
        }
        public string UpdateEmployee(int id, EmployeeVM model)
        {
            try
            {
               var Emp = _context.EmployeeModules.Where(s => s.EmployeeId == id && s.IsDeleted != true).FirstOrDefault();
                if (Emp != null)
                {
                    Emp.Name = model.Name;
                    Emp.Email = model.Email;
                    Emp.DepartmentId = model.DepartmentId;
                    Emp.DesignationId = model.DesignationId;
                    Emp.RoleId = model.RoleId;
                    Emp.DateOfJoining = model.DateOfJoining;
                    Emp.PriviousExperience = model.PriviousExperience;
                    Emp.FirstLevelReportingManager = model.FirstLevelReportingManager;
                    Emp.SecondLevelReportingManager = model.SecondLevelReportingManager;
                    Emp.DateOfBirth = model.DateOfBirth;
                    Emp.Age = model.Age;
                    Emp.Gender = model.Gender;
                    Emp.MaritalStatus = model.MaritalStatus;
                    Emp.WorkPhoneNumber = model.WorkPhoneNumber;
                    Emp.PersonalPhone = model.PersonalPhone;
                    Emp.PersonalEmail = model.PersonalEmail;
                    Emp.ProfilePicture = model.ProfilePicture;
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
        public string UpdateSkill(int id, SkillsVM skills)
        {
            try
            {
                var skill = _context.Skills.Where(s => s.SkillId == id).FirstOrDefault();
                if (skill != null)
                {
                    skill.SkillName = skills.SkillName;
                    _context.Skills.Update(skill);
                    return "Updated";
                }
                else
                {
                    return "Skill Not Exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic ReqForUpdateLvl(int EmpID, int SklID, string descrip, string rea, IFormFileCollection fiels)
        {
            var userlevel = _context.UserLevels.Where(x =>x.SkillId == SklID && x.EmployeeId==EmpID).FirstOrDefault();
            if(userlevel.Level >= 4)
            {
                return "MaxLevel";
            }
             var user = _context.EmployeeModules.Where(x => x.EmployeeId.Equals(EmpID) && x.IsDeleted.Equals(false)).FirstOrDefault();//--------------------

            if (user != null)
            {
                var data = from emp in _context.EmployeeModules
                           join man in _context.ManagersTbls
                           on emp.FirstLevelReportingManager equals man.ManagerId
                           where emp.EmployeeId == EmpID && emp.IsDeleted != true //------------------
                           select new { emp, man };

                var Data = from emp in _context.EmployeeModules
                           join man in _context.ManagersTbls
                           on emp.SecondLevelReportingManager equals man.ManagerId
                           where emp.EmployeeId == EmpID && emp.IsDeleted != true  //---------------------
                           select new { emp, man };

                var first = data.FirstOrDefault();
                var second = Data.FirstOrDefault();


                RequestForApproved request = new RequestForApproved();

                request.EmployeeId = EmpID;
                request.RequestCreatedById = first.man.ManagerId;
                request.RequestCreatedBy = first.man.ManagerName;
                request.RequestCreatedAt = DateTime.Now;
                request.Reason = rea;
                request.Comments = descrip;
                request.IsActivated = true;
                request.IsDeliverd = false;
                request.Skillid= SklID;
                
                _context.RequestForApproveds.Add(request);
                _context.SaveChanges();
                
                var msg = "(Req_ID " + request.ReqId + ".)" + " " + "</br>" + "Hi " + second.man.ManagerName + " I Would Like To Improve " + user.Name + "'s SkillLevel to Next Level For " + "Reason:" + "</br>" + "<h3>" + rea + "Descriptions:" + "</br>" + "<h3>" + "  " + descrip + " Kindly Approve This" + "</br>" + "<button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Confirm</button></br><button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Reject</button>";
                var message = new Message(new string[] { second.man.Email }, "Requst For Level Update", msg.ToString(), fiels);
                var a = _emailservice.SendEmail(message);

                if (a == "ok")
                {
                    var abc = _context.RequestForApproveds.Where(x => x.ReqId == request.ReqId).FirstOrDefault();
                      abc.IsDeliverd = true;
                     _context.RequestForApproveds.Update(abc);
                     _context.SaveChanges();
                }
                else
                {
                    var abc = _context.RequestForApproveds.Where(x => x.ReqId == request.ReqId).FirstOrDefault();
                    abc.IsActivated = false;
                    _context.RequestForApproveds.Update(abc);
                    _context.SaveChanges();
                }

                return "Ok";
            }
            return "Error";
        }
        public string LevlelApprovedSuccess(int reqid, bool status)
        {
            
            var user = _context.RequestForApproveds.Where(x => x.ReqId == reqid && x.IsActivated == true).FirstOrDefault();
            if(user != null)
            {
                var mail1 = from emp in _context.EmployeeModules
                           join man in _context.ManagersTbls
                           on emp.FirstLevelReportingManager  equals man.ManagerId
                           where emp.EmployeeId == user.EmployeeId
                           select new { emp, man };

                var mail2 = from emp in _context.EmployeeModules
                           join man in _context.ManagersTbls
                           on emp.SecondLevelReportingManager  equals man.ManagerId
                           where emp.EmployeeId == user.EmployeeId
                           select new { emp, man };

                var skillname = _context.Skills.Where(x => x.SkillId == user.Skillid).FirstOrDefault();
                var a = mail1.FirstOrDefault();
                var b = mail2.FirstOrDefault();

                if(a != null)
                {
                    ResponseEmail email = new ResponseEmail();  

                    email.ReqId= reqid;
                    email.Status= status;
                    email.FirstlvlManagerMail = a.man.Email;
                    email.SecondlvlManagerMail = a.man.Email;
                    email.Employeemail= a.emp.Email;
                    email.IsDeliverd = false;
                    email.IsNotified= false;
                    email.IsUpdated= false;
                    email.IsActive = true;
                    email.Skillid= user.Skillid;
                    email.SkillName= skillname.SkillName;
                    email.FirstLvlManagerName = a.man.ManagerName;
                    email.SecondlvlManagerName= b.man.ManagerName;
                    email.EmployeeId = user.EmployeeId;
                    email.Employeename = a.emp.Name;
                    _context.ResponseEmails.Add(email);
                    _context.SaveChanges();
                   
                    return "Ok";
                }
                else
                {
                    return "erorr";
                }              
            }
            else
            {
                return "RequestExpired";
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
                    PotentialCal(employeeId);
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
        public string UpdateLevelForEmployee(UserLevelVM level)
        {
            var user = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.EmployeeId == level.EmployeeId).FirstOrDefault();
            if (user == null)
            {
                return "User Not Exist";
            }

            var weightages = _context.UserLevels.Where(x => x.EmployeeId.Equals(level.EmployeeId) && x.SkillId.Equals(level.SkillId)).FirstOrDefault();
            if (weightages != null)
            {
                weightages.Level = level.Level;
                _context.UserLevels.Update(weightages);
                return "Updated";
            }
            return "Error";
        }
        public string UpdateSkillWeightage(WeightageVM weightage)
        {
            try
            {
                var Weight = _context.Weightages.Where(s => s.SkillId == weightage.SkillId && s.DesignationId == weightage.DesignationId && s.DepartmentId == weightage.DepartmentId).FirstOrDefault();
                if (Weight != null)
                {
                    Weight.Weightage1 = weightage.Weightage1;
                    _context.Weightages.Update(Weight);
                    return "Updated";
                }
                else
                {
                    return "Skill Not Exists";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeModule> EmployeeList()
        {
            return _context.EmployeeModules.Where(s => s.IsDeleted != true && s.IsActivated != false).ToList();
        }
        public EmployeeModule EmployeeById(int id)
        {
            return _context.EmployeeModules.Where(s => s.EmployeeId == id && s.IsDeleted != true).FirstOrDefault();
        }
        public List<EmployeeModule> EmployeeByDepartment(int id)
        {
            return _context.EmployeeModules.Where(X => X.DepartmentId == id && X.IsDeleted != true).ToList();
        }
        public List<Department> DepartmentModule()
        {
            return _context.Departments.ToList();
        }
        public List<Skill> SkilsList()
        {
            return _context.Skills.ToList();
        }
        public List<Weightage> SkillbyDepartmentID(int id)
        {
            return _context.Weightages.Where(x => x.DepartmentId == id).ToList();
        }
        public List<Designation> DesignationModule()
        {
            return _context.Designations.ToList();
        }
        public IQueryable<GetEmployeeSkillsByIdVM> GetEmployeeSkillsById(int id)
        {
           
                var join = from emp in _context.EmployeeModules
                           join lvl in _context.UserLevels
                           on emp.EmployeeId equals lvl.EmployeeId
                           join skl in _context.Skills
                           on lvl.SkillId equals skl.SkillId
                           join dep in _context.Departments
                           on emp.DepartmentId equals dep.DepartmentId
                           join des in _context.Designations
                           on emp.DesignationId equals des.DesignationId
                           where emp.EmployeeId == id && emp.IsDeleted != true
                           select new GetEmployeeSkillsByIdVM
                           {
                               EmployeeId = emp.EmployeeId,
                               EmpName = emp.Name,
                               Skill = skl.SkillName,
                               SkillId = skl.SkillId,
                               DepartmenName = dep.DepartmentName,
                               DepartmentId = dep.DepartmentId,
                               DesignationId = des.DesignationId,
                               DesignationName = des.DesignationName,
                               Level = lvl.Level,
                               Weightage = lvl.Weightage,
                               PotentialLevel = emp.PotentialLevel

                           };

                return join;
      
        }
        public string DeleteEmployee(int EmployeeId)
        {
            var DelEmp = _context.EmployeeModules.Where(s => s.EmployeeId == EmployeeId).FirstOrDefault();
            if (DelEmp != null)
            {
                DelEmp.IsDeleted = true;
                _context.EmployeeModules.Update(DelEmp);
                return "Deleted";
            }

            return "Error";
        }
        public string DeleteSkillbyEmp(int EmployeeId, int SkillId)
        {
            var DelskillbyEmp = _context.UserLevels.Where(s => s.EmployeeId == EmployeeId && s.SkillId == SkillId).FirstOrDefault();
            if (DelskillbyEmp != null)
            {
                _context.UserLevels.Remove(DelskillbyEmp);
                return "Employee Skill Removed";
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
        public dynamic UserLevelDecrement(int Employeeid, int skillId)
        {
            var userlvl = _context.UserLevels.Where(x => x.EmployeeId == Employeeid && x.SkillId == skillId).FirstOrDefault();
            if (userlvl != null)
            {
                if (userlvl.Level == 0)
                {
                    _context.UserLevels.Remove(userlvl);
                    _context.SaveChanges();
                    PotentialCal(Employeeid);
                    return userlvl;
                }
                else
                {
                    var decre = userlvl.Level - 1;
                    userlvl.Level = decre;
                    _context.UserLevels.Update(userlvl);
                    _context.SaveChanges();
                    PotentialCal(Employeeid);

                    return userlvl;
                }
            }
            return null;
        }
        public void PotentialCal(int? employeeId)
        {
            var UserLevel = _context.UserLevels.Where(x => x.EmployeeId == employeeId).ToList();
            var employee = _context.EmployeeModules.Where(c => c.EmployeeId == employeeId).FirstOrDefault();
            decimal WeightedSkillScore = 0;
            decimal TotalPossibleScore = 0;
            int SkillPotential = 0;
            int percent = 0;

            foreach (var item in UserLevel)
            {
                if (item.Level == 1)
                {
                    percent = 25;
                }
                else if (item.Level == 2)
                {
                    percent = 50;
                }
                else if (item.Level == 3)
                {
                    percent = 75;
                }
                else if (item.Level == 4)
                {
                    percent = 100;
                }
                else
                {
                    percent = 0;
                }
                WeightedSkillScore += Convert.ToDecimal(((item.Weightage * percent)));
                TotalPossibleScore += Convert.ToDecimal((100 * item.Weightage));
            }
            SkillPotential = Convert.ToInt32(((WeightedSkillScore / TotalPossibleScore) * 100));
            employee.PotentialLevel = SkillPotential;
            _context.EmployeeModules.Update(employee);
            _context.SaveChanges();
            
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}










