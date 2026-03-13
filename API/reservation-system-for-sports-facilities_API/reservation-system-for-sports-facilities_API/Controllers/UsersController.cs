using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;
using BCrypt.Net;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDataContext _context;

        public UsersController(AppDataContext context)
        {
            _context = context;
        }

        // všechny účty
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    Membership = u.Membership
                })
                .ToListAsync();
        }

        // Detail účtu
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    Membership = u.Membership
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("Uživatel nebyl nalezen.");

            return user;
        }

        // Registrace
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserRequestDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Uživatel s tímto emailem již existuje.");
            }

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                Membership = "standard"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Membership = user.Membership
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }

        // Editace
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, CreateUserRequestDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Email = dto.Email;
            user.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //přihlášení (/api/users/login)
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Neplatný email nebo heslo.");
            }

            var response = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Membership = user.Membership
            };

            return Ok(response);
        }
    }
}