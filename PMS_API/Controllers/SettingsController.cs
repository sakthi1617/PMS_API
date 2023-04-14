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
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepo repository;
        public SettingsController(ISettingsRepo _repository)
        {
            repository = _repository;
        }

        [HttpPost]
        [Route("TimeSetting")]
        public async Task<IActionResult> TimeSetting(TimeSettingVM model)
        {
            try
            {
                var result = repository.TimeSetting(model);
                if (result != null)
                {
                    return Ok(new SuccessResponse<object>
                    {
                        statusCode = "200",
                        ModelData = new
                        {
                            EmployeeReviewDate = model.EmployeeReviewDay,
                            ManagerReviewDate = model.ManagerReviewDay
                        }
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseStatus
                {
                    status = "Error",
                    message = "Dates Could't updated or Created",
                    statusCode = StatusCodes.Status400BadRequest
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

    }
}
