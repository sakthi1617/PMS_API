using PMS_API.Data;
using PMS_API.Models;
using PMS_API.Repository;
using System.Security.Cryptography;
using System.Text;

namespace PMS_API.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PMSContext _context;

        public PasswordService(PMSContext context)
        {
            _context = context;
        }

        public string ResetPassword(string Email, ResetPassword request)
        {

            var user = _context.EmployeeModules.FirstOrDefault(u => u.EmailId == Email);
            if (user == null)
            {
                return "Please Try Again";
            }
            if(user.IsActivated == false)
            {
                HashPassword(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsActivated = true;
                _context.EmployeeModules.Update(user);
                _context.SaveChanges();
                return "Your Password Generate Successfully";
            }
            else
            {
                return "Your Account has Already Activated";
            }

        }

        public void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasssword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return ComputeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
