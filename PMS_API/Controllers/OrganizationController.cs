using Hangfire.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public OrganizationController(IOrganizationRepo _repository, IEmailService emailservice)
        {
            repository = _repository;
            _emailservice = emailservice;
        }

        #region Adding Employee which was access only by Admin
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> addEmployee(EmployeeVM employeeModule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employeeCreationResult = repository.AddEmployee(employeeModule);

                    if (employeeCreationResult != 0 && employeeCreationResult != null)
                    {

                        var userLevelResult = repository.AddUserLevel(employeeModule.DesignationId, employeeModule.DepartmentId, employeeCreationResult);
                        if (userLevelResult == "Created")
                        {
                            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
                            var msg = " Hi " + employeeModule.Name + "" + "your Account created Succesfully To set your password, please click the following link: https://localhost:7099/api/OrganizationAuth/ResetPassword?Email= " + employeeModule.Email;
                            var message = new Message(new string[] { employeeModule.Email }, "Welcome To PMS", msg.ToString(), files);
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

        #region Adding Designation which was access only by Admin
        [HttpPost]
        [Route("AddDesignation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDesignation(DesignationVM designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddDesignation(designation);
                    repository.Save();
                    return StatusCode(StatusCodes.Status201Created,
                      new ResponseStatus { status = "Success", message = "Designation Added Successfully" , statusCode = StatusCodes.Status201Created });
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

        #region Adding Skills which was access only by Admin
        [HttpPost]
        [Route("AddSkills")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addSkill(SkillsVM skill)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddSkill(skill);
                    repository.Save();
                    return StatusCode(StatusCodes.Status201Created,
                        new ResponseStatus { status = "Success", message = "Skill Added Successfully", statusCode = StatusCodes.Status201Created });
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

        #region Adding Additional skills which was access only by Admin
        [HttpPost]
        [Route("AddAdditionalSkills")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdditionalSkills(UserLevelVM level)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.AddAdditionalSkills(level);
                    switch (a)
                    {
                        case "Success":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                               new ResponseStatus { status = "Success", message = "Skill Added Successfully" , statusCode = StatusCodes.Status201Created });

                        case "Skill Already Exist":
                            return StatusCode(StatusCodes.Status400BadRequest,
                               new ResponseStatus { status = "Error", message = "Skill Already Added This Employee...", statusCode = StatusCodes.Status400BadRequest });
                        case "User Not exists":
                            return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Error", message = "User Not Found" , statusCode= StatusCodes.Status404NotFound });
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

        #region Adding skillWeightage which was access only by Admin
        [HttpPost]
        [Route("AddSkillWeightage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSkillWeightage(WeightageVM weightage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository.AddSkillWeightage(weightage);
                    repository.Save();
                    return StatusCode(StatusCodes.Status201Created,
                         new ResponseStatus { status = "Success", message = "Weightage Added Successfully." ,statusCode= StatusCodes.Status201Created });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Data." , statusCode= StatusCodes.Status400BadRequest });
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
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeVM employee)
        {
            try
            {
                if (ModelState.IsValid && employee.EmployeeId != null)
                {
                    string a = repository.UpdateEmployee(id, employee);
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

        #region Updating Skill which was access only by Admin
        [HttpPut]
        [Route("UpdateSkill")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkill(int id, SkillsVM skill)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string a = repository.UpdateSkill(id, skill);
                    switch (a)
                    {
                        case "Updated":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                                new ResponseStatus { status = "Success", message = "Skill Updated SuccessFully" , statusCode = StatusCodes.Status201Created });

                        case "Designation Not Exists":

                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Skill Not Exists" , statusCode = StatusCodes.Status404NotFound });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode= StatusCodes.Status400BadRequest });
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

        #region Updating Level
        [HttpPost]
        [Route("ReqForUpdateLvl")]
        public async Task<IActionResult> ReqForUpdateLvl(int EmpID,int SklID, string descrip, string rea, IFormFileCollection fiels)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.ReqForUpdateLvl(EmpID,SklID,descrip,rea, fiels);

                    switch (a)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Update Request Send Successfully" , statusCode = StatusCodes.Status200OK });
                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "User Not Exsist" , statusCode = StatusCodes.Status404NotFound });

                        case "MaxLevel":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Cannot upgrade as you have reached the maximum score already.", statusCode= StatusCodes.Status400BadRequest });
                    }

                }
                return StatusCode(StatusCodes.Status400BadRequest,
                                 new ResponseStatus { status = "Error", message = "Invalid datas", statusCode= StatusCodes.Status400BadRequest });
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

        #region LevelApprovedSucess
        [HttpPost]
        [Route("LevlelApprovedSuccess")]
        public async Task<IActionResult> LevlelApprovedSuccess(int reqid, bool status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var approved = repository.LevlelApprovedSuccess(reqid, status);

                    switch (approved)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status201Created,
                                new ResponseStatus { status = "Success", message = "Response Deliverd", statusCode= StatusCodes.Status201Created });
                        case "error":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Something Error", statusCode= StatusCodes.Status404NotFound });
                        case "RequestExpired":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "RequestExpired", statusCode= StatusCodes.Status400BadRequest });
                    }
                }
                return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status404NotFound });
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

        #region Updating Level For Employee which was access only Admin
        [HttpPut]
        [Route("UpdateLevelForEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLevelForEmployee(UserLevelVM level)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateLevelForEmployee(level);

                    switch (a)
                    {
                        case "Updated":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                          new ResponseStatus { status = "Success", message = "Level Updated Successfully.", statusCode= StatusCodes.Status201Created });

                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Error", message = "Level not updated" , statusCode= StatusCodes.Status404NotFound });
                        case "User Not Exist":
                            return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Error", message = "User Not Exist" , statusCode = StatusCodes.Status404NotFound });
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

        #region UpdateSkillWeightage which was access only by Admin
        [HttpPut]
        [Route("UpdateSkillWeightage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkillWeightage(WeightageVM weightage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateSkillWeightage(weightage);
                    switch (a)
                    {
                        case "Updated":
                            repository.Save();
                            return StatusCode(StatusCodes.Status201Created,
                                new ResponseStatus { status = "Success", message = "Weightage Updated SuccessFully",statusCode= StatusCodes.Status201Created });

                        case "Skill Not Exists":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Skill Not Exists", statusCode= StatusCodes.Status404NotFound  });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                 new ResponseStatus { status = "Error", message = "Invalid Datas",statusCode = StatusCodes.Status400BadRequest });
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
        public async Task<IActionResult> EmployeeModule()
        {
            try
            {
                var employeeList = repository.EmployeeList().ToList();
                return Ok(new
                {

                    list = employeeList,
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

        #region Listing EmployeeDetail by Id which wass access by all
        [HttpGet]
        [Route("EmployeeById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> EmployeeById(int id)
        {
            try
            {
                var EmployeeId = repository.EmployeeById(id);
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

        #region Listing Skill which was access by all
        [HttpGet]
        [Route("SkillsModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SkillsModule()
        {
            try
            {
                var skillList = repository.SkilsList().ToList();
                return Ok(new
                {

                    list = skillList,
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

        #region Listing Skill by DepartmentId which wass access by All
        [HttpGet]
        [Route("SkillbyDepartmentID")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SkillbyDepartmentID(int id)
        {
            try
            {
                var skillbydeptid = repository.SkillbyDepartmentID(id).ToList();
                return Ok(new
                {

                    list = skillbydeptid,
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

        #region Listing Employee skill by Id which wass access by all
        [HttpGet]
        [Route("GetEmployeeSkillsById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeSkillsById(int EmployeeId)
        {
            try
            {
                var Employee = repository.GetEmployeeSkillsById(EmployeeId).ToList();
                if(Employee.Count >0)
                {
                    return Ok(new
                    {

                        list = Employee,
                        ResponseStatus = new ResponseStatus { status = "Success", message = "Skill List.", statusCode = StatusCodes.Status200OK }
                    });
                }
                return NotFound(new
                {
                    ResponseStatus = new ResponseStatus { status = "Error", message = "Employee Not Found.", statusCode = StatusCodes.Status404NotFound}
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
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            try
            {
                var a = repository.DeleteEmployee(EmployeeId);

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

        #region Deleting Skill by Employee which was access only by Admin
        [HttpDelete]
        [Route("DeleteSkillbyEmp")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSkillbyEmp(int EmpployeeId, int SkillId)
        {
            try
            {
                var a = repository.DeleteSkillbyEmp(EmpployeeId, SkillId);
                switch (a)
                {
                    case "Employee Skill Removed":
                        repository.Save();
                        return StatusCode(StatusCodes.Status200OK,
                           new ResponseStatus { status = "Success", message = "Employee Skill Deleted SuccessFully" , statusCode= StatusCodes.Status200OK });

                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                              new ResponseStatus { status = "Error", message = "Employee Skill not found" , statusCode = StatusCodes.Status404NotFound });

                }

                return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Something Error" , statusCode= StatusCodes.Status404NotFound });
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
        
        #region UserLEvelDecrement
        [HttpPost]
        [Route("UserLevelDecrement")]
        public async Task<IActionResult> UserLevlDecrement(int employeeId, int skillId)
        {
            try
            {
                var levelDecre = repository.UserLevelDecrement(employeeId, skillId);
                if (levelDecre != null)
                {
                    //return Ok(levelDecre);
                    return StatusCode(StatusCodes.Status200OK,
                         new ResponseStatus { status = "Success", message = "Employee Skill level Decreased SuccessFully" , statusCode = StatusCodes.Status200OK });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Invaid Datas", statusCode = StatusCodes.Status404NotFound });
                }
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

    }
}

