using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_API.LogHandling;
using PMS_API.Reponse;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;

namespace PMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepo repository;
        private readonly IEmailService _emailservice;
        public SkillsController(ISkillRepo _repository, IEmailService emailservice)
        {
            repository = _repository;
            _emailservice = emailservice;
        }

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
                    return StatusCode(StatusCodes.Status201Created,
                         new ResponseStatus { status = "Success", message = "Weightage Added Successfully.", statusCode = StatusCodes.Status201Created });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Data.", statusCode = StatusCodes.Status400BadRequest });
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
                               new ResponseStatus { status = "Success", message = "Skill Added Successfully", statusCode = StatusCodes.Status201Created });

                        case "Skill Already Exist":
                            return StatusCode(StatusCodes.Status400BadRequest,
                               new ResponseStatus { status = "Error", message = "Skill Already Added This Employee...", statusCode = StatusCodes.Status400BadRequest });
                        case "User Not exists":
                            return StatusCode(StatusCodes.Status404NotFound,
                               new ResponseStatus { status = "Error", message = "User Not Found", statusCode = StatusCodes.Status404NotFound });
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

        #region Listing Employee skill by Id which wass access by all
        [HttpGet]
        [Route("GetEmployeeSkillsById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeSkillsById(string EmployeeIdentity)
        {
            try
            {
                var Employee = repository.GetEmployeeSkillsById(EmployeeIdentity).ToList();
                if (Employee.Count > 0)
                {
                    return Ok(new
                    {

                        list = Employee,
                        ResponseStatus = new ResponseStatus { status = "Success", message = "Skill List.", statusCode = StatusCodes.Status200OK }
                    });
                }
                return NotFound(new
                {
                    ResponseStatus = new ResponseStatus { status = "Error", message = "Employee Not Found.", statusCode = StatusCodes.Status404NotFound }
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
                                new ResponseStatus { status = "Success", message = "Skill Updated SuccessFully", statusCode = StatusCodes.Status201Created });

                        case "Designation Not Exists":

                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Skill Not Exists", statusCode = StatusCodes.Status404NotFound });
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

        #region UpdateSkillWeightages which wass access by All
        [HttpPut]
        [Route("UpdateSkillWeightages")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkillWeightages(int skillId, string employeeIdentity, int weightage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateSkillWeightages(skillId, employeeIdentity, weightage);
                    switch (a)
                    {
                        case "ok":
                            return StatusCode(StatusCodes.Status201Created,
                       new ResponseStatus { status = "Success", message = "Weightage Successfully Updated.", statusCode = StatusCodes.Status201Created });

                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                       new ResponseStatus { status = "Error", message = "Skill Not Found", statusCode = StatusCodes.Status404NotFound });

                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                       new ResponseStatus { status = "Error", message = "Invalid Data.", statusCode = StatusCodes.Status400BadRequest });
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
        public async Task<IActionResult> ReqForUpdateLvl(string employeeIdentity, int SklID, string descrip, string rea, IFormFileCollection fiels)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.ReqForUpdateLvl(employeeIdentity, SklID, descrip, rea, fiels);

                    switch (a)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Update Request Send Successfully", statusCode = StatusCodes.Status200OK });
                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "User Not Exsist", statusCode = StatusCodes.Status404NotFound });

                        case "MaxLevel":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Cannot upgrade as you have reached the maximum score already.", statusCode = StatusCodes.Status400BadRequest });
                    }

                }
                return StatusCode(StatusCodes.Status400BadRequest,
                                 new ResponseStatus { status = "Error", message = "Invalid datas", statusCode = StatusCodes.Status400BadRequest });
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
                                new ResponseStatus { status = "Success", message = "Response Deliverd", statusCode = StatusCodes.Status201Created });
                        case "error":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Something Error", statusCode = StatusCodes.Status404NotFound });
                        case "RequestExpired":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "RequestExpired", statusCode = StatusCodes.Status400BadRequest });
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

        #region UserLEvelDecrement
        [HttpPost]
        [Route("UserLevelDecrement")]
        public async Task<IActionResult> UserLevlDecrement(string EmployeeIdentity, int skillId)
        {
            try
            {
                var levelDecre = repository.UserLevelDecrement(EmployeeIdentity, skillId);
                if (levelDecre != null)
                {
                    //return Ok(levelDecre);
                    return StatusCode(StatusCodes.Status200OK,
                         new ResponseStatus { status = "Success", message = "Employee Skill level Decreased SuccessFully", statusCode = StatusCodes.Status200OK });
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
