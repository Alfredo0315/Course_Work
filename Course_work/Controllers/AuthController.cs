using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course_Work.Models;
using System.Security.Cryptography;
using System.Text;

namespace Course_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EsportsDbContext _context;

        public AuthController(EsportsDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Login == request.Login && u.IsActive);

                if (user == null)
                {
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }

                var passwordHash = GetPasswordHash(request.Password);
                if (user.PasswordHash != passwordHash)
                {
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }

                return Ok(new
                {
                    id = user.ID_User,
                    login = user.Login,
                    name = user.Name,
                    role = user.Role,
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка авторизации", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<object>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Login == request.Login))
                {
                    return BadRequest(new { message = "Пользователь с таким логином уже существует" });
                }

                var user = new User
                {
                    Login = request.Login,
                    PasswordHash = GetPasswordHash(request.Password),
                    Role = request.Role ?? "User",
                    Name = request.Name,
                    Email = request.Email,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = user.ID_User,
                    login = user.Login,
                    name = user.Name,
                    role = user.Role,
                    message = "Пользователь успешно зарегистрирован"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка регистрации", error = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.IsActive)
                    .Select(u => new
                    {
                        u.ID_User,
                        u.Login,
                        u.Name,
                        u.Role,
                        u.Email,
                        u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка загрузки пользователей", error = ex.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "Пользователь не найден" });
                }

                user.IsActive = false;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Пользователь удален" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка удаления", error = ex.Message });
            }
        }
        [HttpGet("test-hash")]
        public ActionResult<object> TestHash(string password)
        {
            return Ok(new { 
                input = password,
                hash = GetPasswordHash(password),
                expected = "6ad14ba9986e3615423dfca256d04e3f"
            });
        }
        private string GetPasswordHash(string password)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}