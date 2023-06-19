using Hangfire.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.LogHandling;
using PMS_API.Models;
using PMS_API.Reponse;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Linq;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace PMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepo repository;
        private readonly IEmailService _emailservice;
        private readonly PMSContext _context;
        public OrganizationController(IOrganizationRepo _repository, IEmailService emailservice, PMSContext context)
        {
            repository = _repository;
            _emailservice = emailservice;
            _context = context;
        }

        #region Adding Employee which was access only by Admin
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> addEmployee([FromBody]EmployeeVM employeeModule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employeeCreationResult = repository.AddEmployee(employeeModule);

                    if (employeeCreationResult != 0 && employeeCreationResult != null)
                    {

                        var userLevelResult = repository.AddUserLevel(employeeCreationResult,employeeModule.DepartmentId,employeeModule.DesignationId,employeeModule.TeamId);
                        if (userLevelResult == "Created")
                        {
                            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
                            string msg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta name=\"x-apple-disable-message-reformatting\" />\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n    <meta name=\"color-scheme\" content=\"light dark\" />\r\n    <meta name=\"supported-color-schemes\" content=\"light dark\" />\r\n    <title></title>\r\n    <style type=\"text/css\" rel=\"stylesheet\" media=\"all\">\r\n    /* Base ------------------------------ */\r\n    \r\n    @import url(\"https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap\");\r\n    body {\r\n      width: 100% !important;\r\n      height: 100%;\r\n      margin: 0;\r\n      -webkit-text-size-adjust: none;\r\n    }\r\n    \r\n    a {\r\n      color: #3869D4;\r\n    }\r\n    \r\n    a img {\r\n      border: none;\r\n    }\r\n    \r\n    td {\r\n      word-break: break-word;\r\n    }\r\n    \r\n    .preheader {\r\n      display: none !important;\r\n      visibility: hidden;\r\n      mso-hide: all;\r\n      font-size: 1px;\r\n      line-height: 1px;\r\n      max-height: 0;\r\n      max-width: 0;\r\n      opacity: 0;\r\n      overflow: hidden;\r\n    }\r\n    /* Type ------------------------------ */\r\n    \r\n    body,\r\n    td,\r\n    th {\r\n      font-family: \"Nunito Sans\", Helvetica, Arial, sans-serif;\r\n    }\r\n    \r\n    h1 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 22px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    h2 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 16px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    h3 {\r\n      margin-top: 0;\r\n      color: #333333;\r\n      font-size: 14px;\r\n      font-weight: bold;\r\n      text-align: left;\r\n    }\r\n    \r\n    td,\r\n    th {\r\n      font-size: 16px;\r\n    }\r\n    \r\n    p,\r\n    ul,\r\n    ol,\r\n    blockquote {\r\n      margin: .4em 0 1.1875em;\r\n      font-size: 16px;\r\n      line-height: 1.625;\r\n    }\r\n    \r\n    p.sub {\r\n      font-size: 13px;\r\n    }\r\n    /* Utilities ------------------------------ */\r\n    \r\n    .align-right {\r\n      text-align: right;\r\n    }\r\n    \r\n    .align-left {\r\n      text-align: left;\r\n    }\r\n    \r\n    .align-center {\r\n      text-align: center;\r\n    }\r\n    \r\n    .u-margin-bottom-none {\r\n      margin-bottom: 0;\r\n    }\r\n    /* Buttons ------------------------------ */\r\n    \r\n    .button {\r\n      background-color: #3869D4;\r\n      border-top: 10px solid #3869D4;\r\n      border-right: 18px solid #3869D4;\r\n      border-bottom: 10px solid #3869D4;\r\n      border-left: 18px solid #3869D4;\r\n      display: inline-block;\r\n      color: #FFF;\r\n      text-decoration: none;\r\n      border-radius: 3px;\r\n      box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);\r\n      -webkit-text-size-adjust: none;\r\n      box-sizing: border-box;\r\n    }\r\n    \r\n    .button--green {\r\n      background-color: #22BC66;\r\n      border-top: 10px solid #22BC66;\r\n      border-right: 18px solid #22BC66;\r\n      border-bottom: 10px solid #22BC66;\r\n      border-left: 18px solid #22BC66;\r\n    }\r\n    \r\n    .button--red {\r\n      background-color: #FF6136;\r\n      border-top: 10px solid #FF6136;\r\n      border-right: 18px solid #FF6136;\r\n      border-bottom: 10px solid #FF6136;\r\n      border-left: 18px solid #FF6136;\r\n    }\r\n    \r\n    @media only screen and (max-width: 500px) {\r\n      .button {\r\n        width: 100% !important;\r\n        text-align: center !important;\r\n      }\r\n    }\r\n    /* Attribute list ------------------------------ */\r\n    \r\n    .attributes {\r\n      margin: 0 0 21px;\r\n    }\r\n    \r\n    .attributes_content {\r\n      background-color: #F4F4F7;\r\n      padding: 16px;\r\n    }\r\n    \r\n    .attributes_item {\r\n      padding: 0;\r\n    }\r\n    /* Related Items ------------------------------ */\r\n    \r\n    .related {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 25px 0 0 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .related_item {\r\n      padding: 10px 0;\r\n      color: #CBCCCF;\r\n      font-size: 15px;\r\n      line-height: 18px;\r\n    }\r\n    \r\n    .related_item-title {\r\n      display: block;\r\n      margin: .5em 0 0;\r\n    }\r\n    \r\n    .related_item-thumb {\r\n      display: block;\r\n      padding-bottom: 10px;\r\n    }\r\n    \r\n    .related_heading {\r\n      border-top: 1px solid #CBCCCF;\r\n      text-align: center;\r\n      padding: 25px 0 10px;\r\n    }\r\n    /* Discount Code ------------------------------ */\r\n    \r\n    .discount {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 24px;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #F4F4F7;\r\n      border: 2px dashed #CBCCCF;\r\n    }\r\n    \r\n    .discount_heading {\r\n      text-align: center;\r\n    }\r\n    \r\n    .discount_body {\r\n      text-align: center;\r\n      font-size: 15px;\r\n    }\r\n    /* Social Icons ------------------------------ */\r\n    \r\n    .social {\r\n      width: auto;\r\n    }\r\n    \r\n    .social td {\r\n      padding: 0;\r\n      width: auto;\r\n    }\r\n    \r\n    .social_icon {\r\n      height: 20px;\r\n      margin: 0 8px 10px 8px;\r\n      padding: 0;\r\n    }\r\n    /* Data table ------------------------------ */\r\n    \r\n    .purchase {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 35px 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .purchase_content {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 25px 0 0 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .purchase_item {\r\n      padding: 10px 0;\r\n      color: #51545E;\r\n      font-size: 15px;\r\n      line-height: 18px;\r\n    }\r\n    \r\n    .purchase_heading {\r\n      padding-bottom: 8px;\r\n      border-bottom: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .purchase_heading p {\r\n      margin: 0;\r\n      color: #85878E;\r\n      font-size: 12px;\r\n    }\r\n    \r\n    .purchase_footer {\r\n      padding-top: 15px;\r\n      border-top: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .purchase_total {\r\n      margin: 0;\r\n      text-align: right;\r\n      font-weight: bold;\r\n      color: #333333;\r\n    }\r\n    \r\n    .purchase_total--label {\r\n      padding: 0 15px 0 0;\r\n    }\r\n    \r\n    body {\r\n      background-color: #F2F4F6;\r\n      color: #51545E;\r\n    }\r\n    \r\n    p {\r\n      color: #51545E;\r\n    }\r\n    \r\n    .email-wrapper {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #F2F4F6;\r\n    }\r\n    \r\n    .email-content {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    /* Masthead ----------------------- */\r\n    \r\n    .email-masthead {\r\n      padding: 25px 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .email-masthead_logo {\r\n      width: 94px;\r\n    }\r\n    \r\n    .email-masthead_name {\r\n      font-size: 16px;\r\n      font-weight: bold;\r\n      color: #A8AAAF;\r\n      text-decoration: none;\r\n      text-shadow: 0 1px 0 white;\r\n    }\r\n    /* Body ------------------------------ */\r\n    \r\n    .email-body {\r\n      width: 100%;\r\n      margin: 0;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n    }\r\n    \r\n    .email-body_inner {\r\n      width: 570px;\r\n      margin: 0 auto;\r\n      padding: 0;\r\n      -premailer-width: 570px;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      background-color: #FFFFFF;\r\n    }\r\n    \r\n    .email-footer {\r\n      width: 570px;\r\n      margin: 0 auto;\r\n      padding: 0;\r\n      -premailer-width: 570px;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .email-footer p {\r\n      color: #A8AAAF;\r\n    }\r\n    \r\n    .body-action {\r\n      width: 100%;\r\n      margin: 30px auto;\r\n      padding: 0;\r\n      -premailer-width: 100%;\r\n      -premailer-cellpadding: 0;\r\n      -premailer-cellspacing: 0;\r\n      text-align: center;\r\n    }\r\n    \r\n    .body-sub {\r\n      margin-top: 25px;\r\n      padding-top: 25px;\r\n      border-top: 1px solid #EAEAEC;\r\n    }\r\n    \r\n    .content-cell {\r\n      padding: 45px;\r\n    }\r\n    /*Media Queries ------------------------------ */\r\n    \r\n    @media only screen and (max-width: 600px) {\r\n      .email-body_inner,\r\n      .email-footer {\r\n        width: 100% !important;\r\n      }\r\n    }\r\n    \r\n    @media (prefers-color-scheme: dark) {\r\n      body,\r\n      .email-body,\r\n      .email-body_inner,\r\n      .email-content,\r\n      .email-wrapper,\r\n      .email-masthead,\r\n      .email-footer {\r\n        background-color: #333333 !important;\r\n        color: #FFF !important;\r\n      }\r\n      p,\r\n      ul,\r\n      ol,\r\n      blockquote,\r\n      h1,\r\n      h2,\r\n      h3,\r\n      span,\r\n      .purchase_item {\r\n        color: #FFF !important;\r\n      }\r\n      .attributes_content,\r\n      .discount {\r\n        background-color: #222 !important;\r\n      }\r\n      .email-masthead_name {\r\n        text-shadow: none !important;\r\n      }\r\n    }\r\n    \r\n    :root {\r\n      color-scheme: light dark;\r\n      supported-color-schemes: light dark;\r\n    }\r\n    </style>\r\n    <!--[if mso]>\r\n    <style type=\"text/css\">\r\n      .f-fallback  {\r\n        font-family: Arial, sans-serif;\r\n      }\r\n    </style>\r\n  <![endif]-->\r\n  </head>\r\n  <body>\r\n    <span class=\"preheader\">Thanks for trying out [Product Name]. We’ve pulled together some information and resources to help you get started.</span>\r\n    <table class=\"email-wrapper\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n      <tr>\r\n        <td align=\"center\">\r\n          <table class=\"email-content\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n            <tr>\r\n              <td class=\"email-masthead\">\r\n                <a href=\"https://example.com\" class=\"f-fallback email-masthead_name\">\r\n                Colan Infotech\r\n              </a>\r\n              </td>\r\n            </tr>\r\n            <!-- Email Body -->\r\n            <tr>\r\n              <td class=\"email-body\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\">\r\n                <table class=\"email-body_inner\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                  <!-- Body content -->\r\n                  <tr>\r\n                    <td class=\"content-cell\">\r\n                      <div class=\"f-fallback\">\r\n                        <h1>Welcome, {"+employeeCreationResult+"}!</h1>\r\n                        <p>Thanks for trying [Product Name]. We’re thrilled to have you on board. To get the most out of [Product Name], do this primary next step:</p>\r\n                        <!-- Action -->\r\n                        <table class=\"body-action\" align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                          <tr>\r\n                            <td align=\"center\">\r\n                              <!-- Border based button\r\n           https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->\r\n                              <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\r\n                                <tr>\r\n                                  <td align=\"center\">\r\n                                    <a href=\"{{action_url}}\" class=\"f-fallback button\" target=\"_blank\">Do this Next</a>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                        <p>For reference, here's your login information:</p>\r\n                        <table class=\"attributes\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                          <tr>\r\n                            <td class=\"attributes_content\">\r\n                              <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                                <tr>\r\n                                  <td class=\"attributes_item\">\r\n                                    <span class=\"f-fallback\">\r\n              <strong>Login Page:</strong> {{"+ "http://192.168.7.43:8888/api/OrganizationAuth/GeneratetPassword?Email="+employeeModule.Email + "}}\r\n            </span>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td class=\"attributes_item\">\r\n                                    <span class=\"f-fallback\">\r\n              <strong>Username:</strong> {{"+employeeCreationResult+"}}\r\n            </span>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                         <p>Thanks,\r\n                          <br>Colan Infotech Private Limited</p>\r\n                        \r\n                        <!-- Sub copy -->\r\n                        <table class=\"body-sub\" role=\"presentation\">\r\n                          <tr>\r\n                            <td>\r\n                              <p class=\"f-fallback sub\">If you’re having trouble with the button above, copy and paste the URL below into your web browser.</p>\r\n                              <p class=\"f-fallback sub\">{{action_url}}</p>\r\n                            </td>\r\n                          </tr>\r\n                        </table>\r\n                      </div>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n            <tr>\r\n              <td>\r\n                <table class=\"email-footer\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                  <tr>\r\n                    <td class=\"content-cell\" align=\"center\">\r\n                      <p class=\"f-fallback sub align-center\">\r\n                        Colan Infotech Private Limited\r\n                      </p>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </td>\r\n            </tr>\r\n          </table>\r\n        </td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>";
                            var message = new Message(new string[] { employeeModule.Email }, "Welcome To PMS", msg.ToString(), null,null);
                            _emailservice.SendEmail(message);

                            return StatusCode(StatusCodes.Status201Created,
                            new ResponseStatus { status = "Success", message = "Employee Added Successfully.", statusCode = StatusCodes.Status201Created });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest,
                              new ResponseStatus { status = "Error", message = "Something Error" , statusCode = StatusCodes.Status400BadRequest });
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                   new ResponseStatus { status = "Error", message = "User Already Exists" , statusCode = StatusCodes.Status400BadRequest });
                    }

                }
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Adding Department which was access only by Admin
        [HttpPost]
        [Route("AddDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(DepartmentVM department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddDepartment(department);
                    repository.Save();
                    return StatusCode(StatusCodes.Status201Created,
                       new ResponseStatus { status = "Success", message = "Department Added Successfully", statusCode = StatusCodes.Status201Created });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                   new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion


        #region AddDesignations which was access only by Admin
        [HttpPost]
        [Route("AddDesignations")]
        public async Task<IActionResult> AddDesignations(Designation1VM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddDesignations(model);
                    repository.Save();
                    return StatusCode(StatusCodes.Status201Created,
                      new ResponseStatus { status = "Success", message = "Designation Added Successfully", statusCode = StatusCodes.Status201Created });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                 new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Add Team
        [HttpPost]
        [Route("AddTeam")]
        public async Task<IActionResult> AddTeam(TeamVM team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddTeam(team);
                    return StatusCode(StatusCodes.Status201Created,
                      new ResponseStatus { status = "Success", message = "Team Added Successfully", statusCode = StatusCodes.Status201Created });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                 new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Get Team
        [HttpGet]
        [Route("GetTeam")]
        public async Task<IActionResult> GetTeam (int DepartmentID)
        {
            try
            {
                var a = repository.GetTeam(DepartmentID);
                return Ok(new
                {

                    list = a,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Skill List.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }

        }
        #endregion

        #region Get Designation
        [HttpGet]
        [Route("GetDesignation")]
        public async Task<IActionResult> GetDesignation(int DepartmentID)
        {
            try
            {
                var a = repository.GetDesignation(DepartmentID);
                return Ok(new
                {

                    list = a,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Skill List.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }

        }
        #endregion

        #region Updating EMployee which wass access only by Admin
        [HttpPut]
        [Route("UpdateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(string EmployeeIdentity, EmployeeVM employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string a = repository.UpdateEmployee(EmployeeIdentity, employee);
                    switch (a)
                    {
                        case "Updated":
                            return StatusCode(StatusCodes.Status201Created,
                            new ResponseStatus { status = "Success", message = "Employee Details Updated Successfully." , statusCode= StatusCodes.Status201Created });

                        case "User Not Exists":
                            return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Not Found", message = "User Not Exists" , statusCode= StatusCodes.Status404NotFound });
                    }
                }

                return StatusCode(StatusCodes.Status404NotFound,
                       new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode= StatusCodes.Status404NotFound });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }

        }
        #endregion

        #region Updating Department which wass access only by Admin
        [HttpPut]
        [Route("UpdateDepertment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepertment(int id, DepartmentVM department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string a = repository.UpdateDepertment(id, department);

                    switch (a)
                    {

                        case "Updated":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                               new ResponseStatus { status = "Success", message = "Department Updated Successfully", statusCode = StatusCodes.Status201Created });

                        case "Department Not Exists":

                            return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Success", message = "Department Not Exists",statusCode= StatusCodes.Status404NotFound });
                    }

                }
                return StatusCode(StatusCodes.Status400BadRequest,
                   new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }

        }
        #endregion

        #region Updating Designation which was access only by Admin
        [HttpPut]
        [Route("UpdateDesignation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDesignation(int id, DesignationVM designation)
        {
            try
            {

                if (ModelState.IsValid)

                {
                    string a = repository.UpdateDesignation(id, designation);

                    switch (a)
                    {
                        case "Updated":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                              new ResponseStatus { status = "Success", message = "Designation Updated Successfully" , statusCode = StatusCodes.Status201Created });

                        case "Designation Not Exists":

                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Department Not Exists", statusCode = StatusCodes.Status404NotFound });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                       new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Listing EmployeeDetail which was access by all
        [HttpGet]
        [Route("EmployeeModule")]
       [Authorize(Roles = "Admin,User")]
        #region OldEmployeeModule
        //public async Task<IActionResult> EmployeeModule()
        //{
        //    try
        //    {
        //        var employeeList = repository.EmployeeList().ToList();
        //        return Ok(new
        //        {

        //            list = employeeList,
        //            ResponseStatus = new ResponseStatus { status = "Success", message = "Employee List.", statusCode = StatusCodes.Status200OK }
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //} 
        #endregion
        public async Task<IActionResult> EmployeeModule()
        {
            try
            {

                var result = repository.EmployeeList();


                return Ok(new SuccessResponse<object>
                {

                    ModelData = new
                    {
                        
                        EMployeeDetails = result
                    },
                    statusCode = "200",
                    Response = "ok"

                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion
        [HttpGet]
        [Route("EmployeeHierachy")]
        public async Task<IActionResult> EmployeeHierachy(int employeeId)
        {
            try
            {
                var result = repository.EmployeeHierachy(employeeId);
                return Ok(new SuccessResponse<object>
                {
                    ModelData = new
                    {
                        Managers = result
                    }
                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }

        }


        #region Listing EmployeeDetail by Id which wass access by all
        [HttpGet]
        [Route("EmployeeById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> EmployeeById(string EmployeeIdentity)
        {
            try
            {
                var EmployeeId = repository.EmployeeById(EmployeeIdentity);
                if (EmployeeId == null)
                {
                    return NotFound(new
                    {        
                        ResponseStatus = new ResponseStatus { status = "Error", message = "User Data Unavailable", statusCode = StatusCodes.Status404NotFound }
                    });
                }
                return Ok(new
                {

                    list = EmployeeId,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Employee Detail.", statusCode = StatusCodes.Status200OK }
                }); 
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion
       
        #region Listing Employee by Department which was access by all
        [HttpGet]
        [Route("GetEmployeeByDepartment")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeByDepartment(int Id)
        {
            try
            {
                var EmpById = repository.EmployeeByDepartment(Id).ToList();
                if (EmpById.Count > 0)
                {
                    return Ok(new
                    {
                        list = EmpById,
                        ResponseStatus = new ResponseStatus { status = "Success", message = "Employee List.", statusCode = StatusCodes.Status200OK }
                    });
                }
                return NotFound(new
                {
                    ResponseStatus = new ResponseStatus { status = "Error", message = "Employee not found.", statusCode = StatusCodes.Status404NotFound }

                });
               
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Listing Department which was access by all
        [HttpGet]
        [Route("DepartmentModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DepartmentModule()
        {
            try
            {
                var departmentlist = repository.DepartmentModule().ToList();
                return Ok(new
                {
                    list = departmentlist,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Department List.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Listing Designation which was access by all
        [HttpGet]
        [Route("DesignationModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DesignationModule()
        {
            try
            {
                var desig = repository.DesignationModule().ToList();
                return Ok(new
                {

                    list = desig,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Designation List.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Deleting Employee which wass access only by Admin
        [HttpDelete]
        [Route("DeleteEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(string EmployeeIdentity)
        {
            try
            {
                var a = repository.DeleteEmployee(EmployeeIdentity);

                switch (a)
                {
                    case "Deleted":
                        repository.Save();
                        return StatusCode(StatusCodes.Status200OK,
                            new ResponseStatus { status = "Success", message = "Employee Details Deleted SuccessFully" ,statusCode= StatusCodes.Status200OK });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Error", message = "Employee not found", statusCode= StatusCodes.Status404NotFound });

                }
                return StatusCode(StatusCodes.Status404NotFound,
                              new ResponseStatus { status = "Error", message = "Something Error",statusCode= StatusCodes.Status404NotFound });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region FindRequiredEmployee which was access only by Admin
        [HttpGet]
        [Route("FindRequiredEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindRequiredEmployee([FromQuery] FindEmployee find)
        {
            try
            {
                var emplist = repository.FindRequiredEmployee(find);
                return Ok(new
                {

                    list = emplist,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Employee List.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region EmployeeCountByStages
        [HttpGet]
        [Route("EmployeeCountByStages")]
        public async Task<IActionResult> EmployeeCountByStages()
        {
            try
            {
                var counts = repository.nineStages();
                if (counts == null)
                {
                    return NotFound(new
                    {
                        ResponseStatus = new ResponseStatus { status = "Error", message = "Data Unavailable", statusCode = StatusCodes.Status404NotFound }
                    });
                }
                return Ok(new
                {

                    list = counts,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Employee Counts.", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region EmployeeListByStages
        [HttpGet]
        [Route("EmployeeListByStages")]
        public async Task<IActionResult> EmployeeListByStages(int performancestage , int potentialstage)
        {
            try
            {

                var result = repository.EmployeeListByStages(performancestage,potentialstage);


                return Ok(new SuccessResponse<object>
                {

                    ModelData = new
                    {

                        EMployeeDetails = result
                    },
                    statusCode = "200",
                    Response = "ok"

                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Get Reporting person
        [HttpGet]
        [Route("GetReportingPerson")]
        public async Task<IActionResult> GetReportingPerson()
        {
            try
            {
                var result = repository.GetReportingPerson();
                return Ok(new
                {

                    list = result,
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Reporting Person List", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Include Exclude Ratings
        [HttpPost]
        [Route("AdminRatingApprove")]
        public async Task<IActionResult> AdminRatingApprove(string EmployeeIdentity , bool approvel)
        {
            try
            {
                repository.AdminRatingApprove(EmployeeIdentity,approvel);
                return Ok(new
                {
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Response Updated", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Anual Rating Publish
        [HttpPost]
        [Route("annualRatingPublish")]
        public async Task<IActionResult> annualRatingPublish(bool approvel)
        {
            try
            {
                repository.annualRatingPublish(approvel);
                return Ok(new
                {
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Annual Ratings are Published", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Rating Accept by Employees
        [HttpPost]
        [Route("AcceptRating")]
        public async Task<IActionResult> AcceptRating ( string EmployeeIdentity , bool approvel)
        {
            try
            {
                repository.AcceptRating(EmployeeIdentity,approvel);
                return Ok(new
                {
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Responce Updated", statusCode = StatusCodes.Status200OK }
                });
            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Employeelist who Accept the Annual Rating
        [HttpGet]
        [Route("AcceptedEmployeeList")]
        public async Task<IActionResult> AcceptedEmployeeList()
        {
            try
            {

                var result = repository.AcceptedEmployeeList();


                return Ok(new SuccessResponse<object>
                {

                    ModelData = new
                    {

                        EMployeeDetails = result
                    },
                    statusCode = "200",
                    Response = "ok"

                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Employeelist who Accept the Annual Rating
        [HttpGet]
        [Route("RejectedEmployeeList")]
        public async Task<IActionResult> RejectedEmployeeList()
        {
            try
            {

                var result = repository.RejectedEmployeeList();


                return Ok(new SuccessResponse<object>
                {

                    ModelData = new
                    {

                        EMployeeDetails = result
                    },
                    statusCode = "200",
                    Response = "ok"

                });
            }
            catch (Exception ex)
            {

                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion

        #region Salary Increment
        [HttpPost]
        [Route("SalaryIncrement")]
        public async Task<IActionResult> SalaryIncrement(string EmployeeIdentity , decimal incrementPercentage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.SalaryIncrement(EmployeeIdentity, incrementPercentage);
                    switch (a)
                    {
                        case "Employee Salary Upadted":
                            return StatusCode(StatusCodes.Status201Created,
                          new ResponseStatus { status = "Success", message = "Employee Salary Upadted", statusCode = StatusCodes.Status201Created });
                        case "Employee Not Found":
                            return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Employee Not Found", statusCode = StatusCodes.Status404NotFound });
                    }
                }
                return StatusCode(StatusCodes.Status404NotFound,
                        new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status404NotFound });

            }
            catch (Exception ex)
            {
                ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
                return BadRequest(new FailureResponse<object>
                {
                    Error = ex.Message,
                    IsreponseSuccess = false
                });
            }
        }
        #endregion
        #region Adding Designation which was access only by Admin
        //[HttpPost]
        //[Route("AddDesignation")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddDesignation(DesignationVM designation)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            repository.AddDesignation(designation);
        //            repository.Save();
        //            return StatusCode(StatusCodes.Status201Created,
        //              new ResponseStatus { status = "Success", message = "Designation Added Successfully" , statusCode = StatusCodes.Status201Created });
        //        }
        //        return StatusCode(StatusCodes.Status400BadRequest,
        //         new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
        //    }
        //    catch (Exception ex)
        //    {

        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //}
        #endregion
        #region Adding skillWeightage which was access only by Admin
        //[HttpPost]
        //[Route("AddSkillWeightage")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddSkillWeightage(WeightageVM weightage)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            repository.AddSkillWeightage(weightage);
        //            repository.Save();
        //            return StatusCode(StatusCodes.Status201Created,
        //                 new ResponseStatus { status = "Success", message = "Weightage Added Successfully." ,statusCode= StatusCodes.Status201Created });
        //        }
        //        return StatusCode(StatusCodes.Status400BadRequest,
        //                new ResponseStatus { status = "Error", message = "Invalid Data." , statusCode= StatusCodes.Status400BadRequest });
        //    }
        //    catch (Exception ex)
        //    {
        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //}
        #endregion
        #region Updating Level For Employee which was access only Admin
        //[HttpPut]
        //[Route("UpdateLevelForEmployee")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateLevelForEmployee(UserLevelVM level)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var a = repository.UpdateLevelForEmployee(level);

        //            switch (a)
        //            {
        //                case "Updated":
        //                    repository.Save();
        //                    return StatusCode(StatusCodes.Status201Created,
        //                  new ResponseStatus { status = "Success", message = "Level Updated Successfully.", statusCode= StatusCodes.Status201Created });

        //                case "Error":
        //                    return StatusCode(StatusCodes.Status404NotFound,
        //                       new ResponseStatus { status = "Error", message = "Level not updated" , statusCode= StatusCodes.Status404NotFound });
        //                case "User Not Exist":
        //                    return StatusCode(StatusCodes.Status404NotFound,
        //                       new ResponseStatus { status = "Error", message = "User Not Exist" , statusCode = StatusCodes.Status404NotFound });
        //            }
        //        }
        //        return StatusCode(StatusCodes.Status400BadRequest,
        //           new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
        //    }
        //    catch (Exception ex)
        //    {
        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //}
        #endregion
        #region UpdateSkillWeightage which was access only by Admin
        //[HttpPut]
        //[Route("UpdateSkillWeightage")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateSkillWeightage(WeightageVM weightage)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var a = repository.UpdateSkillWeightage(weightage);
        //            switch (a)
        //            {
        //                case "Updated":
        //                    repository.Save();
        //                    return StatusCode(StatusCodes.Status201Created,
        //                        new ResponseStatus { status = "Success", message = "Weightage Updated SuccessFully",statusCode= StatusCodes.Status201Created });

        //                case "Skill Not Exists":
        //                    return StatusCode(StatusCodes.Status404NotFound,
        //                        new ResponseStatus { status = "Error", message = "Skill Not Exists", statusCode= StatusCodes.Status404NotFound  });
        //            }
        //        }
        //        return StatusCode(StatusCodes.Status400BadRequest,
        //         new ResponseStatus { status = "Error", message = "Invalid Datas",statusCode = StatusCodes.Status400BadRequest });
        //    }
        //    catch (Exception ex)
        //    {

        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //}
        #endregion
        #region Deleting Skill by Employee which was access only by Admin
        //[HttpDelete]
        //[Route("DeleteSkillbyEmp")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteSkillbyEmp(string EmployeeIdentity, int SkillId)
        //{
        //    try
        //    {
        //        var a = repository.DeleteSkillbyEmp(EmployeeIdentity, SkillId);
        //        switch (a)
        //        {
        //            case "Employee Skill Removed":
        //                repository.Save();
        //                return StatusCode(StatusCodes.Status200OK,
        //                   new ResponseStatus { status = "Success", message = "Employee Skill Deleted SuccessFully" , statusCode= StatusCodes.Status200OK });

        //            case "Error":
        //                return StatusCode(StatusCodes.Status404NotFound,
        //                      new ResponseStatus { status = "Error", message = "Employee Skill not found" , statusCode = StatusCodes.Status404NotFound });

        //        }

        //        return StatusCode(StatusCodes.Status404NotFound,
        //                    new ResponseStatus { status = "Error", message = "Something Error" , statusCode= StatusCodes.Status404NotFound });
        //    }
        //    catch (Exception ex)
        //    {

        //        ApiLog.Log("LogFile", ex.Message, ex.StackTrace, 10);
        //        return BadRequest(new FailureResponse<object>
        //        {
        //            Error = ex.Message,
        //            IsreponseSuccess = false
        //        });
        //    }
        //}
        #endregion


    }
}

