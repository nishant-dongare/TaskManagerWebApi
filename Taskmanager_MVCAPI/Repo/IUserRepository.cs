using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI
{
    public interface IUserRepository
    {
        List<string> GetAllBatches();
        List<UserDto> GetUsersByBatch(string batch);
    }
}
