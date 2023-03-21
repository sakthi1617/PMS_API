using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using PMS_API.SupportModel;
using PMS_API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrganizationAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly PMSContext _context;
        private readonly IEmailService _emailservice;

        public OrganizationAuthController(IConfiguration configuration, IPasswordService passwordService, PMSContext context, IEmailService emailService)   
        {
            _configuration = configuration;
            _passwordService = passwordService;
            _context = context;
            _emailservice = emailService;
        }

        [HttpPost]
        [Route("adminlogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginCredential model)
        {
            var exisitingUser = await _context.EmployeeModules!.FirstOrDefaultAsync(user => user.Email == model.Username);

            if (exisitingUser == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseStatus { status = "Error", message = "Please Register!.." });
            }
            else if (!_passwordService.VerifyPasssword(model.Password, exisitingUser.PasswordHash, exisitingUser.PasswordSalt))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                     new ResponseStatus { status = "Error", message = "Please Enter Password Correctly" });
            }
            else
            {
                var userRole = _context.Roles.SingleOrDefault(x => x.RollId.Equals(exisitingUser.RoleId)).RollName;
                var authClaims = new List<Claim>
                      {
                              new Claim(ClaimTypes.Name, model.Username),
                              new Claim(ClaimTypes.Role, userRole.ToString())
                       };



                var token = GenerateToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    ResponseStatus = new { status = "Success", message = "Login Successfully." }

                });
            }
        }

        [HttpPost]
        [Route("GeneratetPassword")]
        public async Task<IActionResult> GeneratePassword(string Email, ResetPassword request)
        {

            if (Email != null)
            {
                var A = _passwordService.GeneratePassword(Email, request).ToString();
                if (A == "Your Account has Already Activated")
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new ResponseStatus { status = "Success", message = A });
                }
                return StatusCode(StatusCodes.Status201Created, new ResponseStatus { status = "Success", message = "Your Password has been Created." });
            }

            return StatusCode(StatusCodes.Status404NotFound, new ResponseStatus { status = "Error", message = "Id Not Found" });
        }
        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddMinutes(45),
                signingCredentials: signIn);

            return token;
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Email, ResetPassword request)
        {

            if (Email != null)
            {
                var A = _passwordService.Forgetpassword(Email, request).ToString();
                if (A == "Your Account has Already Activated")
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new ResponseStatus { status = "Success", message = A });
                }
                return StatusCode(StatusCodes.Status201Created, new ResponseStatus { status = "Success", message = "Your Password has been Created." });
            }

            return StatusCode(StatusCodes.Status404NotFound, new ResponseStatus { status = "Error", message = "Id Not Found" });
        }




        [HttpPost]
        [Route("Forgetpassword")]
        public async Task<IActionResult> Forgetpassword(string Email, int id)
        {
            var user = _context.EmployeeModules.Where(x => x.EmployeeId== id && x.Email == Email && x.IsDeleted == false && x.IsActivated == true).FirstOrDefault();
            if (user != null)
            {
                var msg = " Hi " + user.Name +  " Generate your new password, please click the following link: https://localhost:7099/api/OrganizationAuth/ResetPassword?Email= " + user.Email;
                var message = new Message(new string[] { user.Email }, "Forget Password", msg.ToString());
                _emailservice.SendEmail(message);
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseStatus { status = "Success", message = "Forget Password Link Send on Your Mail." });
        } 
    }

}
