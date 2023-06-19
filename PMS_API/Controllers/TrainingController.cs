using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using PMS_API.LogHandling;
using PMS_API.Models;
using PMS_API.Reponse;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.Formats.Asn1;
using System.Reflection.Metadata;

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
                    var a = repository.UpdateQuestion(QuestionID, question);
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
        [Route("GetAllQuestions")]
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

        #region Get all Questions By SkillID and Level
        [HttpGet]
        [Route("QuestionsByskillId")]
        public async Task<IActionResult> QuestionsByskillId(int skillId, int level)
        {
            try
            {
                var a = repository.QuestionsByskillId(skillId, level);

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

        //#region Get Questions By EmployeeSkillLevel
        //[HttpGet]
        //[Route("QuestionByEmployeeLevel")]
        //public async Task<IActionResult> QuestionByEmployeeLevel(int EmployeeId,int skillId)
        //{
        //    try
        //    {
        //        var a = repository.EmpExamQuestions(EmployeeId, skillId);

        //        return Ok(new SuccessResponse<object>
        //        {
        //            ModelData = new
        //            {
        //                Questions = a
        //            },
        //            statusCode = "200",
        //            Response = "ok"
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
        //#endregion

        #region Generate QustionPaper
        [HttpPost]
        [Route("GenerateQustionPaper")]
        public async Task<IActionResult> GenerateQustionPaper(GenerateQustionPaper generate)
        {
            if (ModelState.IsValid)
            {
                var a = repository.GenerateQustionPaper(generate);
                switch (a)
                {
                    case "ok":
                        return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Question Paper Created", statusCode = StatusCodes.Status201Created });


                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                      new ResponseStatus { status = "Error", message = "Invalid datas", statusCode = StatusCodes.Status400BadRequest });
        }
        #endregion

        #region Set PassMark
        [HttpPost]
        [Route("SetPassLimit")]
        public async Task<IActionResult> SetPassLimit(int QuestionPaperId , int LimitPersentage)
        {
            if (ModelState.IsValid)
            {
                var a = repository.SetPassLimit(QuestionPaperId,LimitPersentage);
                switch (a)
                {
                    case "Pass Percentage Updated":
                        return StatusCode(StatusCodes.Status201Created,
                     new ResponseStatus { status = "Success", message = "Pass Percentage Updated", statusCode = StatusCodes.Status201Created });

                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                     new ResponseStatus { status = "Error", message = "QuestionPaper Not Found", statusCode = StatusCodes.Status404NotFound });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                     new ResponseStatus { status = "Error", message = "Invalid datas", statusCode = StatusCodes.Status400BadRequest });
        }
        #endregion

        #region Edit Question Paper
        [HttpPut]
        [Route("EditQuestionPaper")]
        public async Task<IActionResult> EditQuestionPaper(int QuestionPaperId, GenerateQustionPaper generate)
        {
            if (ModelState.IsValid)
            {
                var a = repository.EditQuestionPaper(QuestionPaperId, generate);
                switch (a)
                {
                    case "ok":
                        return StatusCode(StatusCodes.Status201Created,
                            new ResponseStatus { status = "Success", message = "Question Paper Upadated", statusCode = StatusCodes.Status201Created });
                    case "NotFound":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Question Paper Not Found", statusCode = StatusCodes.Status404NotFound });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
        }
        #endregion

        #region Get Skillwise AllQuestionPaper
        [HttpGet]
        [Route("GetSkillwiseAllQuestionPaper")]
        public async Task<IActionResult> GetSkillwiseAllQuestionPaper(int skillId)
        {
            try
            {
                var a = repository.GetSkillwiseAllQuestionPaper(skillId);

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

        #region Get QustionPaper For Examiner
        [Route("GetQustionPaperForExaminer")]
        [HttpGet]
        public async Task<IActionResult> GetQustionPaperForExaminer(int QuestionpaperID)
        {
            try
            {
                var a = repository.GetQustionPaperForExaminer(QuestionpaperID);

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

        #region Assign to Employee
        [HttpPost]
        [Route("AssignToEmployee")]
        public async Task<IActionResult> AssignToEmployee(string EmployeeIdentity, int QuestionpaperID)
        {
            if (ModelState.IsValid)
            {
                var a = repository.AssignToEmployee(EmployeeIdentity, QuestionpaperID);
                switch (a)
                {
                    case "Success":
                        return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Test Assigned to Employee", statusCode = StatusCodes.Status201Created });
                    case "NoSkill":
                        return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Success", message = "Employee Dont Have a Skill", statusCode = StatusCodes.Status404NotFound });
                    case "Questionpaper Not Found":
                        return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Success", message = "Question Paper Notfound", statusCode = StatusCodes.Status404NotFound });
                    case "Employee Not Found":
                        return StatusCode(StatusCodes.Status404NotFound,
                             new ResponseStatus { status = "Success", message = "Employee Not Found", statusCode = StatusCodes.Status404NotFound });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                             new ResponseStatus { status = "Success", message = "Invalid Data", statusCode = StatusCodes.Status400BadRequest });
        }
        #endregion

        #region Get QustionPaper For Examer
        [HttpGet]
        [Route("GetQustionPaperForExamer")]
        public async Task<IActionResult> GetQustionPaperForExamer(string EmployeeIdentity, int QuestionpaperID)
        {
            try
            {
                var a = repository.GetQustionPaperForExamer(EmployeeIdentity, QuestionpaperID);

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
        public async Task<IActionResult> ValidateAnswers(List<ValidateVM> validate)
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

        #region
        [HttpGet]
        [Route("QuestionPaperStatus")]
        public async Task<IActionResult> QuestionPaperStatus(int QuestionpaperId)
        {
            try
            {
                var a = repository.QuestionPaperStatus(QuestionpaperId);

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

        #region
        [HttpGet]
        [Route("QuestionPaperStatusDetail")]
        public async Task<IActionResult> QuestionPaperStatusDetail(int QuestionpaperId)
        {
            try
            {
                var a = repository.QuestionPaperStatusDetail(QuestionpaperId);

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


        #region Document Validation
        [HttpPost]
        [Route("DocumentValidation")]
        public async Task<IActionResult> DocumentValidation(List<DocumentValidationVM> document)
        {
            if (ModelState.IsValid)
            {
                 repository.documentValidation(document);
                //switch (a)
                //{
                //    case "Mark updated":
                //        return StatusCode(StatusCodes.Status201Created,
                //              new ResponseStatus { status = "Success", message = "Marks added successfully", statusCode = StatusCodes.Status201Created });
                //    case "invalid Mark":
                //        return StatusCode(StatusCodes.Status400BadRequest,
                //              new ResponseStatus { status = "Error", message = "Invalid marks" , statusCode = StatusCodes.Status400BadRequest });
                //    case "Already Updated":
                //        return StatusCode(StatusCodes.Status400BadRequest,
                //              new ResponseStatus { status = "Error", message = "Invalid marks", statusCode = StatusCodes.Status400BadRequest });

                //}
                return StatusCode(StatusCodes.Status201Created,
                             new ResponseStatus { status = "Success", message = "Marks added successfully", statusCode = StatusCodes.Status201Created });


            }
            return StatusCode(StatusCodes.Status400BadRequest,
                            new ResponseStatus { status = "Error", message = "Invalid Datas", statusCode = StatusCodes.Status400BadRequest });
        }
        #endregion








        //#region Get Document
        //[HttpPost]
        //[Route("GetDocument")]
        //public  async Task<string> GetDocument(int documentid)
        //{
        //    var a = repository.GetFile(documentid);
        //}
        //#endregion


        //         try
        //            {
        //                string HTMLBody = "";

        //                using (StreamReader sReader = System.IO.File.OpenText(pdfTemplatePath))
        //                {
        //                    HTMLBody = await sReader.ReadToEndAsync();
        //                    return HTMLBody;
        //                }
        //            }
        //            catch (Exception ex)
        //{
        //    string mmm = ex.Message;
        //    return mmm;
        //}

    }
}
