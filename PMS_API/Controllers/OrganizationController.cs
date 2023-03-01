using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.ViewModels;
using static System.Net.WebRequestMethods;

namespace PMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {


        private readonly IOrganizationRepo repository;


        public OrganizationController(IOrganizationRepo _repository)
        {
            repository = _repository;
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
            if(ModelState.IsValid)
            {
                repository.AddEmployee(employeeModule);
                repository.Save();
                return  StatusCode(StatusCodes.Status201Created,
                new ResponseStatus { status = "Success", message = "Employee Added Successfully." });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseStatus { status = "Error", message = "Invalid Datas" });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSkills")]
        public async Task<IActionResult> addSkill(Skillset skill) 
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
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SkillsModule")]
        public async Task<IActionResult> SkillsModule()
        {
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AddSkills")]
        public async Task<IActionResult> GoalsModule()
        {
            return Ok();
        }

    }


  
}
