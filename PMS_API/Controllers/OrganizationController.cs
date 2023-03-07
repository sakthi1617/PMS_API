using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeModule"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> addEmployee(EmployeeVM employeeModule)
        {
            if (ModelState.IsValid)
            {
                var a = repository.AddEmployee(employeeModule);

                if(a== "Created")
                {
                    repository.Save();
                    var msg = " Hi " + employeeModule.EmployeeName + "your Account created Succesfully";
                    var message = new Message(new string[] { employeeModule.EmailId }, "Welcome To PMS", msg.ToString());

                    _emailservice.SendEmail(message);

                    return StatusCode(StatusCodes.Status201Created,
                    new ResponseStatus { status = "Success", message = "Employee Added Successfully." });
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


        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeVM employee)
        {
            try
            {

                if (ModelState.IsValid && employee.EmployeeId != null)
                {
                    return repository.UpdateEmployee(id, employee) == "User Not Exists" ? StatusCode(StatusCodes.Status404NotFound,
                       new ResponseStatus { status = "Not Found", message = "User Not Exists" }) : StatusCode(StatusCodes.Status201Created,
                       new ResponseStatus { status = "Success", message = "Employee Details Updated Successfully." });

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                    new ResponseStatus { status = "Error", message = "Invalid Datas" });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                new ResponseStatus { status = "Error", message = ex.Message });
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EmployeeModule")]
        public async Task<IActionResult> EmployeeModule()
        {
            var employeeList = repository.EmployeeList().ToList();

            return Ok(employeeList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EmployeeByDesignation")]

        public async Task<IActionResult> EmployeeByDesignation(int Id)
        {
            var EmpById = repository.EmployeeByDesignation(Id).ToList();

            return Ok(EmpById);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SkillsModule")]
        public async Task<IActionResult> SkillsModule()
        {
            var skillList = repository.SkilsList().ToList();
            return Ok(skillList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("SkillbyID")]

        public async Task<IActionResult> SkillbyID(int id)
        {
            var skillbyid = repository.SkillbyID(id).ToList();
            return Ok(skillbyid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDesignation")]

        public async Task<IActionResult> AddDesignation(DepartmentVM department)
        {
            if (ModelState.IsValid)
            {
                repository.AddDesignation(department);
                repository.Save();
                return StatusCode(StatusCodes.Status201Created,
                   new ResponseStatus { status = "Success", message = "Designation Added Successfully" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
              new ResponseStatus { status = "Error", message = "Invalid Datas" });

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GoalsModule")]
        public async Task<IActionResult> GoalsModule()
        {
            return Ok();
        }



        //[HttpGet]
        //[Route("EmployeeListByManager")]
        //public async Task<IActionResult> ShowEmployeelist()
        //{
        //    var Employeelist = repository.ShowEmployeelist();
        //    return Ok(Employeelist);

        //}

    }



}
