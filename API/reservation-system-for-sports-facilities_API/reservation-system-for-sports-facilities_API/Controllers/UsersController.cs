using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDataContext _context;
        private readonly IConfiguration _config;

        public UsersController(AppDataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string CreateToken(User user)
        {
            // Získání názvu role, nebo "Customer" jako fallback, pokud by Include selhalo
            string roleName = user.Role?.Name ?? "Customer";

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
        new Claim(ClaimTypes.Role, roleName), // Teď už user.Role.Name nezpůsobí pád
        new Claim("Membership", user.Membership)
    };

            var keyStr = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // všechny účty
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        [HttpGet("{id:int}")]
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
                Membership = "STANDART",
                RoleId = 3
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
        [Authorize]
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
        public async Task<ActionResult<object>> Login(LoginRequestDto dto)
        {
            // PŘIDÁNO .Include(u => u.Role), aby user.Role nebyl null
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Neplatný email nebo heslo.");
            }

            var token = CreateToken(user);

            return Ok(new
            {
                Token = token,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Membership = user.Membership
                }
            });
        }

        //Získání všech rezervací uživatele
        [Authorize]
        [HttpGet("{id}/reservations")]
        public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetUserReservations(int id)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == id);
            if (!userExists) return NotFound($"Uživatel s ID {id} neexistuje.");

            var reservations = await _context.Reservations
                .Include(r => r.Facility)
                .Where(r => r.UserId == id)
                .OrderByDescending(r => r.StartAt)
                .Select(r => new ReservationResponseDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    FacilityId = r.FacilityId,
                    FacilityName = r.Facility != null ? r.Facility.Name : "Neznámé",
                    StartAt = r.StartAt,
                    EndAt = r.EndAt,
                    Status = r.Status,
                    TotalPrice = r.TotalPrice
                })
                .ToListAsync();

            return Ok(reservations);
        }

        // Získání zapůjček uživatele
        [Authorize]
        [HttpGet("{userId}/equipmentrentals")]
        public async Task<ActionResult<IEnumerable<EquipmentRentalResponseDto>>> GetUserRentals(int userId)
        {
            return await _context.EquipmentRentals
                .Where(r => r.UserId == userId)
                .Include(r => r.Equipment)
                .OrderByDescending(r => r.StartAt)
                .Select(r => new EquipmentRentalResponseDto
                {
                    Id = r.Id,
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment != null ? r.Equipment.Name : "Neznámé",
                    Qty = r.Qty,
                    StartAt = r.StartAt,
                    EndAt = r.EndAt,
                    ReservationId = r.ReservationId
                })
                .ToListAsync();
        }

        [Authorize]
        [HttpGet("{userId}/SupportTickets")]
        public async Task<ActionResult<IEnumerable<SupportTicketResponseDto>>> GetUserTickets(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists) return NotFound($"Uživatel s ID {userId} neexistuje.");

            return await _context.SupportTickets
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new SupportTicketResponseDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Email = t.Email,
                    Subject = t.Subject,
                    Message = t.Message,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }
    }
}