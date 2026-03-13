using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacilitiesController : ControllerBase
    {
        private readonly AppDataContext _context;

        public FacilitiesController(AppDataContext context)
        {
            _context = context;
        }

        // Získání všech
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacilityResponseDto>>> GetFacilities()
        {
            return await _context.Facilities
                .Select(f => new FacilityResponseDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    VenueId = f.VenueId,
                    SportId = f.SportId
                })
                .ToListAsync();
        }

        // Vytvoření
        [HttpPost]
        public async Task<ActionResult<FacilityResponseDto>> CreateFacility(CreateFacilityRequestDto dto)
        {
            if (!await _context.Venues.AnyAsync(v => v.Id == dto.VenueId) ||
                !await _context.Sports.AnyAsync(s => s.Id == dto.SportId))
            {
                return BadRequest("Zadané VenueId nebo SportId neexistuje.");
            }

            var facility = new Facility
            {
                Name = dto.Name,
                VenueId = dto.VenueId,
                SportId = dto.SportId
            };

            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();

            var response = new FacilityResponseDto
            {
                Id = facility.Id,
                Name = facility.Name,
                VenueId = facility.VenueId,
                SportId = facility.SportId
            };

            return CreatedAtAction(nameof(GetFacilities), new { id = facility.Id }, response);
        }

        // editace
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, CreateFacilityRequestDto dto)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();

            if (!await _context.Venues.AnyAsync(v => v.Id == dto.VenueId) ||
                !await _context.Sports.AnyAsync(s => s.Id == dto.SportId))
            {
                return BadRequest("Zadané VenueId nebo SportId neexistuje.");
            }

            facility.Name = dto.Name;
            facility.VenueId = dto.VenueId;
            facility.SportId = dto.SportId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}