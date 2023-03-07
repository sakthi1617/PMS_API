using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_API.Models;
using PMS_API.Repository;
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
       
        public OrganizationAuthController(IConfiguration configuration, IPasswordService passwordService)
        {
            _configuration = configuration;
            _passwordService = passwordService;
        }

        [HttpPost]
        [Route("adminlogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginCredential model)
        {
            if (model.Username != "Admin")
            {
                return BadRequest("User not found.");
            }

            if (model.Password != "Admin@123")
            {
                return BadRequest("Wrong password.");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };


            var token = GetToken(authClaims);
           

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                ResponseStatus =new { status = "Success", message = "Login Successfully." }
        
            });
            
        }

        


        private JwtSecurityToken GetToken(List<Claim> authClaims)
            {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return token;
        }




        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Email, ResetPassword request)
        {
            
           if (Email != null)
            {
               var A = _passwordService.ResetPassword(Email,request).ToString();
                if (A == "Your Account has Already Activated")
                {
                    return StatusCode(StatusCodes.Status208AlreadyReported, new ResponseStatus { status = "Success", message = A });
                }
                return StatusCode(StatusCodes.Status202Accepted,new ResponseStatus { status = "Success", message = "Your Password has been Created." });
            }

            return StatusCode(StatusCodes.Status404NotFound,new ResponseStatus { status = "Error", message = "Id Not Found" });
        }

    }

}
