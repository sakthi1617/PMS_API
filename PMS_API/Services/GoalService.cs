using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;

namespace PMS_API.Services
{
    public class GoalService : IGoalRepo
    {
        private readonly PMSContext _context;
        private readonly IEmailService emailService;
        public GoalService(PMSContext context, IEmailService _emailService)
        {
            _context=context ;
            emailService = _emailService;
        }
        public string AddGoalForEmployee(GoalVM model)
        {
           var user = _context.EmployeeModules.Where(x=> x.EmployeeId== model.EmployeeId && x.IsDeleted != true).FirstOrDefault();
            if (user != null)
            {
                GoalModule module = new GoalModule();

                module.EmployeeId = model.EmployeeId;
                module.Goalname = model.Goalname;
                module.GoalDescription = model.GoalDescription;
                module.StartDate = model.StartDate;
                module.DueDate = model.DueDate;
                module.Priority = model.Priority;
                module.AssignedAt = DateTime.Now;
                module.Progress = model.Progress;
                module.IsDeleted = false;
                _context.GoalModules.Add(module);
                return "Success";
            }
            return "Error";         
        }

        public string UpdateGoalForEmployee(int empid, int goalid, GoalVM model)
        {
            var name = _context.GoalModules.Where(x => x.EmployeeId == empid && x.GoalId == goalid ).FirstOrDefault();
            if (name != null)
            {      
               name.Goalname = model.Goalname;
               name.GoalDescription = model.GoalDescription;
               name.StartDate = model.StartDate;
               name.DueDate = model.DueDate;
               name.Priority = model.Priority;
               name.ModifyAt = DateTime.Now;
               name.Progress = model.Progress;

                _context.GoalModules.Update(name);
                return "Success";
            }
            return "Error";
        
        }

        public string DeleteGoalForEmployee(int empid, int goalid)
        {
            var user = _context.GoalModules.Where(x => x.EmployeeId == empid && x.GoalId == goalid).FirstOrDefault(); 

            if (user != null)
            {
                user.IsDeleted= true;
                _context.GoalModules.Update(user);
                return "Success";

            }
            return "Error";
        }

        public List<GoalVM> GetGoalbyEmpId(int empid)
        {
           var user = _context.EmployeeModules.Where(x => x.EmployeeId == empid && x.IsDeleted != true).FirstOrDefault();


            if (user != null)
            {
                var goal = _context.GoalModules.Where(X => X.EmployeeId == empid && X.IsDeleted == false).
                    Select(x => new GoalVM 
                    {  EmployeeId= x.EmployeeId , 
                        Goalname =x.Goalname,
                        GoalDescription= x.GoalDescription,
                        StartDate = x.StartDate, 
                        DueDate = x.DueDate, 
                        Priority = x.Priority,
                        Progress = x.Progress })
                    .ToList();   

                if(goal != null)
                {
                   return goal;
                }

                return null;
            }
            return null;
        }


        public string SendMailToEmployee(GoalVM model)
        {
            var data = _context.EmployeeModules.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
            var files =  new FormFileCollection();
            var msg = " Hi " + data.Name + "This Is a Friendly reminder that the Goals has been assigned to you . if you have any question , please don't hesitate to approch.";
            var message = new Message(new string[] { data.Email }, "Notification of goal submission", msg.ToString(), null);

            emailService.SendEmail(message);
            return "ok";
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
