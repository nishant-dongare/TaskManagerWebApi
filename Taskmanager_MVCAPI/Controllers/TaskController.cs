using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Taskmanager_MVCAPI.Models;
using Taskmanager_MVCAPI.Repo;

namespace Taskmanager_MVCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly taskInterface _userRepository;

        public TaskController(taskInterface userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            if (userDto.Password != userDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(userDto.Password, salt);

            User user = new User
            {
                Username = userDto.Username,
                Password = hashedPassword,
                Salt = salt,
                Batch = userDto.Batch,
                Email = userDto.Email,
                FullName = userDto.FullName,
                Role = "Trainee"
            };

            await _userRepository.AddUserAsync(user);

            return Ok("Registration successful.");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            bool isValidUser = await _userRepository.ValidateUserCredentialsAsync(loginDto.Username, loginDto.Password);
            if (!isValidUser)
            {
                return Unauthorized("Invalid username or password.");
            }

            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FullName,
                user.Batch,
                user.Role
            });
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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
