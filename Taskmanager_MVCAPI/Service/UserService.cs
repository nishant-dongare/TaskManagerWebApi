using Microsoft.EntityFrameworkCore;
using Taskmanager_MVCAPI.Data;
using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Service
{
    public class UserService : IUserRepository
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public List<UserDto> GetUsersByBatch(string batch)
        {
            var users = db.Users
                          .Where(u => u.Role == "Trainee" && u.Batch == batch)
                          .Select(u => new UserDto
                          {
                              Id = u.Id,
                              Username = u.Username,
                              Batch = u.Batch,
                              Email = u.Email,
                              FullName = u.FullName,
                              Role = u.Role
                          })
                          .ToList();

            return users;
        }

        public List<string> GetAllBatches()
        {
            return db.Users.Select(u => u.Batch).Distinct().ToList();
        }
    }
}
