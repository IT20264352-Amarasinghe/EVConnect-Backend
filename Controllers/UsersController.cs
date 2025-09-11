using Microsoft.AspNetCore.Mvc;
using EVChonnectService.Models;

namespace EVChonnectService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private static List<User> users = new List<User>();

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (users.Any(u => u.NIC == user.NIC))
                return BadRequest("User with this NIC already exists.");

            users.Add(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = users.FirstOrDefault(u => u.NIC == loginUser.NIC && u.Password == loginUser.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }

        [HttpPut("deactivate/{nic}")]
        public IActionResult Deactivate(string nic)
        {
            var user = users.FirstOrDefault(u => u.NIC == nic);
            if (user == null) return NotFound();

            user.IsActive = false;
            return Ok(user);
        }
    }
}
