using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;

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

        [HttpPost]
        [Route("AddGoalForEmployee")]
        public async Task<IActionResult> AddGoalForEmployee(GoalVM goal)
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
                         new ResponseStatus { status = "Success", message = "Goal Added Successfully" ,statusCode= StatusCodes.Status201Created });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                         new ResponseStatus { status = "Error", message = "Employee not found", statusCode = StatusCodes.Status404NotFound });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                         new ResponseStatus { status = "Error", message = "Invalid Data", statusCode= StatusCodes.Status400BadRequest });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                         new ResponseStatus { status = "Error", message = "Something Error" , statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpPut]
        [Route("UpdateGoalForEmployee")]
        public async Task<IActionResult> UpdateGoalForEmployee(int empid, int goalid, GoalVM model)
        {
            if (ModelState.IsValid)
            {
                var a = repository.UpdateGoalForEmployee(empid, goalid, model);
                switch (a)
                {
                    case "Success":
                        repository.Save();
                        return StatusCode(StatusCodes.Status200OK,
                         new ResponseStatus { status = "Success", message = "Goal Updated Successfully" , statusCode= StatusCodes.Status200OK });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                         new ResponseStatus { status = "Error", message = "Data not found" , statusCode = StatusCodes.Status404NotFound });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "invalid Data" , statusCode = StatusCodes.Status400BadRequest });
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Something Error" , statusCode = StatusCodes.Status400BadRequest });
        }

        [HttpDelete]
        [Route("DeleteGoalForEmployee")]
        public async Task<IActionResult> DeleteGoalForEmployee(int empid, int goalid)
        {
            if (ModelState.IsValid)
            {
                var a = repository.DeleteGoalForEmployee(empid, goalid);
                switch (a)
                {
                    case "Success":
                        repository.Save();
                        return StatusCode(StatusCodes.Status200OK,
                            new ResponseStatus { status = "Success", message = "Goal Deleted Successfully" , statusCode = StatusCodes.Status200OK });

                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Goal Not Found", statusCode = StatusCodes.Status404NotFound });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest  });
            }
            return BadRequest();

        }

        [HttpGet]
        [Route("GetGoalbyEmpId")]
        public async Task<IActionResult> GetGoalbyEmpId(int empid)
        {
            var a = repository.GetGoalbyEmpId(empid).ToList();
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

        [HttpPost]
        [Route("EmployeeGoalReview")]
        public async Task<IActionResult> EmployeeGoalReview([FromForm] EmployeeReviewVM reviewVM)
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
                            new ResponseStatus { status = "Error", message = "Your Review Already Submitted" , statusCode = StatusCodes.Status400BadRequest });
                    case "Time Up":
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Your Review Time is over so please contact your Manager..." , statusCode = StatusCodes.Status400BadRequest });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateEmployeeGoalReview")]
        public async Task<IActionResult> UpdateEmployeeGoalReview([FromForm] updateEmployeeReviewVM reviewVM)
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
                            new ResponseStatus { status = "Error", message = "Review Not Found" , statusCode = StatusCodes.Status404NotFound });
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

        [HttpPut]
        [Route("UpdateManagerGoalReview")]
        public async Task<IActionResult>UpdateManagerGoalReview([FromForm] ManagerReviewVM model)
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
                            new ResponseStatus { status = "Error", message = "Review Not Found" , statusCode = StatusCodes.Status404NotFound });
                    case "ReviewEnd":
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Your Review Time is Already End" , statusCode = StatusCodes.Status400BadRequest });

                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Datas" , statusCode = StatusCodes.Status400BadRequest });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ExtentionRequest")]
        public async Task<IActionResult> ExtentionRequest(int EmployeeID, int GoalID)
        {
            if(ModelState.IsValid)
            {
                var a = repository.ExtentionRequest(EmployeeID, GoalID);
                switch (a)
                {
                    case "":
                        return StatusCode(StatusCodes.Status200OK,
                          new ResponseStatus { status = "Success", message = "Your Comment Posated Successfully" ,statusCode = StatusCodes.Status200OK });

                    case "a":
                        return StatusCode(StatusCodes.Status200OK,
                          new ResponseStatus { status = "Success", message = "Your Comment Posated Successfully" ,statusCode = StatusCodes.Status200OK });
                }
            }

            return Ok(GoalID);
        }

        [HttpPost]
        [Route("ManagerGoalReview")]
        public async Task<IActionResult> ManagerGoalReview([FromForm] ManagerReviewVM reviewVM)
        {
            if (ModelState.IsValid)
            {
                var a = repository.ManagerGoalReview(reviewVM);
                switch (a)
                {
                    case "ok":
                        return StatusCode(StatusCodes.Status200OK,
                            new ResponseStatus { status = "Success", message = "Your Comment Posated Successfully" , statusCode = StatusCodes.Status200OK });

                    case "Goal Not Exist":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Goal Not Found" , statusCode = StatusCodes.Status404NotFound });

                    case "Time Up":
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Your Review Time is over. if you want more time make a Extention Request " , statusCode = StatusCodes.Status400BadRequest });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Datas" ,statusCode = StatusCodes.Status400BadRequest });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("SubmitGoals")]
        public async Task<IActionResult> SubmitGoals(int EmployeeID)
        {
            if (ModelState.IsValid)
            {
                 repository.GoalRatingCalculation(EmployeeID);
                return Ok();
            }
            return BadRequest();
        }



    }
}
