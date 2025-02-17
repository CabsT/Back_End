using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginRegistrationApi.Models;
using LoginRegistrationApi.Data;
using BCrypt.Net;

namespace LoginRegistrationApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            
            // Check if email already exists
            var existingUser = await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
            if (existingUser)
            {
                return BadRequest(new { error = "Email already in use." });
            }

            // Hash password before saving
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            if (existingUser == null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }


            // Compare password with stored hash
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, existingUser.Password))
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            return Ok(new { message = "Login successful!" });
        }
    }
}
