using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Repo
{
    public interface taskInterface
    {

        Task<User> AddUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);

        //login

        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}
