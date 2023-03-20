using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                         new ResponseStatus { status = "Success", message = "Goal Added Successfully" });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                         new ResponseStatus { status = "Error", message = "Employee not found" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                         new ResponseStatus { status = "Error", message = "Invalid Data" });
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                         new ResponseStatus { status = "Error", message = "Something Error" });

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
                         new ResponseStatus { status = "Success", message = "Goal Updated Successfully" });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                         new ResponseStatus { status = "Error", message = "Data not found" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "invalid Data" });
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Something Error" });
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
                            new ResponseStatus { status = "Success", message = "Goal Deleted Successfully" });

                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Goal Not Found" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseStatus { status = "Error", message = "Invalid Datas" });
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
                return Ok("User Data Unavailable");
            }
            return Ok(a.ToList());
        }
    }
}
