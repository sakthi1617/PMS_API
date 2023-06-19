using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;

namespace PMS_API.Services
{
    public class SkillService: ISkillRepo
    {
        private readonly PMSContext _context;
        private readonly IEmailService _emailservice;
        public SkillService(PMSContext context, IEmailService emailService)
        {
            _context = context;
            _emailservice = emailService;
        }
        public void AddSkillWeightage(WeightageVM weightage)
        {
            if(weightage.SkillId.Count >0)
            {
                foreach( var skill in weightage.SkillId )
                {
                    Weightage weightage1 = new Weightage();
                    var a = _context.Weightages.Where(x => x.SkillId == skill && x.DesignationId == weightage.DesignationId && x.DepartmentId == weightage.DepartmentId).FirstOrDefault();
                    if (a == null)
                    {
                        weightage1.DepartmentId = weightage.DepartmentId;
                        weightage1.DesignationId = weightage.DesignationId;
                        weightage1.TeamId = weightage.TeamId;
                        weightage1.SkillId = skill;
                        weightage1.Weightage1 = 0;
                        _context.Weightages.Add(weightage1);
                        _context.SaveChanges();                       
                    }  
                }
            }            
        }
        public void RemoveSkillWeightage(DeleteWeightage weightage)
        {
            if(weightage.TeamId == 0 || weightage.TeamId == null)
            {
                var skill = _context.Weightages.Where(x => x.DepartmentId == weightage.DepartmentId && x.DesignationId == weightage.DesignationId && x.SkillId == weightage.SkillId).FirstOrDefault();
                if(skill != null)
                _context.Weightages.Remove(skill);
                _context.SaveChanges();
            }
            else
            {
                var skill = _context.Weightages.Where(x => x.DepartmentId == weightage.DepartmentId && x.DesignationId == weightage.DesignationId && x.SkillId == weightage.SkillId && x.TeamId == weightage.TeamId).FirstOrDefault();
                if(skill != null)
                _context.Weightages.Remove(skill);
                _context.SaveChanges();
            }
           
        }
        public string AddAdditionalSkills(UserLevelVM level)
        {
            var users = _context.EmployeeModules.Where(x => x.EmployeeIdentity == level.EmployeeIdentity && x.IsDeleted != true).FirstOrDefault();
            if (users != null)
            {
                var lvl = _context.UserLevels.Where(x => x.SkillId == level.SkillId && x.EmployeeId == users.EmployeeId).FirstOrDefault();
                if (lvl != null)
                {
                    return "Skill Already Exist";
                }
                UserLevel user = new UserLevel();
                user.EmployeeId = users.EmployeeId;
                user.SkillId = level.SkillId;
                user.Level = 0;
                user.Weightage = level.Weightage;
                _context.UserLevels.Add(user);
                return "Success";
            }
            return "User Not exists";
        }
        public IQueryable<GetEmployeeSkillsByIdVM> GetEmployeeSkillsById(string EmployeeIdentity)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var join = from emp in _context.EmployeeModules
                       join lvl in _context.UserLevels
                       on emp.EmployeeId equals lvl.EmployeeId
                       join skl in _context.Skills
                       on lvl.SkillId equals skl.SkillId
                       join dep in _context.Departments
                       on emp.DepartmentId equals dep.DepartmentId
                       join des in _context.Designations
                       on emp.DesignationId equals des.DesignationId
                       where emp.EmployeeId == Id.EmployeeId && emp.IsDeleted != true
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
        public void AddSkill(SkillsVM model)
        {
            Skill skill = new Skill();
            skill.SkillName = model.SkillName;
            _context.Skills.Add(skill);
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

        public string DeleteSkill(int id) 
        {
            var data = _context.Weightages.Where(x => x.SkillId == id).FirstOrDefault(); 
            if (data == null)
            {
                var s =_context.Skills.Where(x =>x.SkillId == id).FirstOrDefault();
                _context.Skills.Remove(s);
                _context.SaveChanges();
                return "success";
            }
            return "error";
        }
        public List<Skill> SkilsList()
        {
            return _context.Skills.ToList();
        }
        public List<Weightage> SkillbyDepartmentID(int DeptId , int DesigId , int teamid)
        {
            return _context.Weightages.Where(x => x.DepartmentId == DeptId && x.DesignationId == DesigId && x.TeamId == teamid).ToList();
        }
        public string UpdateSkillWeightages(int skillId, string employeeIdentity, int weightage)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == employeeIdentity).FirstOrDefault();
            var data = _context.UserLevels.Where(x => x.SkillId == skillId && x.EmployeeId == Id.EmployeeId).FirstOrDefault();
            if (data != null)
            {
                data.Weightage = weightage;
                _context.UserLevels.Update(data);
                _context.SaveChanges();
                return "ok";
            }
            return "Error";
        }
        public dynamic ReqForUpdateLvl(string employeeIdentity, int SklID, string descrip, string rea, IFormFileCollection fiels)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == employeeIdentity && x.IsDeleted == false).FirstOrDefault();
            var userlevel = _context.UserLevels.Where(x => x.SkillId == SklID && x.EmployeeId == Id.EmployeeId).FirstOrDefault();
            if (userlevel.Level >= 4)
            {
                return "MaxLevel";
            }

            if (Id != null)
            {
      

                var first = _context.EmployeeModules.Where(x => x.EmployeeId == Id.FirstLevelReportingManager && x.IsDeleted != true).FirstOrDefault();
                var second = _context.EmployeeModules.Where(x => x.EmployeeId == Id.SecondLevelReportingManager && x.IsDeleted != true).FirstOrDefault();
                
                RequestForApproved request = new RequestForApproved();

                request.EmployeeId = Id.EmployeeId;
                request.RequestCreatedById = first.EmployeeId;
                request.RequestCreatedBy = first.Name;
                request.RequestCreatedAt = DateTime.Now;
                request.Reason = rea;
                request.Comments = descrip;
                request.IsActivated = true;
                request.IsDeliverd = false;
                request.Skillid = SklID;

                _context.RequestForApproveds.Add(request);
                _context.SaveChanges();

                var msg = "(Req_ID " + request.ReqId + ".)" + " " + "</br>" + "Hi " + second.Name + " I Would Like To Improve " + Id.Name + "'s SkillLevel to Next Level For " + "Reason:" + "</br>" + "<h3>" + rea + "Descriptions:" + "</br>" + "<h3>" + "  " + descrip + " Kindly Approve This" + "</br>" + "<button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Confirm</button></br><button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Reject</button>";
                var message = new Message(new string[] { second.Email }, "Requst For Level Update", msg.ToString(), fiels,null);
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
            if (user != null)
            {
           
                var a = _context.EmployeeModules.Where(x => x.EmployeeId == user.EmployeeId).FirstOrDefault();
                var first = _context.EmployeeModules.Where(x => x.EmployeeId == a.FirstLevelReportingManager).FirstOrDefault();
                var second = _context.EmployeeModules.Where(x => x.EmployeeId == a.SecondLevelReportingManager).FirstOrDefault();
              
                var skillname = _context.Skills.Where(x => x.SkillId == user.Skillid).FirstOrDefault();
            
                if (a != null)
                {
                    ResponseEmail email = new ResponseEmail();

                    email.ReqId = reqid;
                    email.Status = status;
                    email.FirstlvlManagerMail = first.Email;
                    email.SecondlvlManagerMail =second.Email;
                    email.Employeemail = a.Email;
                    email.IsDeliverd = false;
                    email.IsNotified = false;
                    email.IsUpdated = false;
                    email.IsActive = true;
                    email.Skillid = user.Skillid;
                    email.SkillName = skillname.SkillName;
                    email.FirstLvlManagerName = first.Name;
                    email.SecondlvlManagerName = second.Name;
                    email.EmployeeId = user.EmployeeId;
                    email.Employeename = a.Name;
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
        public dynamic UserLevelDecrement(string EmployeeIdentity, int skillId)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var userlvl = _context.UserLevels.Where(x => x.EmployeeId == Id.EmployeeId && x.SkillId == skillId).FirstOrDefault();
            if (userlvl != null)
            {
                if (userlvl.Level == 0)
                {
                    _context.UserLevels.Remove(userlvl);
                    _context.SaveChanges();
                    PotentialCal(Id.EmployeeId);
                    return userlvl;
                }
                else
                {
                    var decre = userlvl.Level - 1;
                    userlvl.Level = decre;
                    _context.UserLevels.Update(userlvl);
                    _context.SaveChanges();
                    PotentialCal(Id.EmployeeId);

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
            if(SkillPotential >= 75)
            {
                employee.PotentialStage = 1;
            }
            else if(SkillPotential < 75 && SkillPotential >= 50)
            {
                employee.PotentialStage = 2;
            }
            else
            {
                employee.PotentialStage = 3;
            }
            _context.EmployeeModules.Update(employee);
            _context.SaveChanges();

        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
