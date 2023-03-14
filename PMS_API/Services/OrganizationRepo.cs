using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

namespace PMS_API.Services
{

    public class OrganizationRepo : IOrganizationRepo
    {

        private readonly PMSContext _context;
        public OrganizationRepo(PMSContext context)
        {
            _context = context;
        }

        public int? AddEmployee(EmployeeVM model)
        {

            EmployeeModule module = new EmployeeModule();

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
            if(users != null)
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
               // var existingUser = _context.EmployeeModules.FirstOrDefault(x => x.Email == model.Email);
                //if (existingUser == null)
                //{
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
                //}
                //else
                //{
                //    return "User Already Exists";
                //}

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
             //var skill = _context.UserLevels.Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();

            //var deleted = _context.

            var join = from emp in _context.EmployeeModules
                       join lvl in _context.UserLevels
                       on emp.EmployeeId equals lvl.EmployeeId
                       join skl in _context.Skills
                       on lvl.SkillId equals skl.SkillId
                       join dep in _context.Departments
                       on emp.DepartmentId equals dep.DepartmentId
                       join des in _context.Designations
                       on emp.DesignationId equals des.DesignationId where emp.EmployeeId== id && emp.IsDeleted != true
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

        public void Save()
        {
            _context.SaveChanges();
        }


    }
}










