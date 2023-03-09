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
    [Authorize(Roles = "Admin")]
    public class OrganizationController : ControllerBase
    {


        private readonly IOrganizationRepo repository;
        private readonly IEmailService _emailservice;


        public OrganizationController(IOrganizationRepo _repository, IEmailService emailservice)
        {
            repository = _repository;
            _emailservice = emailservice;
        }


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
                        var msg = " Hi " + employeeModule.Name + "your Account created Succesfully";
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
        public async Task<IActionResult> AddAdditionalSkills(UserLevelVM level)
        {
            if (ModelState.IsValid)
            {
                 repository.AddAdditionalSkills(level);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                   new ResponseStatus { status = "Success", message = "Skill Added Successfully" });

            }
            return StatusCode(StatusCodes.Status400BadRequest,
              new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPost]
        [Route("AddSkillWeightage")]
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
        [Route("UpdateLevelForEmployee")]
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
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
               new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }

        [HttpPut]
        [Route("UpdateSkillWeightage")]
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
        public async Task<IActionResult> EmployeeModule()
        {
            var employeeList = repository.EmployeeList().ToList();

            return Ok(employeeList);
        }


        [HttpGet]
        [Route("EmployeeById")]
        public async Task<IActionResult> EmployeeById(int id)
        {
            var EmployeeId = repository.EmployeeById(id);

            return Ok(EmployeeId);
        }



        [HttpGet]
        [Route("GetEmployeeByDepartment")]

        public async Task<IActionResult> GetEmployeeByDepartment(int Id)
        {
            var EmpById = repository.EmployeeByDepartment(Id).ToList();

            return Ok(EmpById);
        }

        [HttpGet]
        [Route("DepartmentModule")]
        public async Task<IActionResult> DepartmentModule()
        {
            var departmentlist = repository.DepartmentModule().ToList();

            return Ok(departmentlist);
        }

        [HttpGet]
        [Route("SkillsModule")]
        public async Task<IActionResult> SkillsModule()
        {
            var skillList = repository.SkilsList().ToList();
            return Ok(skillList);
        }


        [HttpGet]
        [Route("SkillbyDepartmentID")]

        public async Task<IActionResult> SkillbyDepartmentID(int id)
        {
            var skillbydeptid = repository.SkillbyDepartmentID(id).ToList();
            return Ok(skillbydeptid);
        }


        [HttpGet]
        [Route("DesignationModule")]

        public async Task<IActionResult> DesignationModule()
        {
            var desig = repository.DesignationModule().ToList();
            return Ok(desig);
        }

        [HttpGet]
        [Route ("GetEmployeeSkillsById")]
        public async Task<IActionResult> GetEmployeeSkillsById(int EmployeeId)
        {
           var Employee = repository.GetEmployeeSkillsById(EmployeeId).ToList();
            return Ok(Employee);
        }







    }
}
