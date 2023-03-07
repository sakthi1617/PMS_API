using Microsoft.AspNetCore.Mvc;
using PMS_API.Models;

namespace PMS_API.Repository
{
    public interface IPasswordService
    {
        public string ResetPassword(string Email, ResetPassword request);
        public void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasssword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
