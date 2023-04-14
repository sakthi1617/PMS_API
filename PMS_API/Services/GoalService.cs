using Hangfire.Common;
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
using PMS_API.Models;
using System.Security.Cryptography;

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
            var user = _context.EmployeeModules.Where(x=> x.EmployeeIdentity == model.EmployeeIdentity && x.IsDeleted != true).FirstOrDefault();
            if (user != null)
            {
                GoalModule module = new GoalModule();
                var Today = DateTime.Now;
                DateTime a = (DateTime)model.DueDate;
                var dueMonth = a.Month;
                
                if(Today.Month != dueMonth)
                {
                    DelayedGoal delayed = new DelayedGoal();
                    delayed.EmployeeId = user.EmployeeId;
                    delayed.Goalname = model.Goalname;
                    delayed.GoalDescription = model.GoalDescription;
                    delayed.StartDate = model.StartDate;
                    delayed.DueDate = model.DueDate;
                    delayed.Priority = model.Priority;
                    delayed.AssignedAt = DateTime.Now;
                    delayed.Progress = model.Progress;
                    //delayed.Assignedby = model.AssignedBy;
                    delayed.AssingedManagerId = user.FirstLevelReportingManager;
                    _context.DelayedGoals.Add(delayed);
                    _context.SaveChanges();
                    return "Delay";
                }         
                module.EmployeeId = user.EmployeeId;
                module.Goalname = model.Goalname;
                module.GoalDescription = model.GoalDescription;
                module.StartDate = model.StartDate;
                module.DueDate = model.DueDate;
                module.Priority = model.Priority;
                module.CreatedAt = DateTime.Now;
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
        public string UpdateGoalForEmployee(string EmployeeIdentity, int goalid, GoalVM model)
        {
            var Id = _context.EmployeeModules.Where(x=> x.EmployeeIdentity== EmployeeIdentity).FirstOrDefault();
            var name = _context.GoalModules.Where(x => x.EmployeeId == Id.EmployeeId && x.GoalId == goalid ).FirstOrDefault();
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
        //public string DeleteGoalForEmployee(string EmployeeIdentity, int goalid)
        //{
        //    var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity== EmployeeIdentity).FirstOrDefault();   
        //    var user = _context.GoalModules.Where(x => x.EmployeeId == Id.EmployeeId && x.GoalId == goalid).FirstOrDefault(); 

        //    if (user != null)
        //    {
        //        user.IsDeleted= true;
        //        _context.GoalModules.Update(user);
        //        return "Success";
        //    }
        //    return "Error";
        //}
        public List<GoalVM> GetGoalbyEmpId(string EmployeeIdentity)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var a = (from emp in _context.EmployeeModules
                    join gol in _context.GoalModules
                    on emp.EmployeeId equals gol.EmployeeId
                    where emp.EmployeeId == Id.EmployeeId && emp.IsDeleted != true&& gol.IsDeleted != true
                    select new GoalVM
                    {
                        GoalId = gol.GoalId,
                        EmployeeIdentity = emp.EmployeeIdentity,
                        Goalname = gol.Goalname,
                        GoalDescription = gol.GoalDescription,
                        StartDate = gol.StartDate,
                        DueDate = gol.DueDate,
                        Priority = gol.Priority,
                        Progress = gol.Progress
                    }).ToList();                   
                     
            return a;
        }
        public string SendMailToEmployee(GoalVM model)
        {
            
            var data = _context.EmployeeModules.Where(x => x.EmployeeIdentity == model.EmployeeIdentity).FirstOrDefault();
            
            //var files =  new FormFileCollection();
            var msg = " Hi " + data.Name + " This Is a Friendly reminder that the Goals has been assigned to you . if you have any question , please don't hesitate to approch.";
            var message = new Message(new string[] { data.Email }, "Notification of goal submission", msg.ToString(), null);

            emailService.SendEmail(message);
            return "ok";
        }
        public string EmployeeGoalReview(EmployeeReviewVM model)
        {
            EmployeeGoalReview review = new EmployeeGoalReview();
           var Id = _context.EmployeeModules.Where( x => x.EmployeeIdentity == model.EmployeeIdentity).FirstOrDefault();    

            var gole = _context.GoalModules.Where(x => x.GoalId == model.GoalId && x.EmployeeId == Id.EmployeeId  && x.IsDeleted != true).FirstOrDefault();          

            if (gole != null)
            {
                if(gole.IsSubmitted == true)
                {
                    return "Submitted";
                }

                if (gole.IsFreezedEmp == true && (gole.IsEmpExtentionApproved != true || gole.IsEmpExtentionApproved == null))
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
               


                //int a = review.EmpReviewId;
                //if (model.Attachment != null)
                //{
                //    if (model.Attachment.Count > 0)
                //    {
                //        foreach (var attach in model.Attachment)
                //        {
                //            EmployeeAttachment attachment = new EmployeeAttachment();
                //            IFormFile file = attach;

                //            long length = file.Length;


                //            using var fileStream = file.OpenReadStream();
                //            byte[] bytes = new byte[length];
                //            fileStream.Read(bytes, 0, (int)file.Length);

                //            attachment.Attachment = bytes.ToArray();
                //            attachment.EmpReviewId = a;
                //            attachment.IsActive = true;
                //            attachment.IsDeleted = false;
                //            attachment.CreatedOn = DateTime.Now;
                //            _context.EmployeeAttachments.Add(attachment);
                //            _context.SaveChanges();

                //        }

                //    }
                //}

                var data = _context.EmployeeModules.Where(x => x.EmployeeId == Id.EmployeeId).FirstOrDefault();
                var mandata = _context.ManagersTbls.Where( x=> x.ManagerId == gole.AssingedManagerId).FirstOrDefault(); 
               
                var msg = " Hi " + gole.Assignedby + " " + data.Name + " has Submitted her Goal Review at " + review.CreatedOn + " So Kindly check this before Appraisal Time Period";
                var message = new Message(new string[] { mandata.Email }, "Notification of goal review submission", msg.ToString(), null);

              var a =  emailService.SendEmail(message);

                if(a == "ok")
                {
                    gole.IsSubmitted = true;
                    gole.IsReviewNotified = true;
                    _context.GoalModules.Update(gole);
                    _context.SaveChanges();
                    return "ok";
                }
                else
                {
                    return "Error";
                }


            }
            else
            {
                return "Goal Not Exist";
            }

           
        }
        public string UpdateEmployeeGoalReview(updateEmployeeReviewVM model)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == model.EmployeeIdentity).FirstOrDefault();
            var goal = _context.GoalModules.Where( x => x.GoalId == model.GoalId && x.EmployeeId == Id.EmployeeId && x.IsSubmitted == true).FirstOrDefault();
            var rev = _context.EmployeeGoalReviews.Where(x => x.EmployeeId == Id.EmployeeId && x.GoalId == model.GoalId).FirstOrDefault();
            if (goal != null)
            {
                var Empreviewday = 3;
                DateTime abc = goal.CreatedAt;
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
                    rev.EmployeeId = Id.EmployeeId;
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
                if (goal.IsFreezedManager == true && goal.IsManagerExtentionApproved != true)
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
        public string EmployeeExtentionRequest(string EmployeeIdentity, int GoalID)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var goal = _context.GoalModules.Where(x => x.EmployeeId == Id.EmployeeId && x.GoalId == GoalID && x.IsDeleted != true).FirstOrDefault();
           
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

                var msg = " Hi " + Empl.man.ManagerName + " " + " Please Extend my Review Time " +""+ "<button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Confirm</button></br><button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Reject</button>";
                var message = new Message(new string[] { Empl.man.Email }, "Notification of Review Extention Permission", msg.ToString(), null);

                var a = emailService.SendEmail(message);
                if(a == "ok")
                {
                    goal.IsEmpExtentionRequested = true;
                    _context.GoalModules.Update(goal);
                    _context.SaveChanges();
                    return "Ok";
                }
                return "error";

            }
            
            return "error";
        }
        public string ManagerExtentionRequest(string EmployeeIdentity, int GoalID)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var EmpDetail = _context.EmployeeModules.Where(x => x.EmployeeId == Id.EmployeeId).FirstOrDefault();
            var manager = _context.ManagersTbls.Where(x=>x.ManagerId == EmpDetail.FirstLevelReportingManager).FirstOrDefault();
            var Empid = _context.ManagersTbls.Where(x => x.EmployeeId == Id.EmployeeId).FirstOrDefault();
            var goid = _context.GoalModules.Where(x => x.GoalId == GoalID && x.AssingedManagerId == Empid.ManagerId).FirstOrDefault();

            if (goid != null)
            {
                if (goid.IsManagerExtentionRequested == true)
                {
                    return "Already Requested";
                }
                
              
                var msg = " Hi " + manager.ManagerName + " " + " Please Extend my Review Time " + "" + "<button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Confirm</button></br><button type=\"button\" class=\"btn btn-success\" style=\"width:75px;height:50px;font-size:20px\">Reject</button>";
                var message = new Message(new string[] { manager.Email }, "Notification of Review Extention Permission", msg.ToString(), null);
                

                var a = emailService.SendEmail(message);
                if (a == "ok")
                {
                    goid.IsManagerExtentionRequested = true;
                    _context.GoalModules.Update(goid);
                     _context.SaveChanges();
                    return "Ok";
                }
                return "error";

            }

            return "error";
        }
        public string EmpExtReqApprove(bool approved, int GoalId)
        {
            var goaldetail = _context.GoalModules.Where(x => x.IsEmpExtentionRequested == true && x.GoalId == GoalId).FirstOrDefault();
            var empemil= _context.EmployeeModules.Where(x => x.EmployeeId == goaldetail.EmployeeId).FirstOrDefault();
            if (goaldetail != null)
            {
                if(approved == true)
                {
                    goaldetail.IsEmpExtentionApproved = true;
                    goaldetail.IsEmpExtentionApprovedAt = DateTime.Now;
                    _context.GoalModules.Update(goaldetail);
                    var msg = " Hi " + empemil.Name + " " + " Your Review Extension Time is Approved For next 24Hours So Kindly Complete Your Goal Reviews within That Time Duration ";
                    var message = new Message(new string[] {empemil.Email}, "Notification of Review Extention Permission", msg.ToString(), null);

                    var a = emailService.SendEmail(message);
                    if(a == "ok")
                    {
                        _context.SaveChanges();
                        return "ok";
                    }
                   return"error";
                }
                else
                {
                    goaldetail.IsEmpExtentionApproved = false;
                    goaldetail.IsEmpExtentionApprovedAt = DateTime.Now;
                    _context.GoalModules.Update(goaldetail);
                    _context.SaveChanges();
                    return "Notok";
                }
                
            }
            return "Error";
        }      
        public string ManagerExtReqApprove(bool approved, int GoalId)
        {
            var goaldetail = _context.GoalModules.Where(x => x.IsManagerExtentionRequested == true && x.GoalId == GoalId).FirstOrDefault();
            var empmail= _context.EmployeeModules.Where(x => x.EmployeeId == goaldetail.EmployeeId).FirstOrDefault();
            var managermail = _context.ManagersTbls.Where(x => x.ManagerId == empmail.FirstLevelReportingManager).FirstOrDefault();
            if (goaldetail != null)
            {
                if(approved == true)
                {
                    goaldetail.IsManagerExtentionApproved = true;
                    goaldetail.IsManagerExtentionApprovedAt = DateTime.Now;
                    _context.GoalModules.Update(goaldetail);
                    var msg = " Hi " + managermail.ManagerName + " " + " Your Review Extension Time is Approved For next 24Hours So Kindly Colmpelte Your Goal Reviews within That Time Duration " + "" ;
                    var message = new Message(new string[] {managermail.Email}, "Notification of Review Extention Permission", msg.ToString(), null);

                    var a = emailService.SendEmail(message);
                    if(a == "ok")
                    {
                        _context.SaveChanges();
                        return "ok";
                    }
                    return "error";

                }
                else
                {
                    goaldetail.IsManagerExtentionApproved = false;
                    goaldetail.IsManagerExtentionApprovedAt = DateTime.Now;
                    _context.GoalModules.Update(goaldetail);
                    _context.SaveChanges();
                    return "ok";
                }
                
            }
            return "Error";
        }               
        public string AdminReqApproved(bool Approved, int delayedGoalId)
        {
            var data = _context.DelayedGoals.Where( x => x.DelayedGoalId == delayedGoalId).FirstOrDefault();
            if(Approved == true)
            {
                data.IsAdminApproved = true;
                _context.DelayedGoals.Update(data);
                _context.SaveChanges();
            }
            else
            {
                data.IsAdminApproved = false;   
                _context.DelayedGoals.Update(data);
                _context.SaveChanges();
            }
            return "Ok";
        }        
        public string ManagerGoalReview(ManagerReviewVM model)
        {
            
            ManagerGoalReview review = new ManagerGoalReview();
            
            var gole = _context.EmployeeGoalReviews.Where(x => x.EmpReviewId == model.EmpReviewId && x.IsDeleted != true && x.IsActive != false).FirstOrDefault();
          
            if (gole != null)
            {
                var module = _context.GoalModules.Where(x => x.EmployeeId == gole.EmployeeId && x.GoalId == gole.GoalId).FirstOrDefault();

                if (module.IsCompleted == true)
                {
                    return "completed";
                }

                if (module.IsFreezedManager == true && (module.IsManagerExtentionApproved == false || module.IsManagerExtentionApproved == null))
                {
                    return "Time Up";
                }
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
                //int a = review.ManagerReviewId;
                
                //if (model.Attachment != null)
                //{

                //    if(model.Attachment.Count > 0)
                //    {
                //        foreach (var attach in model.Attachment)
                //        {
                //            ManangerAttachment attachment = new ManangerAttachment();
                //            IFormFile file = attach;

                //            long length = file.Length;


                //            using var fileStream = file.OpenReadStream();
                //            byte[] bytes = new byte[length];
                //            fileStream.Read(bytes, 0, (int)file.Length);

                //            attachment.Attachment = bytes.ToArray();
                //            attachment.ManagerReviewId = a;
                //            attachment.IsActive = true;
                //            attachment.IsDeleted = false;
                //            attachment.CreatedOn = DateTime.Now;
                //            _context.ManangerAttachments.Add(attachment);
                //            _context.SaveChanges();

                //        }

                //    }

                //}
                return "ok";

            }
            else
            {
                return "Goal Not Exist";
            }

           
        }
        public void GoalRatingCalculation(string EmployeeIdentity)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity).FirstOrDefault();
            var TotalGoals1 = _context.ManagerGoalReviews.Where( x => x.EmployeeId == Id.EmployeeId && x.IsCalculated != true).ToList();
            var TotalGoals2 = _context.EmployeeGoalReviews.Where( x => x.EmployeeId == Id.EmployeeId && x.IsCalculated != true).ToList();
            var managerid = _context.EmployeeGoalReviews.Where(x => x.EmployeeId == Id.EmployeeId).FirstOrDefault();
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
                rating.EmployeeId= Id.EmployeeId;
                rating.ManagerId = managerid.AssingedManagerId;
                rating.RatingbyManager = a;
                rating.RatingbyEmployee = b;
                rating.RatingbyManagerCalculatedAt = DateTime.Now;
                rating.RatingbyEmployeeCalculatedAt = DateTime.Now;
                _context.GoalRatings.Add(rating);
                _context.SaveChanges();

            }
           
        }
        public void Frezze()
        {
            
            var date = DateTime.Now;
            var Egoal = _context.GoalModules.Where(x => (x.IsSubmitted != true || x.IsSubmitted == null)&& (x.IsFreezedEmp == null || x.IsFreezedEmp == false)).FirstOrDefault();
            var Mgoal = _context.GoalModules.Where(x => (x.IsCompleted != true || x.IsCompleted == null) && (x.IsFreezedManager == null || x.IsFreezedManager == false)).FirstOrDefault();
            var timesetting = _context.TimeSettingTbls.FirstOrDefault(x => x.Id == 1);
            var Empreviewday = Convert.ToInt32(timesetting.EmployeeReviewDay);
            var Managerreviewday = Convert.ToInt32(timesetting.ManagerReviewDay);
            if (Egoal != null)
            {
                DateTime abc = Egoal.CreatedAt;
                var A = abc.Year;
                var B = abc.Month + 1;
                var C = Empreviewday;
                var D = abc.Hour;
                var E = abc.Minute;
                var F = abc.Second;
                var G = Managerreviewday;
                if(B > 12)
                {
                    DateTime dt1 = new DateTime(A+1, 1, C, D, E, F);

                    if (dt1 < DateTime.Now)
                    {
                        Egoal.IsFreezedEmp = true;
                        _context.GoalModules.Update(Egoal);
                        _context.SaveChanges();

                    }
                    else
                    {
                        Egoal.IsFreezedEmp = false;
                        _context.GoalModules.Update(Egoal);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    DateTime dt11 = new DateTime(A, B, C, D, E, F);


                    if (dt11 < DateTime.Now)
                    {
                        Egoal.IsFreezedEmp = true;
                        _context.GoalModules.Update(Egoal);
                        _context.SaveChanges();

                    }
                    else
                    {
                        Egoal.IsFreezedEmp = false;
                        _context.GoalModules.Update(Egoal);
                        _context.SaveChanges();
                    }
                }
               

            }
            if(Mgoal!= null)
            {
                DateTime abc = Mgoal.CreatedAt;
                var A = abc.Year;
                var B = abc.Month + 1;
                var C = Empreviewday;
                var D = abc.Hour;
                var E = abc.Minute;
                var F = abc.Second;
                var G = Managerreviewday;
                if (B > 12)
                {
                    DateTime dt2 = new DateTime(A + 1, 1, C, D, E, F);
                    if (dt2 < DateTime.Now)
                    {
                        Mgoal.IsFreezedManager = true;
                        _context.GoalModules.Update(Mgoal);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Mgoal.IsFreezedManager = false;
                        _context.GoalModules.Update(Mgoal);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    DateTime dt21 = new DateTime(A, B, G, D, E, F);


                    if (dt21 < DateTime.Now)
                    {
                        Mgoal.IsFreezedManager = true;
                        _context.GoalModules.Update(Mgoal);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Mgoal.IsFreezedManager = false;
                        _context.GoalModules.Update(Mgoal);
                        _context.SaveChanges();
                    }
                }
               
            }

        }
        public void ExtentionUpdate()
        {
            var Job_1 = _context.GoalModules.Where(x => x.IsEmpExtentionApproved == true).FirstOrDefault();
            var Job_2 = _context.GoalModules.Where(y => y.IsManagerExtentionApproved == true).FirstOrDefault();
            if (Job_1 != null)
            {
                DateTime a = Job_1.IsEmpExtentionApprovedAt.Value;
                var b = a.AddDays(1);
                if (DateTime.Now > b)
                {
                    Job_1.IsEmpExtentionApproved = false;
                    _context.GoalModules.Update(Job_1);
                    _context.SaveChanges();
                }
            }
            if (Job_2 != null)
            {
                DateTime a = Job_2.IsManagerExtentionApprovedAt.Value;
                var b = a.AddDays(1);
                if (DateTime.Now > b)
                {
                    Job_2.IsManagerExtentionApproved = false;
                    _context.GoalModules.Update(Job_2);
                    _context.SaveChanges();
                }
            }
        }
        public void sendMailtoAdmin()
        {
            var admin = _context.EmployeeModules.Where(x => x.RoleId == 1).FirstOrDefault();
            var Job =  _context.DelayedGoals.Where( x => x.IsNotified != true).FirstOrDefault();
            if(Job != null)
            {
                var msg = " Hi " + admin.Name + "," + " " + Job.Assignedby + "Assigned privious month goal for " + Job.EmployeeId + "In this Month" + " " + "</br>" + Job.DelayedGoalId;
                var message = new Message(new string[] { admin.Email }, "Notification For Delayed Goals", msg.ToString(), null);
                var a = emailService.SendEmail(message);
                if (a == "ok")
                {
                    Job.IsNotified = true;
                    _context.DelayedGoals.Update(Job);
                    _context.SaveChanges();
                }
            }
           

        }
        public void AssignToEmployee()
        {
            var a = _context.DelayedGoals.Where( x => x.IsNotified== true && x.IsAdminApproved == true && (x.IsAssignedtoEmployee == false || x.IsAssignedtoEmployee == null)).FirstOrDefault();
           
            if (a != null)
            {
                var Identity = _context.EmployeeModules.Where(x => x.EmployeeId == a.EmployeeId).FirstOrDefault();
                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month, 1);
                var first = month.AddMonths(-1);
                var last = month.AddDays(-1);


                GoalVM goalVM = new GoalVM();
                goalVM.Goalname = a.Goalname;
                goalVM.GoalDescription = a.GoalDescription;
                goalVM.StartDate = a.StartDate;
                goalVM.DueDate = a.DueDate; 
                goalVM.Progress = a.Progress;   
                goalVM.Priority = a.Priority;
                goalVM.EmployeeIdentity = Identity.EmployeeIdentity;
                
               var res = AddDealyGoalForEmployee(goalVM , last );
                if(res == "Success")
                {
                    SendMailToEmployee(goalVM);
                    a.IsAssignedtoEmployee = true;
                    _context.DelayedGoals.Update(a);
                    _context.SaveChanges();
                }
            }
        }
        public string AddDealyGoalForEmployee(GoalVM model , DateTime last)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == model.EmployeeIdentity).FirstOrDefault();
            var user = _context.EmployeeModules.Where(x => x.EmployeeId == Id.EmployeeId && x.IsDeleted != true).FirstOrDefault();
            if (user != null)
            {
                GoalModule module = new GoalModule();         

                module.EmployeeId = Id.EmployeeId;
                module.Goalname = model.Goalname;
                module.GoalDescription = model.GoalDescription;
                module.StartDate = model.StartDate;
                module.DueDate = model.DueDate;
                module.Priority = model.Priority;
                module.CreatedAt = last;
                module.Progress = model.Progress;
                module.IsDeleted = false;
                module.AssingedManagerId = user.FirstLevelReportingManager;
                module.IsSubmitted = false;
                module.IsActive = true;
                module.IsCompleted = false;
                _context.GoalModules.Add(module);
                _context.SaveChanges();
                return "Success";
            }
            return "Error";
        }       
        public void DateUpdate(TimeSettingVM model)
        {
            var Egoal = _context.GoalModules.Where(x => (x.IsSubmitted != true || x.IsSubmitted == null)).ToList();
            var Mgoal = _context.GoalModules.Where(x => (x.IsCompleted != true || x.IsCompleted == null)).ToList();
            var timesetting = _context.TimeSettingTbls.FirstOrDefault(x => x.Id == 1);

            if (Egoal.Count > 0)
            {
                foreach(var e in Egoal)
                {
                    var Empreviewday = Convert.ToInt32(timesetting.EmployeeReviewDay);
                    
                    DateTime abc = e.CreatedAt;
                    var A = abc.Year;
                    var B = abc.Month + 1;
                    var C = Empreviewday;
                    var D = abc.Hour;
                    var E = abc.Minute;
                    var F = abc.Second;

                    DateTime dt2 = new DateTime(A, B, C, D, E, F);

                    if (dt2 < DateTime.Now)
                    {
                        e.IsFreezedEmp = true;
                        _context.GoalModules.Update(e);
                        _context.SaveChanges();
                    }
                    else
                    {
                        e.IsFreezedEmp = false;
                        _context.GoalModules.Update(e);
                        _context.SaveChanges();
                    }


                }
            }
            if(Mgoal.Count> 0)
            {
                foreach(var e in Mgoal)
                {
                    var Managerreviewday = Convert.ToInt32(timesetting.ManagerReviewDay);
                    

                    DateTime abc = e.CreatedAt;
                    var A = abc.Year;
                    var B = abc.Month + 1;
                    var G = Managerreviewday;
                    var D = abc.Hour;
                    var E = abc.Minute;
                    var F = abc.Second;

                    DateTime dt2 = new DateTime(A, B, G, D, E, F);

                    if (dt2 < DateTime.Now)
                    {
                        e.IsFreezedManager = true;
                        _context.GoalModules.Update(e);
                        _context.SaveChanges();
                    }
                    else
                    {
                        e.IsFreezedManager = false;
                        _context.GoalModules.Update(e);
                        _context.SaveChanges();
                    }

                }
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
