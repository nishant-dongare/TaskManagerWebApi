using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Taskmanager_MVCAPI.Data;
using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Repo
{
    public class TaskService:taskInterface
    {
        private readonly ApplicationDbContext db;

        public TaskService(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }



        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            string hashedPassword = HashPassword(password, user.Salt);
            return user.Password == hashedPassword;
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
