using Microsoft.AspNetCore.Mvc;
using PMS_API.SupportModel;

namespace PMS_API.Repository
{
    public interface IPasswordService
    {
        public string GeneratePassword(string Email, ResetPassword request);
        public void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasssword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
