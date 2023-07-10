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
                            

                            repository.AccountActivateEmail((int)employeeCreationResult, employeeModule);

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
        #region InactiveEmployeeList
        [HttpGet]
        [Route("InactiveEmployeeList")]
        public async Task<IActionResult> InactiveEmployeeList()
        {
              try
            {

                var result = repository.InactiveEmployeeList();


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
        [Route("EmployeesinNoticePeriod")]
        public async Task<IActionResult> EmployeesinNoticePeriod()
        {
            try
            {

                var result = repository.EmployeesinNoticePeriod();


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
        [HttpGet]
        [Route("ResignedEmployeeList")]
        public async Task<IActionResult> ResignedEmployeeList()
        {
            try
            {

                var result = repository.ResignedEmployeeList();


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

