using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
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

        [Authorize(Roles ="Admin")]
      
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> addEmployee(EmployeeVM employeeModule)
        {
            if (ModelState.IsValid)
            {
                var employeeCreationResult = repository.AddEmployee(employeeModule);

                if (employeeCreationResult != 0 && employeeCreationResult != null)
                {

                    var userLevelResult = repository.AddUserLevel(employeeModule.DesignationId, employeeModule.DepartmentId, employeeCreationResult);
                    if (userLevelResult == "Created")
                    {
                        var msg = " Hi " + employeeModule.Name + "" + "your Account created Succesfully To set your password, please click the following link: https://localhost:7099/api/OrganizationAuth/ResetPassword?Email= "+employeeModule.Email;
                        var message = new Message(new string[] { employeeModule.Email }, "Welcome To PMS", msg.ToString());
                        _emailservice.SendEmail(message);

                        return StatusCode(StatusCodes.Status201Created,
                        new ResponseStatus { status = "Success", message = "Employee Added Successfully." });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                          new ResponseStatus { status = "Error", message = "Something Error" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "User Already Exists" });
                }

            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

                                                                                                    

        [HttpPost]
        [Route("AddDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(DepartmentVM department)
        {
            if (ModelState.IsValid)
            {
                repository.AddDepartment(department);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                   new ResponseStatus { status = "Success", message = "Department Added Successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }


        [HttpPost]
        [Route("AddDesignation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDesignation(DesignationVM designation)
        {
            if (ModelState.IsValid)
            {
                repository.AddDesignation(designation);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                  new ResponseStatus { status = "Success", message = "Designation Added Successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
             new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPost]
        [Route("AddSkills")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addSkill(SkillsVM skill)
        {
            if (ModelState.IsValid)
            {
                repository.AddSkill(skill);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                    new ResponseStatus { status = "Success", message = "Skill Added Successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPost]
        [Route("AddAdditionalSkills")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdditionalSkills(UserLevelVM level)
        {
            if (ModelState.IsValid)
            {
             var a =    repository.AddAdditionalSkills(level);
                switch (a)
                {
                    case "Success":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                           new ResponseStatus { status = "Success", message = "Skill Added Successfully" });

                    case "Skill Already Exist":
                        return StatusCode(StatusCodes.Status400BadRequest,
             new ResponseStatus { status = "Error", message = "Skill Already Added This Employee..." });
                    case "User Not exists":
                        return StatusCode(StatusCodes.Status404NotFound,
             new ResponseStatus { status = "Error", message ="User Not Found" });
                }
               

            }
            return StatusCode(StatusCodes.Status400BadRequest,
              new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPost]
        [Route("AddSkillWeightage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSkillWeightage(WeightageVM weightage)
        {
            if (ModelState.IsValid)
            {
                repository.AddSkillWeightage(weightage);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                     new ResponseStatus { status = "Success", message = "Weightage Added Successfully." });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseStatus { status = "Error", message = "Invalid Data." });
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeVM employee)
        {


            if (ModelState.IsValid && employee.EmployeeId != null)
            {

                string a = repository.UpdateEmployee(id, employee);

                switch (a)
                {
                    case "Updated":
                        return StatusCode(StatusCodes.Status201Created,
                        new ResponseStatus { status = "Success", message = "Employee Details Updated Successfully." });

                    case "User Not Exists":
                        return StatusCode(StatusCodes.Status404NotFound,
                        new ResponseStatus { status = "Not Found", message = "User Not Exists" });
                }

            }

            return StatusCode(StatusCodes.Status404NotFound,
                   new ResponseStatus { status = "Error", message = "Invalid Datas" });

        }

        [HttpPut]
        [Route("UpdateDepertment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepertment(int id, DepartmentVM department)
        {
            if (ModelState.IsValid)
            {
                string a = repository.UpdateDepertment(id, department);

                switch (a)
                {

                    case "Updated":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                           new ResponseStatus { status = "Success", message = "Department Updated Successfully" });

                    case "Department Not Exists":

                        return StatusCode(StatusCodes.Status404NotFound,
                           new ResponseStatus { status = "Success", message = "Department Not Exists" });

                }


            }
            return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "Invalid Datas" });

        }


        [HttpPut]
        [Route("UpdateDesignation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDesignation(int id, DesignationVM designation)
        {
            if (ModelState.IsValid)
            {
                string a = repository.UpdateDesignation(id, designation);

                switch (a)
                {
                    case "Updated":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                          new ResponseStatus { status = "Success", message = "Designation Updated Successfully" });

                    case "Designation Not Exists":

                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Department Not Exists" });
                }


            }
            return StatusCode(StatusCodes.Status400BadRequest,
             new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPut]
        [Route("UpdateSkill")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkill(int id, SkillsVM skill)
        {
            if (ModelState.IsValid)
            {
                string a = repository.UpdateSkill(id, skill);
                switch (a)
                {
                    case "Updated":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                            new ResponseStatus { status = "Success", message = "Skill Updated SuccessFully" });

                    case "Designation Not Exists":

                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Skill Not Exists" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
            new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPut]
        [Route("ReqForUpdateLvl")]
        public async Task<IActionResult> ReqForUpdateLvl(UserLevelVM level)
        {
            if (ModelState.IsValid)
            {
                var a = repository.ReqForUpdateLvl(level);

                switch (a)
                {
                    case "Ok":
                        return StatusCode(StatusCodes.Status102Processing,
                            new ResponseStatus { status = "Success", message = "Your Update Request Send Successfully" });
                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "User Not Exsist" });
                }
            
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                             new ResponseStatus { status = "Error", message = "Invalid datas" });
        }


        [HttpPost]
        [Route("LevlelApprovedSuccess")]
        public async Task<IActionResult> LevlelApprovedSuccess( int reqid, bool status)
        {
            if(ModelState.IsValid) 
            {
                var approved = repository.LevlelApprovedSuccess(reqid, status);
            }
           

            return Ok();
        }



        [HttpPut]
        [Route("UpdateLevelForEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLevelForEmployee(UserLevelVM level)
        {
            if (ModelState.IsValid)
            {
                var a = repository.UpdateLevelForEmployee(level);

                switch (a)
                {
                    case "Updated":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                      new ResponseStatus { status = "Success", message = "Level Updated Successfully." });

                    case "Error":
                        return StatusCode(StatusCodes.Status404NotFound,
                           new ResponseStatus { status = "Error", message = "Level not updated" });
                    case "User Not Exist":
                        return StatusCode(StatusCodes.Status404NotFound,
                           new ResponseStatus { status = "Error", message = "User Not Exist" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPut]
        [Route("UpdateSkillWeightage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkillWeightage(WeightageVM weightage)
        {
            if (ModelState.IsValid)
            {
                var a = repository.UpdateSkillWeightage(weightage);
                switch (a)
                {
                    case "Updated":
                        repository.Save();
                        return StatusCode(StatusCodes.Status201Created,
                            new ResponseStatus { status = "Success", message = "Weightage Updated SuccessFully" });

                    case "Skill Not Exists":
                        return StatusCode(StatusCodes.Status404NotFound,
                            new ResponseStatus { status = "Error", message = "Skill Not Exists" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
             new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpGet]
        [Route("EmployeeModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> EmployeeModule()
        {
            var employeeList = repository.EmployeeList().ToList();

            return Ok(employeeList);
        }


        [HttpGet]
        [Route("EmployeeById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> EmployeeById(int id)
        {
            var EmployeeId = repository.EmployeeById(id);
            if (EmployeeId == null)
            {
                return Ok("User Data Unavailable");
            }
            return Ok(EmployeeId);
        }



        [HttpGet]
        [Route("GetEmployeeByDepartment")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeByDepartment(int Id)
        {
            var EmpById = repository.EmployeeByDepartment(Id).ToList();

            return Ok(EmpById);
        }

        [HttpGet]
        [Route("DepartmentModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DepartmentModule()
        {
            var departmentlist = repository.DepartmentModule().ToList();

            return Ok(departmentlist);
        }

        [HttpGet]
        [Route("SkillsModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SkillsModule()
        {
            var skillList = repository.SkilsList().ToList();
            return Ok(skillList);
        }


        [HttpGet]
        [Route("SkillbyDepartmentID")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SkillbyDepartmentID(int id)
        {
            var skillbydeptid = repository.SkillbyDepartmentID(id).ToList();
            return Ok(skillbydeptid);
        }


        [HttpGet]
        [Route("DesignationModule")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DesignationModule()
        {
            var desig = repository.DesignationModule().ToList();
            return Ok(desig);
        }

        [HttpGet]
        [Route("GetEmployeeSkillsById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeSkillsById(int EmployeeId)
        {
            var Employee = repository.GetEmployeeSkillsById(EmployeeId).ToList();
            return Ok(Employee);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            var a = repository.DeleteEmployee(EmployeeId);

            switch (a)
            {
                case "Deleted":
                    repository.Save();
                    return StatusCode(StatusCodes.Status200OK,
                        new ResponseStatus { status = "Success", message = "Employee Details Deleted SuccessFully" });
                case "Error":
                    return StatusCode(StatusCodes.Status404NotFound,
                           new ResponseStatus { status = "Error", message = "Employee not found" });

            }
            return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Something Error" });
        }


        [HttpDelete]
        [Route("DeleteSkillbyEmp")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSkillbyEmp(int EmpployeeId, int SkillId)
        {
            var a = repository.DeleteSkillbyEmp(EmpployeeId, SkillId);
            switch (a)
            {
                case "Employee Skill Removed":
                    repository.Save();
                    return StatusCode(StatusCodes.Status200OK,
                       new ResponseStatus { status = "Success", message = "Employee Skill Deleted SuccessFully" });

                case "Error":
                    return StatusCode(StatusCodes.Status404NotFound,
                          new ResponseStatus { status = "Error", message = "Employee Skill not found" });

            }

            return StatusCode(StatusCodes.Status404NotFound,
                        new ResponseStatus { status = "Error", message = "Something Error" });
        }

        [HttpGet]
        [Route("FindRequiredEmployee")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> FindRequiredEmployee([FromQuery] FindEmployee find)
        {
            var emplist = repository.FindRequiredEmployee(find);

            return Ok(emplist);
        }

        //[HttpGet]
        //[Route("LevelupManager")]
        ////[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ApproveEmail([FromQuery] bool IsReject,int EmployeeId, int SkillId)
        //{
            

        //    return Ok();
        //}
    }
}

