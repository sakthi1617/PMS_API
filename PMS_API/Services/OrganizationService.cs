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
                if(model.TeamId != 0)
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
        public void AccountActivateEmail(int employeeCreationResult, EmployeeVM employeeModule)
        {
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            string msg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta name=\"x-apple-disable-message-reformatting\" />\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n    <meta name=\"color-scheme\" content=\"light dark\" />\r\n    <meta name=\"supported-color-schemes\" content=\"light dark\" />\r\n    <title></title>\r\n    <style type=\"text/css\" rel=\"stylesheet\" media=\"all\">\r\n    /* Base ------------------------------ */\r\n    \r\n    @import url(\"https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap\");\r\n    body {\r\n      width: 100% !important;\r\n      height: 100%;\r\n      margin: 0;\r\n      -webkit-text-size-adjust: none;\r\n    }\r\n    \r\n    a {\r\n      color: #3869D4;\r\n    }\r\n    \r\n    a img {\r\n      border: none;\r\n    }\r\n    \r\n    td {\r\n      word-break: break-word;\r\n    }\r\n    \r\n    .preheader {\r\n      display: none !important;\r\n      visibility: hidden;\r\n      mso-hide: all;\r\n      font-size: 1px;\r\n      line-height: 1px;\r\n      max-height: 0;\r\n      max-width: 0;\r\n      opacity: 0;\r\n      overflow: hidden;\r\n    }\r\n    /* Type ------------------------------ */\r\n    \r\n    body,\r\n    td,\r\n    th {\r\n      font-family: \"Nunito Sans\", Helvetica, Arial, sans-serif;\r\n    }\r\n    \r\n    h1 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 22px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    h2 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 16px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    h3 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 14px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    td,\r\n    th {\r\n      font-size: 16px;\r\n    }\r\n    \r\n    p,\r\n    ul,\r\n    ol,\r\n    blockquote {\r\n      margin: .4em 0 1.1875em;\r\n      font-size: 16px;\r\n      line-height: 1.625;\r\n    }\r\n    \r\n    p.sub {\r\n      font-size: 13px;\r\n    }\r\n    /* Utilities ------------------------------ */\r\n    \r\n    .align-right {\r\n      text-align: right;\r\n    }\r\n    \r\n    .align-left {\r\n      text-align: left;\r\n    }\r\n    \r\n    .align-center {\r\n      text-align: center;\r\n    }\r\n    \r\n    .u-margin-bottom-none {\r\n      margin-bottom: 0;\r\n    }\r\n    /* Buttons ------------------------------ */\r\n    \r\n    .button {\r\n      background-color: #3869D4;\r\n      border-top: 10px solid #3869D4;\r\n      border-right: 18px solid #3869D4;\r\n      border-bottom: 10px solid #3869D4;\r\n      border-left: 18px solid #3869D4;\r\n      display: inline-block;\r\n      color: #FFF;\r\n      text-decoration: none;\r\n      border-radius: 3px;\r\n      box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);\r\n      -webkit-text-size-adjust: none;\r\n      box-sizing: border-box;\r\n    }\r\n    \r\n    .button--green {\r\n      background-color: #22BC66;\r\n      border-top: 10px solid #22BC66;\r\n      border-right: 18px solid #22BC66;\r\n      border-bottom: 10px solid #22BC66;\r\n      border-left: 18px solid #22BC66;\r\n    }\r\n    \r\n    .button--red {\r\n      background-color: #FF6136;\r\n      border-top: 10px solid #FF6136;\r\n      border-right: 18px solid #FF6136;\r\n      border-bottom: 10px solid #FF6136;\r\n      border-left: 18px solid #FF6136;\r\n    }\r\n    \r\n    @media only screen and (max-width: 500px) {\r\n      .button {\r\n        width: 100% !important;\r\n        text-align: center !important;\r\n      }\r\n    }\r\n    /* Attribute list ------------------------------ */\r\n    \r\n    .attributes {\r\n      margin: 0 0 21px;\r\n    }\r\n    \r\n    .attributes_content {\r\n      background-color: #F4F4F7;\r\n      padding: 16px;\r\n    }\r\n    \r\n    .attributes_item {\r\n      padding: 0;\r\n    }\r\n    /* Related Items ------------------------------ */\r\n    \r\n    .related {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 25px 0 0 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .related_item {\r\n      padding: 10px 0;\r\n      color: #CBCCCF;\r\n      font-size: 15px;\r\n      line-height: 18px;\r\n    }\r\n    \r\n    .related_item-title {\r\n      display: block;\r\n      margin: .5em 0 0;\r\n    }\r\n    \r\n    .related_item-thumb {\r\n      display: block;\r\n      padding-bottom: 10px;\r\n    }\r\n    \r\n    .related_heading {\r\n      border-top: 1px solid #CBCCCF;\r\n      text-align: center;\r\n      padding: 25px 0 10px;\r\n    }\r\n    /* Discount Code ------------------------------ */\r\n    \r\n    .discount {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 24px;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #F4F4F7;\r\n      border: 2px dashed #CBCCCF;\r\n    }\r\n    \r\n    .discount_heading {\r\n      text-align: center;\r\n    }\r\n    \r\n    .discount_body {\r\n      text-align: center;\r\n      font-size: 15px;\r\n    }\r\n    /* Social Icons ------------------------------ */\r\n    \r\n    .social {\r\n      width: auto;\r\n    }\r\n    \r\n    .social td {\r\n      padding: 0;\r\n      width: auto;\r\n    }\r\n    \r\n    .social_icon {\r\n      height: 20px;\r\n      margin: 0 8px 10px 8px;\r\n      padding: 0;\r\n    }\r\n    /* Data table ------------------------------ */\r\n    \r\n    .purchase {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 35px 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .purchase_content {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 25px 0 0 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .purchase_item {\r\n      padding: 10px 0;\r\n      color: #51545E;\r\n      font-size: 15px;\r\n      line-height: 18px;\r\n    }\r\n    \r\n    .purchase_heading {\r\n      padding-bottom: 8px;\r\n      border-bottom: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .purchase_heading p {\r\n      margin: 0;\r\n      color: #85878E;\r\n      font-size: 12px;\r\n    }\r\n    \r\n    .purchase_footer {\r\n      padding-top: 15px;\r\n      border-top: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .purchase_total {\r\n      margin: 0;\r\n      text-align: right;\r\n      font-weight: bold;\r\n      color: #333333;\r\n    }\r\n    \r\n    .purchase_total--label {\r\n      padding: 0 15px 0 0;\r\n    }\r\n    \r\n    body {\r\n      background-color: #F2F4F6;\r\n      color: #51545E;\r\n    }\r\n    \r\n    p {\r\n      color: #51545E;\r\n    }\r\n    \r\n    .email-wrapper {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #F2F4F6;\r\n    }\r\n    \r\n    .email-content {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    /* Masthead ----------------------- */\r\n    \r\n    .email-masthead {\r\n      padding: 25px 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .email-masthead_logo {\r\n      width: 94px;\r\n    }\r\n    \r\n    .email-masthead_name {\r\n      font-size: 16px;\r\n      font-weight: bold;\r\n      color: #A8AAAF;\r\n      text-decoration: none;\r\n      text-shadow: 0 1px 0 white;\r\n    }\r\n    /* Body ------------------------------ */\r\n    \r\n    .email-body {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .email-body_inner {\r\n      width: 570px;\r\n      margin: 0 auto;\r\n      padding: 0;\r\n      -premailer-width: 570px;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #FFFFFF;\r\n    }\r\n    \r\n    .email-footer {\r\n      width: 570px;\r\n      margin: 0 auto;\r\n      padding: 0;\r\n      -premailer-width: 570px;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .email-footer p {\r\n      color: #A8AAAF;\r\n    }\r\n    \r\n    .body-action {\r\n      width: 100%;\r\n      margin: 30px auto;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .body-sub {\r\n      margin-top: 25px;\r\n      padding-top: 25px;\r\n      border-top: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .content-cell {\r\n      padding: 45px;\r\n    }\r\n    /*Media Queries ------------------------------ */\r\n    \r\n    @media only screen and (max-width: 600px) {\r\n      .email-body_inner,\r\n      .email-footer {\r\n        width: 100% !important;\r\n      }\r\n    }\r\n    \r\n    @media (prefers-color-scheme: dark) {\r\n      body,\r\n      .email-body,\r\n      .email-body_inner,\r\n      .email-content,\r\n      .email-wrapper,\r\n      .email-masthead,\r\n      .email-footer {\r\n        background-color: #333333 !important;\r\n        color: #FFF !important;\r\n      }\r\n      p,\r\n      ul,\r\n      ol,\r\n      blockquote,\r\n      h1,\r\n      h2,\r\n      h3,\r\n      span,\r\n      .purchase_item {\r\n        color: #FFF !important;\r\n      }\r\n      .attributes_content,\r\n      .discount {\r\n        background-color: #222 !important;\r\n      }\r\n      .email-masthead_name {\r\n        text-shadow: none !important;\r\n      }\r\n    }\r\n    \r\n    :root {\r\n      color-scheme: light dark;\r\n      supported-color-schemes: light dark;\r\n    }\r\n    </style>\r\n    <!--[if mso]>\r\n    <style type=\"text/css\">\r\n      .f-fallback  {\r\n        font-family: Arial, sans-serif;\r\n      }\r\n    </style>\r\n  <![endif]-->\r\n  </head>\r\n  <body>\r\n    <span class=\"preheader\">Thanks for trying out [Product Name]. We’ve pulled together some information and resources to help you get started.</span>\r\n    <table class=\"email-wrapper\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n      <tr>\r\n        <td align=\"center\">\r\n          <table class=\"email-content\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n            <tr>\r\n              <td class=\"email-masthead\">\r\n                <a href=\"https://example.com\" class=\"f-fallback email-masthead_name\">\r\n                Colan Infotech\r\n              </a>\r\n              </td>\r\n            </tr>\r\n            <!-- Email Body -->\r\n            <tr>\r\n              <td class=\"email-body\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\">\r\n                <table class=\"email-body_inner\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                  <!-- Body content -->\r\n                  <tr>\r\n                    <td class=\"content-cell\">\r\n                      <div class=\"f-fallback\">\r\n                        <h1>Welcome, " + employeeModule.Name + " !</h1>\r\n                        <p>Thanks for trying [Product Name]. We’re thrilled to have you on board. To get the most out of [Product Name], do this primary next step:</p>\r\n                        <!-- Action -->\r\n                        <table class=\"body-action\" align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                          <tr>\r\n                            <td align=\"center\">\r\n                              <!-- Border based button\r\n           https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->\r\n                              <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\r\n                                <tr>\r\n                                  <td align=\"center\">\r\n                                    <a href=\"{{action_url}}\" class=\"f-fallback button\" target=\"_blank\">Do this Next</a>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                        <p>For reference, here's your login information:</p>\r\n                        <table class=\"attributes\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                          <tr>\r\n                            <td class=\"attributes_content\">\r\n                              <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                                <tr>\r\n                                  <td class=\"attributes_item\">\r\n                                    <span class=\"f-fallback\">\r\n              <strong>Login Page:</strong>  <a href=\"http://192.168.7.28:5173/reset\"  style=\" display: block;\r\n    width: 215px;\r\n    height: 46px;\r\n margin-bottom: 5px;\r\n    background: #0ac3f1;\r\n    padding: 10px;\r\n    text-align: center;\r\n    border-radius: 5px;\r\n    color: white;\r\n    font-weight: bold;\r\n    line-height: 25px;\">Generate Password</a>\r\n </span>\r\n   <span class=\"f-fallback\">\r\n  <a href=\"http://192.168.7.29:4200/auth/resetPass\"  style=\" display: block;\r\n    width: 215px;\r\n    height: 46px;\r\n    background: #e90966;\r\n margin-bottom: 5px;\r\n   padding: 10px;\r\n    text-align: center;\r\n    border-radius: 5px;\r\n    color: white;\r\n    font-weight: bold;\r\n    line-height: 25px;\">Generate Password</a></span>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td class=\"attributes_item\">\r\n                                    <span class=\"f-fallback\">\r\n              <strong>Username:</strong> {{" + employeeModule.Email + "}}\r\n            </span>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                         <p>Thanks,\r\n                          <br>Colan Infotech Private Limited</p>\r\n                        \r\n                        <!-- Sub copy -->\r\n                        <table class=\"body-sub\" role=\"presentation\">\r\n                          <tr>\r\n                            <td>\r\n                              <p class=\"f-fallback sub\">If you’re having trouble with the button above, copy and paste the URL below into your web browser.</p>\r\n                              <p class=\"f-fallback sub\">{{action_url}}</p>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                      </div>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td>\r\n                <table class=\"email-footer\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                  <tr>\r\n                    <td class=\"content-cell\" align=\"center\">\r\n                      <p class=\"f-fallback sub align-center\">\r\n                        Colan Infotech Private Limited\r\n                      </p>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>";
            var message = new Message(new string[] { employeeModule.Email }, "Welcome To PMS", msg.ToString(), null, null);
            _emailservice.SendEmail(message);
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
            Designation designation1 = new Designation();
            designation1.DepartmentId= model.DepartmentId;
            designation1.DesignationName= model.DesignationName;
            designation1.AddTime = DateTime.Now;
            _context.Designations.Add(designation1);
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
                    list.TeamId= item.TeamId;
                    list.TeamName= item.TeamName;
                    teams.Add(list);
                }
                return teams;
            
        }
        public List<getDesignation> GetDesignation(int DepartmentID)
        {
            List<getDesignation> designation = new List<getDesignation>();
            var a = _context.Designations.Where(x => x.DepartmentId == DepartmentID).ToList();

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
                var message = new Message(new string[] { job_1.FirstlvlManagerMail }, "Approval Message", msg.ToString(),null,null);
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
                var message = new Message(new string[] { job_2.FirstlvlManagerMail }, "Approval Message", msg.ToString(), null,null);
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
                var message = new Message(new string[] { job_3.Employeemail }, "Skill Updated", msg.ToString(), null,null);
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

        public dynamic InactiveEmployeeList()
        {
            List<TestEmployeeList> testlist = new List<TestEmployeeList>();
            TestEmployeeVM testemp = new TestEmployeeVM();

            var report1 = _context.EmployeeModules.Where(s => s.IsDeleted == false && s.IsActivated != true).ToList();
          
            foreach (var report in report1)
            {
                TestEmployeeList test = new TestEmployeeList();
                var secondmanager = report1.Where(s => s.SecondLevelReportingManager == report.SecondLevelReportingManager).ToList();
                test.FirstLevelReportingManager = report.FirstLevelReportingManager;
                test.FirstLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.FirstLevelReportingManager).Name;
                test.SecondLevelReportingManager = report.SecondLevelReportingManager;
                test.SecondLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.SecondLevelReportingManager).Name;
                testemp = new TestEmployeeVM();
                testemp.EmployeeId = report.EmployeeId;
                testemp.Name = report.Name;
                testemp.Age = report.Age;
                testemp.RoleId= report.RoleId;
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


        public dynamic EmployeesinNoticePeriod()
        {
            List<TestEmployeeList> testlist = new List<TestEmployeeList>();
            TestEmployeeVM testemp = new TestEmployeeVM();

            var report1 = _context.EmployeeModules.Where(x =>x.IsDeleted != true && x.IsActivated ==true && x.InNoticePeriod == true).ToList();

            foreach (var report in report1)
            {
                TestEmployeeList test = new TestEmployeeList();
                var secondmanager = report1.Where(s => s.SecondLevelReportingManager == report.SecondLevelReportingManager).ToList();
                test.FirstLevelReportingManager = report.FirstLevelReportingManager;
                test.FirstLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.FirstLevelReportingManager).Name;
                test.SecondLevelReportingManager = report.SecondLevelReportingManager;
                test.SecondLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.SecondLevelReportingManager).Name;
                testemp = new TestEmployeeVM();
                testemp.EmployeeId = report.EmployeeId;
                testemp.Name = report.Name;
                testemp.Age = report.Age;
                testemp.DateOfBirth = report.DateOfBirth;
                testemp.RoleId = report.RoleId;
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

        public dynamic ResignedEmployeeList()
        {
            List<TestEmployeeList> testlist = new List<TestEmployeeList>();
            TestEmployeeVM testemp = new TestEmployeeVM();

            var report1 = _context.EmployeeModules.Where(x => x.IsResigned == true).ToList();

            foreach (var report in report1)
            {
                TestEmployeeList test = new TestEmployeeList();
                var secondmanager = report1.Where(s => s.SecondLevelReportingManager == report.SecondLevelReportingManager).ToList();
                test.FirstLevelReportingManager = report.FirstLevelReportingManager;
                test.FirstLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.FirstLevelReportingManager).Name;
                test.SecondLevelReportingManager = report.SecondLevelReportingManager;
                test.SecondLevelReportingManagerName = _context.EmployeeModules.First(w => w.EmployeeId == report.SecondLevelReportingManager).Name;
                testemp = new TestEmployeeVM();
                testemp.EmployeeId = report.EmployeeId;
                testemp.Name = report.Name;
                testemp.RoleId = report.RoleId;
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
        public dynamic EmployeeListByStages(int potential , int performance)
        {
            List<TestEmployeeList> testlist = new List<TestEmployeeList>();
            TestEmployeeVM testemp = new TestEmployeeVM();

            var report1 = _context.EmployeeModules.Where(s => s.IsDeleted == false && s.IsActivated == true && s.PotentialStage == potential && s.PerformanceStage == performance).ToList();
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
        public NineStage nineStages()
        {
            var ConsistanceStar = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 1 && x.PotentialStage == 1).Count();
            var FutureStar = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 1 && x.PotentialStage == 2).Count();
            var RoughDiamond = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 1 && x.PotentialStage == 3).Count();
            var CurrentStar = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 2 && x.PotentialStage == 1).Count();
            var KeyPlayer = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 2 && x.PotentialStage == 2).Count();
            var InconsistentPlayer = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 2 && x.PotentialStage == 3).Count();
            var HighProfessional = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 3 && x.PotentialStage == 1).Count();
            var SolidProfessional = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 3 && x.PotentialStage == 2).Count();
            var TalentRisk = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.PerformanceStage == 3 && x.PotentialStage == 3).Count();

            NineStage nine = new NineStage();
            nine.ConsistanceStar = ConsistanceStar;
            nine.FutureStar = FutureStar;
            nine.RoughDiamond = RoughDiamond;
            nine.CurrentStar = CurrentStar;
            nine.KeyPlayer = KeyPlayer;
            nine.InconsistentPlayer = InconsistentPlayer;
            nine.HighProfessional = HighProfessional;
            nine.SolidProfessional = SolidProfessional;
            nine.TalentRisk = TalentRisk;

            return nine;
        }
        public void AdminRatingApprove(string EmployeeIdentity, bool approvel)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity && x.IsActivated == true && x.IsDeleted != true).FirstOrDefault();
            if (Id != null)
            {
                if (approvel == true)
                {
                    Id.IsWantToPublish = true;
                }
                else
                {
                    Id.IsWantToPublish = false;
                }

                _context.EmployeeModules.Update(Id);
                _context.SaveChanges();
            }
        }
        public void AcceptRating(string EmployeeIdentity, bool approvel)
        {
            EmployeeModule module = new EmployeeModule();
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity && x.IsActivated == true && x.IsDeleted != true && x.IsWantToPublish == true).FirstOrDefault();
            if(Id!= null)
            {
                if (approvel == true)
                {
                    Id.RatingIsaccepted = true;
                }
                else
                {
                    Id.RatingIsaccepted = false;
                }
               
                _context.EmployeeModules.Update(Id);
                _context.SaveChanges();
            }       
                        
        }
        public dynamic AcceptedEmployeeList()
        {
            List<SimpleEmployeeListVM> EmpList = new List<SimpleEmployeeListVM>();
            var a = _context.EmployeeModules.Where(x => x.IsActivated == true && x.IsDeleted != true && x.IsPublished == true && x.RatingIsaccepted == true).ToList();

            foreach (var item in a)
            {
                SimpleEmployeeListVM list = new SimpleEmployeeListVM();
                list.EmployeeIdentity = item.EmployeeIdentity;
                list.Name = item.Name;
                list.DepartmentId = item.DepartmentId;
                list.DesignationId = item.DesignationId;
                list.DesignationId = item.TeamId;
                list.Email = item.Email;
                EmpList.Add(list);
            }
            return EmpList;
        }
        public dynamic RejectedEmployeeList()
        {
            List<SimpleEmployeeListVM> EmpList = new List<SimpleEmployeeListVM>();
            var a = _context.EmployeeModules.Where(x => x.IsActivated == true && x.IsDeleted != true && x.IsPublished == true && x.RatingIsaccepted == false).ToList();

            foreach (var item in a)
            {
                SimpleEmployeeListVM list = new SimpleEmployeeListVM();
                list.EmployeeIdentity = item.EmployeeIdentity;
                list.Name = item.Name;
                list.DepartmentId = item.DepartmentId;
                list.DesignationId = item.DesignationId;
                list.DesignationId = item.TeamId;
                list.Email = item.Email;
                EmpList.Add(list);
            }
            return EmpList;
        }
        public string SalaryIncrement(string EmployeeIdentity, decimal incrementPercentage)
        {
            var Id = _context.EmployeeModules.Where(x => x.EmployeeIdentity == EmployeeIdentity && x.IsActivated == true && x.IsDeleted != true).FirstOrDefault();
            if(Id != null)
            {
                decimal oldSalary = (decimal)Id.Salary;
                decimal newSalary = 0;

                var IncrementPercentage = incrementPercentage/ 100;
                var IncrementAmount = oldSalary * IncrementPercentage;
                newSalary = oldSalary + IncrementAmount;

                Id.Salary = newSalary;
                _context.EmployeeModules.Update(Id);    
                _context.SaveChanges();
                return "Employee Salary Upadted";
            }
            return "Employee Not Found";
        }
        public void annualRatingPublish(bool approvel)
        {
            if(approvel == true)
            {
                var id = _context.EmployeeModules.Where(x => x.IsDeleted != true && x.IsActivated == true && x.IsWantToPublish == true ).ToList();
                if(id.Count > 0)
                {
                    foreach(var item in id)
                    {
                        var allrate = _context.MonthwiseRatings.Where(x => x.EmployeeId== item.EmployeeId && x.CalculatedAt.Value.Year == DateTime.Now.Year).FirstOrDefault();
                        var msg = " Hi " + item.Name + " Anuual Rating " + DateTime.Now.Year + " Was Published " + "</br>" + " Your 'potential stage = " + item.PotentialStage + "' and Your 'performace stage = " + item.PerformanceStage + "'  Improve your Potential and Performance to Next Level " + "Your Month wise ratings are below : " + "January : " + allrate.January  
                                                                                                                                                                                                                                                                                                                                                 + "February: " + allrate.February
                                                                                                                                                                                                                                                                                                                                                 + "March: " + allrate.March
                                                                                                                                                                                                                                                                                                                                                 + "April: " + allrate.April
                                                                                                                                                                                                                                                                                                                                                 + "May: " + allrate.May
                                                                                                                                                                                                                                                                                                                                                 + "June: " + allrate.June
                                                                                                                                                                                                                                                                                                                                                 + "July: " + allrate.July
                                                                                                                                                                                                                                                                                                                                                 + "August: " + allrate.August
                                                                                                                                                                                                                                                                                                                                                 + "September: " + allrate.September
                                                                                                                                                                                                                                                                                                                                                 + "October: " + allrate.October
                                                                                                                                                                                                                                                                                                                                                 + "November: " + allrate.November
                                                                                                                                                                                                                                                                                                                                                 + "December: " + allrate.December;
                        var message = new Message(new string[] { item.Email }, "Rating Cycle Notification", msg.ToString(), null,null);
                        var a = _emailservice.SendEmail(message);
                        if(a == "ok")
                        {
                            item.IsPublished = true;
                            _context.EmployeeModules.Update(item);
                            _context.SaveChanges();
                        }
                    }
                }
               
            }
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