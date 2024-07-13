using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repo;

        public UserController(IUserRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("batches")]
        public IActionResult GetBatches()
        {
            List<string> batches = repo.GetAllBatches();
            return Ok(batches);
        }

        [HttpGet("GetUsersByBatch/{batch}")]
        public IActionResult GetUsersByBatch(string batch)
        {
            return Ok(repo.GetUsersByBatch(batch));
        }
    }
}
