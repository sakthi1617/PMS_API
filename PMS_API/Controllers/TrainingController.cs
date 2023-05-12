using Microsoft.AspNetCore.Components.Forms;
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
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepo repository;

        public TrainingController(ITrainingRepo _repository)
        {
           repository = _repository;   
        }

        #region Add Questions
        [HttpPost]
        [Route("AddQuestion")]
        public async Task<IActionResult> AddQuestion(QuestionBankVM question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.AddQuestion(question);
                    switch (a)
                    {
                        case "Success":
                            return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Question added successfully", statusCode = StatusCodes.Status201Created });
                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Error", message = "Skill not found ", statusCode = StatusCodes.Status404NotFound });
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

        #region Update Question
        [HttpPut]
        [Route("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion(int QuestionID, QuestionBankVM question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.UpdateQuestion(QuestionID,question);
                    switch (a)
                    {
                        case "Success":
                            return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Question Updated successfully", statusCode = StatusCodes.Status201Created });
                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Error", message = "Question not found ", statusCode = StatusCodes.Status404NotFound });
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

        #region Delete Question
        [HttpDelete]
        [Route("DeleteQuestion")]
        public async Task<IActionResult> DeleteQuestion(int QuestionID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var a = repository.DeleteQuestion(QuestionID);
                    switch (a)
                    {
                        case "Success":
                            return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Question Deleted successfully", statusCode = StatusCodes.Status201Created });
                        case "Error":
                            return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Error", message = "Question not found ", statusCode = StatusCodes.Status404NotFound });
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

        #region Get all Questions
        [HttpGet]
        [Route ("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var a = repository.GetAllQuestions();

                return Ok(new SuccessResponse<object>
                {
                    ModelData = new
                    {
                        Questions = a
                    },
                    statusCode = "200",
                    Response = "ok"
                }) ;
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

        #region Get all Questions By SkillID and Level
        [HttpGet]
        [Route("QuestionsByskillId")]
        public async Task<IActionResult> QuestionsByskillId(int skillId, string level)
        {
            try
            {
                var a = repository.QuestionsByskillId(skillId,level);

                return Ok(new SuccessResponse<object>
                {
                    ModelData = new
                    {
                        Questions = a
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

        #region Get Questions By EmployeeSkillLevel
        [HttpGet]
        [Route("QuestionByEmployeeLevel")]
        public async Task<IActionResult> QuestionByEmployeeLevel(int EmployeeId,int skillId)
        {
            try
            {
                var a = repository.EmpExamQuestions(EmployeeId, skillId);

                return Ok(new SuccessResponse<object>
                {
                    ModelData = new
                    {
                        Questions = a
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

        #region Validate the answers
        [HttpPost]
        [Route("ValidateAnswers")]
        public async Task<IActionResult> ValidateAnswers([FromBody]List<ValidateVM> validate)
        {
            try
            {
                var a = repository.ValidateAnswers(validate);

                return Ok(new SuccessResponse<object>
                {
                    ModelData = new
                    {
                        result = a,
                        
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
    }
}
