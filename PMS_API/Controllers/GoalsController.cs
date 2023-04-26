using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_API.LogHandling;
using PMS_API.Models;
using PMS_API.Reponse;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Data;

namespace PMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalRepo repository;
        public GoalsController(IGoalRepo _repository)
        {
            repository = _repository;
        }

        #region Add Goals For Employee
        [HttpPost]
        [Route("AddGoalForEmployee")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddGoalForEmployee(GoalVM goal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.AddGoalForEmployee(goal);

                    switch (a)
                    {
                        case "Success":
                            repository.Save();
                            repository.SendMailToEmployee(goal);

                            return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Goal Added Successfully", statusCode = StatusCodes.Status201Created });

                        case "Delay":
                            return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "You have uploaded goals late so your goals are Assign To Employee after Admin approval", statusCode = StatusCodes.Status201Created });

                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Error", message = "Employee not found", statusCode = StatusCodes.Status404NotFound });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                             new ResponseStatus { status = "Error", message = "Invalid Data", statusCode = StatusCodes.Status400BadRequest });
                }
                return StatusCode(StatusCodes.Status400BadRequest,
                             new ResponseStatus { status = "Error", message = "Something Error", statusCode = StatusCodes.Status400BadRequest });
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
        #region Update Goals For Employee
        [HttpPost]
        [Route("UpdateGoalForEmployee")]
        public async Task<IActionResult> UpdateGoalForEmployee(string EmployeeIdentity, int goalid, GoalVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateGoalForEmployee(EmployeeIdentity, goalid, model);
                    switch (a)
                    {
                        case "Success":
                            repository.Save();
                            return StatusCode(StatusCodes.Status200OK,
                             new ResponseStatus { status = "Success", message = "Goal Updated Successfully", statusCode = StatusCodes.Status200OK });

                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Error", message = "Data not found", statusCode = StatusCodes.Status404NotFound });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "invalid Data", statusCode = StatusCodes.Status400BadRequest });
                }

                return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Something Error", statusCode = StatusCodes.Status400BadRequest });
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
        #region Get Goals By Employee Id
        [HttpGet]
        [Route("GetGoalbyEmpId")]
        public async Task<IActionResult> GetGoalbyEmpId(string EmployeeIdentity)
        {
            try
            {
                var a = repository.GetGoalbyEmpId(EmployeeIdentity).ToList();
                if (a == null)
                {
                    return BadRequest(new
                    {
                        ResponseStatus = new ResponseStatus { status = "Error", message = "User Data Unavailable.", statusCode = StatusCodes.Status404NotFound }
                    });
                }
                return Ok(new
                {

                    list = a.ToList(),
                    ResponseStatus = new ResponseStatus { status = "Success", message = "Goal List.", statusCode = StatusCodes.Status200OK }
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
        #region Employee Post her Review For Goals
        [HttpPost]
        [Route("EmployeeGoalReview")]
        public async Task<IActionResult> EmployeeGoalReview([FromForm] EmployeeReviewVM reviewVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.EmployeeGoalReview(reviewVM);
                    switch (a)
                    {
                        case "ok":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Comment Posated Successfully", statusCode = StatusCodes.Status200OK });
                        case "Goal Not Exist":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Goal Not Found", statusCode = StatusCodes.Status404NotFound });
                        case "Submitted":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Already Submitted", statusCode = StatusCodes.Status400BadRequest });
                        case "Time Up":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Time is over so please contact your Manager...", statusCode = StatusCodes.Status400BadRequest });
                        case "Error":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Somthing Wrong Please try again Later...", statusCode = StatusCodes.Status400BadRequest });
                    }
                    return BadRequest(a.ToString());
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
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
        #region Update Employee Goal Review
        [HttpPost]
        [Route("UpdateEmployeeGoalReview")]
        public async Task<IActionResult> UpdateEmployeeGoalReview([FromForm] updateEmployeeReviewVM reviewVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateEmployeeGoalReview(reviewVM);
                    switch (a)
                    {
                        case "updated":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Goal Review Updated Successfully", statusCode = StatusCodes.Status200OK });

                        case "Goal Not Exist":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Review Not Found", statusCode = StatusCodes.Status404NotFound });
                        case "ReviewEnd":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Already Submitted", statusCode = StatusCodes.Status400BadRequest });

                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
                }
                return BadRequest();
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
        #region Manager post Her Review about Employee's Goal Review
        [HttpPost]
        [Route("ManagerGoalReview")]
        public async Task<IActionResult> ManagerGoalReview([FromForm] ManagerReviewVM reviewVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.ManagerGoalReview(reviewVM);
                    switch (a)
                    {
                        case "ok":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Comment Posated Successfully", statusCode = StatusCodes.Status200OK });

                        case "Goal Not Exist":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Goal Not Found", statusCode = StatusCodes.Status404NotFound });
                        case "completed":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Already Completed", statusCode = StatusCodes.Status400BadRequest });

                        case "Time Up":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Time is over. if you want more time make a Extention Request ", statusCode = StatusCodes.Status400BadRequest });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
                }
                return BadRequest();
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
        #region Update Manager Goal Review
        [HttpPost]
        [Route("UpdateManagerGoalReview")]
        public async Task<IActionResult>UpdateManagerGoalReview([FromForm] ManagerReviewVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateManagerGoalReview(model);
                    switch (a)
                    {
                        case "updated":
                            return StatusCode(StatusCodes.Status200OK,
                                new ResponseStatus { status = "Success", message = "Your Goal Review Updated Successfully", statusCode = StatusCodes.Status200OK });

                        case "Goal Not Exist":
                            return StatusCode(StatusCodes.Status404NotFound,
                                new ResponseStatus { status = "Error", message = "Review Not Found", statusCode = StatusCodes.Status404NotFound });
                        case "ReviewEnd":
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseStatus { status = "Error", message = "Your Review Time is Already End", statusCode = StatusCodes.Status400BadRequest });

                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
                }
                return BadRequest();
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
        #region Employee Extention Request
        [HttpPost]
        [Route("EmployeeExtentionRequest")]
        public async Task<IActionResult> EmployeeExtentionRequest(string EmployeeIdentity, int GoalID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.EmployeeExtentionRequest(EmployeeIdentity, GoalID);
                    switch (a)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status200OK,
                              new ResponseStatus { status = "Success", message = "Your Request send To Your Manager", statusCode = StatusCodes.Status200OK });

                        case "error":
                            return StatusCode(StatusCodes.Status400BadRequest,
                              new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status400BadRequest });

                        case "Already Requested":
                            return StatusCode(StatusCodes.Status400BadRequest,
                              new ResponseStatus { status = "Error", message = "Your Request already Submitted", statusCode = StatusCodes.Status400BadRequest });
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
        #region Manager Extention Request
        [HttpPost]
        [Route("ManagerExtentionRequest")]
        public async Task<IActionResult> ManagerExtentionRequest(string EmployeeIdentity, int GoalID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.ManagerExtentionRequest(EmployeeIdentity, GoalID);
                    switch (a)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status200OK,
                              new ResponseStatus { status = "Success", message = "Your Request send To Your Manager", statusCode = StatusCodes.Status200OK });

                        case "error":
                            return StatusCode(StatusCodes.Status400BadRequest,
                              new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status400BadRequest });

                        case "Already Requested":
                            return StatusCode(StatusCodes.Status400BadRequest,
                              new ResponseStatus { status = "Error", message = "Your Request already Submitted", statusCode = StatusCodes.Status400BadRequest });
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
        #region Employee Extention Request Approve
        [HttpPost]
        [Route("EmpExtReqApprove")]
        public async Task<IActionResult> EmpExtReqApprove(bool approved , int GoalId)
        {
            try
            {
                var a = repository.EmpExtReqApprove(approved, GoalId);
                switch (a)
                {
                    case "ok":
                        return StatusCode(StatusCodes.Status200OK,
                          new ResponseStatus { status = "Success", message = "Extention Time Approved", statusCode = StatusCodes.Status200OK });
                    case "Notok":
                        return StatusCode(StatusCodes.Status200OK,
                          new ResponseStatus { status = "Success", message = "Extention Time Declined", statusCode = StatusCodes.Status200OK });
                    case "error":
                        return StatusCode(StatusCodes.Status400BadRequest,
                          new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status400BadRequest });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status404NotFound });

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
        #region Manager Extention Request Approve
        [HttpPost]
        [Route("ManagerExtReqApprove")]
        public async Task<IActionResult> ManagerExtReqApprove(bool approved, int GoalId)
        {
            try
            {
                var a = repository.ManagerExtReqApprove(approved, GoalId);
                switch (a)
                {
                    case "ok":
                        return StatusCode(StatusCodes.Status200OK,
                          new ResponseStatus { status = "Success", message = "Your Request send To Your Manager", statusCode = StatusCodes.Status200OK });
                    case "error":
                        return StatusCode(StatusCodes.Status400BadRequest,
                          new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status400BadRequest });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Somthing went wrong Please Try again Later", statusCode = StatusCodes.Status404NotFound });
                    case "Already Requested":
                        return StatusCode(StatusCodes.Status400BadRequest,
                          new ResponseStatus { status = "Error", message = "Your Request already Submitted", statusCode = StatusCodes.Status400BadRequest });
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
        #region Admin Request Aprrove For Delayed Goals
        [HttpPost]
        [Route("AdminReqApproved")]
        public async Task<IActionResult> AdminReqApproved(bool Approved, int delayedGoalId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.AdminReqApproved(Approved, delayedGoalId);

                    switch (a)
                    {
                        case "Ok":
                            return StatusCode(StatusCodes.Status200OK,
                           new ResponseStatus { status = "Success", message = "Request Accepted goal add to Employee", statusCode = StatusCodes.Status200OK });
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
        #region Submit Goal
        [HttpPost]
        [Route("SubmitGoals")]
        public async Task<IActionResult> SubmitGoals(string EmployeeIdentity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   // repository.GoalRatingCalculation(EmployeeIdentity);
                    return Ok();
                }
                return BadRequest();
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
        #region Delete Employee's Goal
        //[HttpDelete]
        //[Route("DeleteGoalForEmployee")]
        //public async Task<IActionResult> DeleteGoalForEmployee(int empid, int goalid)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var a = repository.DeleteGoalForEmployee(empid, goalid);
        //        switch (a)
        //        {
        //            case "Success":
        //                repository.Save();
        //                return StatusCode(StatusCodes.Status200OK,
        //                    new ResponseStatus { status = "Success", message = "Goal Deleted Successfully" , statusCode = StatusCodes.Status200OK });

        //            case "Error":
        //                return StatusCode(StatusCodes.Status404NotFound,
        //                    new ResponseStatus { status = "Error", message = "Goal Not Found", statusCode = StatusCodes.Status404NotFound });
        //        }
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest,
        //                new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest  });
        //    }
        //    return BadRequest();

        //}
        #endregion

    }
}
