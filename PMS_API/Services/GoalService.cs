using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json.Schema;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Ocsp;
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

            var x = DateTime.Now;
            var y = x.Year;
            var z = x.Month + 1;
            var a = 2;

            var d = DateTime.Now.ToString(z + "/" + a + "/" + y);

            var user = _context.EmployeeModules.Where(x=> x.EmployeeId == model.EmployeeId && x.IsDeleted != true).FirstOrDefault();
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
                module.AssingedManagerId = user.FirstLevelReportingManager;
                module.IsSubmitted= false;
                module.IsActive= true;
                module.IsCompleted= false;
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
        public string EmployeeGoalReview(EmployeeReviewVM model)
        {
            EmployeeGoalReview review = new EmployeeGoalReview();
            var Empreviewday = 20;

            var gole = _context.GoalModules.Where(x => x.GoalId == model.GoalId && x.EmployeeId == model.EmployeeId  && x.IsDeleted != true).FirstOrDefault();
          // var module = _context.GoalModules.Where(x => x.EmployeeId == gole.EmployeeId && x.GoalId == gole.GoalId).FirstOrDefault();

            if (gole != null)
            {
                if(gole.IsSubmitted == true)
                {
                    return "Submitted";
                }

                DateTime abc = gole.AssignedAt;
                var A = abc.Year;
                var B = abc.Month + 1;
                var C = Empreviewday;
                var D = abc.Hour;
                var E = abc.Minute;
                var F = abc.Second;

                DateTime dt1 = new DateTime(A, B, C, D, E, F);

                if (dt1 < DateTime.Now && gole.IsEmpExtentionApproved != true)
                {
                    return "Time Up";
                }
                review.EmpReviewId = 0;
                review.EmployeeId = gole.EmployeeId;
                review.AssingedManagerId = gole.AssingedManagerId;
                review.GoalId = gole.GoalId;
                review.EmpReview = model.EmpReview;
                review.GoalRating = model.GoalRating;
                review.IsActive = true;
                review.IsDeleted = false;
                review.CreatedOn = DateTime.Now;    
                _context.EmployeeGoalReviews.Add(review);
                _context.SaveChanges();
                gole.IsSubmitted = true;
                _context.GoalModules.Update(gole);
                _context.SaveChanges();


                int a = review.EmpReviewId;
                if (model.Attachment != null)
                {
                    if (model.Attachment.Count > 0)
                    {
                        foreach (var attach in model.Attachment)
                        {
                            EmployeeAttachment attachment = new EmployeeAttachment();
                            IFormFile file = attach;

                            long length = file.Length;


                            using var fileStream = file.OpenReadStream();
                            byte[] bytes = new byte[length];
                            fileStream.Read(bytes, 0, (int)file.Length);

                            attachment.Attachment = bytes.ToArray();
                            attachment.EmpReviewId = a;
                            attachment.IsActive = true;
                            attachment.IsDeleted = false;
                            attachment.CreatedOn = DateTime.Now;
                            _context.EmployeeAttachments.Add(attachment);
                            _context.SaveChanges();

                        }

                    }
                }

                var data = _context.EmployeeModules.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
                var mandata = _context.ManagersTbls.Where( x=> x.ManagerId == gole.AssingedManagerId).FirstOrDefault(); 
               
                var msg = " Hi " + gole.Assignedby + " " + data.Name + " has Submitted her Goal Review at " + review.CreatedOn + " So Kindly check this before Appraisal Time Period";
                var message = new Message(new string[] { mandata.Email }, "Notification of goal review submission", msg.ToString(), null);

               emailService.SendEmail(message);


            }
            else
            {
                return "Goal Not Exist";
            }

            return "ok";
        }
        public string UpdateEmployeeGoalReview(updateEmployeeReviewVM model)
        {

            var goal = _context.GoalModules.Where( x => x.GoalId == model.GoalId && x.EmployeeId == model.EmployeeId && x.IsSubmitted == true).FirstOrDefault();
            var rev = _context.EmployeeGoalReviews.Where(x => x.EmployeeId == model.EmployeeId && x.GoalId == model.GoalId).FirstOrDefault();
            if (goal != null)
            {
                var Empreviewday = 3;
                DateTime abc = goal.AssignedAt;
                var A = abc.Year;
                var B = abc.Month + 1;
                var C = Empreviewday;
                var D = abc.Hour;
                var E = abc.Minute;
                var F = abc.Second;

                DateTime dt1 = new DateTime(A, B, C, D, E, F);


                if (goal.IsCompleted == true || dt1 < DateTime.Now)
                {
                    return "ReviewEnd";
                }
                else
                {
                    rev.EmployeeId = model.EmployeeId;
                    rev.GoalId = model.GoalId;
                    rev.EmpReview = model.EmpReview;
                    rev.GoalRating= model.GoalRating;
                    _context.EmployeeGoalReviews.Update(rev);
                    _context.SaveChanges();

                    return "updated";
                }
            }

            return "Goal Not Exist";
        }

        public string UpdateManagerGoalReview(ManagerReviewVM model)
        {
           
            var emprev = _context.EmployeeGoalReviews.Where(x => x.EmpReviewId == model.EmpReviewId && x.IsDeleted != true).FirstOrDefault();
            var manrev = _context.ManagerGoalReviews.Where(x => x.EmpReviewId == model.EmpReviewId && x.GoalId == emprev.GoalId).FirstOrDefault();
            var goal = _context.GoalModules.Where(x => x.GoalId == emprev.GoalId && x.EmployeeId == emprev.EmployeeId && x.IsSubmitted == true).FirstOrDefault();


            if (manrev != null)
            {
                var Empreviewday = 3;
                DateTime abc = goal.AssignedAt;
                var A = abc.Year;
                var B = abc.Month + 1;
                var C = Empreviewday;
                var D = abc.Hour;
                var E = abc.Minute;
                var F = abc.Second;

                DateTime dt1 = new DateTime(A, B, C, D, E, F);


                if (dt1 < DateTime.Now)
                {
                    return "ReviewEnd";
                }
                else
                {
                    manrev.ManagerReview = model.ManagerReview;
                    manrev.GoalRating= model.GoalRating;
                    _context.ManagerGoalReviews.Update(manrev);
                    _context.SaveChanges();

                    return "updated";
                }
            }


            return "Goal Not Exist";
        }
        public string ExtentionRequest(int EmployeeID, int GoalID)
        {
            var goal = _context.GoalModules.Where(x => x.EmployeeId == EmployeeID && x.GoalId == GoalID && x.IsDeleted != true).FirstOrDefault();
           
            if (goal != null)
            {
                if (goal.IsEmpExtentionRequested == true)
                {
                    return "Already Requested";
                }
                var join = from emp in _context.EmployeeModules
                           join man in _context.ManagersTbls
                           on emp.FirstLevelReportingManager equals man.ManagerId
                           select new
                           {
                               emp,
                               man
                           };
                var Empl = join.FirstOrDefault();

                var msg = " Hi " + Empl.man.ManagerName + " " + " Please Extend my Review Time ";
                var message = new Message(new string[] { Empl.man.Email }, "Notification of Review Extention Permission", msg.ToString(), null);

               var a = emailService.SendEmail(message);
                if(a == "ok")
                {
                    goal.IsEmpExtentionRequested= true;
                    _context.GoalModules.Update(goal);
                    _context.SaveChanges();
                    return "Ok";
                }
                return "error";

            }
            
            return "error";
        }
        public string ManagerGoalReview(ManagerReviewVM model)
        {
            var Managerreviewday = 3;
            ManagerGoalReview review = new ManagerGoalReview();
            
            var gole = _context.EmployeeGoalReviews.Where(x => x.EmpReviewId == model.EmpReviewId && x.IsDeleted != true && x.IsActive != false).FirstOrDefault();
            var module = _context.GoalModules.Where(x => x.EmployeeId == gole.EmployeeId && x.GoalId == gole.GoalId).FirstOrDefault();

            DateTime abc = module.AssignedAt;
            var A = abc.Year;
            var B = abc.Month + 1;
            var C = Managerreviewday;
            var D = abc.Hour;
            var E = abc.Minute;
            var F = abc.Second;

            DateTime dt1 = new DateTime(A, B, C, D, E, F);

            if (dt1 < DateTime.Now && (module.IsManagerExtentionApproved == false || module.IsManagerExtentionApproved == null))
            {
                return "Time Up";
            }
            if (gole != null)
            {
                review.ManagerReviewId = 0;
                review.EmployeeId = gole.EmployeeId;
                review.AssingedManagerId = gole.AssingedManagerId;
                review.GoalId = gole.GoalId;
                review.ManagerReview = model.ManagerReview;
                review.GoalRating = model.GoalRating;
                review.IsActive = true;
                review.IsDeleted = false;
                review.CreatedOn = DateTime.Now;
                review.EmpReviewId= model.EmpReviewId;  
                _context.ManagerGoalReviews.Add(review);
                _context.SaveChanges();
                module.IsCompleted = true;
                module.IsActive= false;
                _context.GoalModules.Update(module);
                _context.SaveChanges(); 
                gole.IsActive= false;
                _context.EmployeeGoalReviews.Update(gole);
                _context.SaveChanges();
                int a = review.ManagerReviewId;
                
                if (model.Attachment != null)
                {

                    if(model.Attachment.Count > 0)
                    {
                        foreach (var attach in model.Attachment)
                        {
                            ManangerAttachment attachment = new ManangerAttachment();
                            IFormFile file = attach;

                            long length = file.Length;


                            using var fileStream = file.OpenReadStream();
                            byte[] bytes = new byte[length];
                            fileStream.Read(bytes, 0, (int)file.Length);

                            attachment.Attachment = bytes.ToArray();
                            attachment.ManagerReviewId = a;
                            attachment.IsActive = true;
                            attachment.IsDeleted = false;
                            attachment.CreatedOn = DateTime.Now;
                            _context.ManangerAttachments.Add(attachment);
                            _context.SaveChanges();

                        }

                    }

                }
                return "ok";

            }
            else
            {
                return "Goal Not Exist";
            }

           
        }
        public void GoalRatingCalculation(int EmpID)
        {
            
            var TotalGoals1 = _context.ManagerGoalReviews.Where( x => x.EmployeeId == EmpID && x.IsCalculated == false).ToList();
            var TotalGoals2 = _context.EmployeeGoalReviews.Where( x => x.EmployeeId == EmpID && x.IsCalculated == false).ToList();
            var managerid = _context.EmployeeGoalReviews.Where(x => x.EmployeeId == EmpID).FirstOrDefault();
            var NumberOfGoals = TotalGoals1.Count();
            var SummationofRating1 = 0;
            var SummationofRating2 = 0;
            if (TotalGoals1.Count > 0)
            {
                GoalRating rating = new GoalRating();
                foreach (var item in TotalGoals1)
                {
                    SummationofRating1 += Convert.ToInt32((item.GoalRating));
                    item.IsCalculated= true;
                    _context.ManagerGoalReviews.Update(item);
                    _context.SaveChanges();
                }
                foreach (var item in TotalGoals2)
                {
                    SummationofRating2 += Convert.ToInt32((item.GoalRating));
                    item.IsCalculated = true;
                    _context.EmployeeGoalReviews.Update(item);
                    _context.SaveChanges();
                } 

                var a = SummationofRating1 / NumberOfGoals;
                var b = SummationofRating2 / NumberOfGoals; 
                rating.EmployeeId= EmpID;
                rating.ManagerId = managerid.AssingedManagerId;
                rating.RatingbyManager = a;
                rating.RatingbyEmployee = b;
                rating.RatingbyManagerCalculatedAt = DateTime.Now;
                rating.RatingbyEmployeeCalculatedAt = DateTime.Now;
                _context.GoalRatings.Add(rating);
                _context.SaveChanges();

            }
           
        }
        public void Save()
        {
            _context.SaveChanges();
        }

      
    }
}
