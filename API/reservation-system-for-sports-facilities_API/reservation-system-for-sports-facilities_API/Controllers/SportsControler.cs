using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SportsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public SportsController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SportDto>>> GetSports()
        {
            return await _context.Sports
                .Select(s => new SportDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<SportDto>> CreateSport(SportDto dto)
        {
            var sport = new Sport
            {
                Name = dto.Name
            };

            _context.Sports.Add(sport);
            await _context.SaveChangesAsync();

            dto.Id = sport.Id;
            return CreatedAtAction(nameof(GetSports), new { id = sport.Id }, dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSport(int id, SportDto dto)
        {
            var sport = await _context.Sports.FindAsync(id);
            if (sport == null) return NotFound();

            sport.Name = dto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSport(int id)
        {
            var sport = await _context.Sports.FindAsync(id);
            if (sport == null) return NotFound();

            _context.Sports.Remove(sport);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
