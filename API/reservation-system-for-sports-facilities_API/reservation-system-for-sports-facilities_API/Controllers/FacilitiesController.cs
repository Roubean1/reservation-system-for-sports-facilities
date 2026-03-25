using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacilityResponseDto>>> GetFacilities()
        {
            return await _context.Facilities
                .Include(f => f.Sports) // Načtení M:N vazeb
                .Select(f => new FacilityResponseDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    VenueId = f.VenueId,
                    Sports = f.Sports.Select(s => new SportDto { Id = s.Id, Name = s.Name }).ToList()
                })
                .ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<FacilityResponseDto>> CreateFacility(CreateFacilityRequestDto dto)
        {
            var venue = await _context.Venues.FindAsync(dto.VenueId);
            if (venue == null) return BadRequest("Zadané VenueId neexistuje.");

            var sports = await _context.Sports
                .Where(s => dto.SportIds.Contains(s.Id))
                .ToListAsync();

            if (sports.Count == 0)
            {
                return BadRequest("Nebyl nalezen žádný platný sport pro zadaná ID.");
            }

            var facility = new Facility
            {
                Name = dto.Name,
                VenueId = dto.VenueId,
                Sports = sports
            };

            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();

            var response = new FacilityResponseDto
            {
                Id = facility.Id,
                Name = facility.Name,
                VenueId = facility.VenueId,
                Sports = facility.Sports.Select(s => new SportDto { Id = s.Id, Name = s.Name }).ToList()
            };

            return CreatedAtAction(nameof(GetFacilities), new { id = facility.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, CreateFacilityRequestDto dto)
        {
            var facility = await _context.Facilities.Include(f => f.Sports).FirstOrDefaultAsync(f => f.Id == id);
            if (facility == null) return NotFound();

            var sport = await _context.Sports.FindAsync(dto.SportId);
            if (sport == null) return BadRequest("SportId neexistuje.");

            facility.Name = dto.Name;
            facility.VenueId = dto.VenueId;

            // Jednoduchá logika: nahradíme stávající sporty tím novým z DTO
            facility.Sports.Clear();
            facility.Sports.Add(sport);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // Get prices for Facility
        [HttpGet("{facilityId}/prices")]
        public async Task<ActionResult<IEnumerable<PriceListResponseDto>>> GetPricesByFacility(int facilityId)
        {
            return await _context.PriceLists
                .Where(p => p.FacilityId == facilityId)
                .Select(p => new PriceListResponseDto
                {
                    Id = p.Id,
                    FacilityId = p.FacilityId,
                    SportId = p.SportId,
                    Membership = p.Membership,
                    PricePerHour = p.PricePerHour,
                    Currency = p.Currency
                })
                .ToListAsync();
        }

        [Authorize]
        [HttpGet("{facilityId}/reservations")]
        public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservationsByFacility(int facilityId)
        {
            var facilityExists = await _context.Facilities.AnyAsync(f => f.Id == facilityId);
            if (!facilityExists) return NotFound($"Hala s ID {facilityId} neexistuje.");

            var reservations = await _context.Reservations
                .Include(r => r.Facility)
                .Where(r => r.FacilityId == facilityId)
                .OrderBy(r => r.StartAt) 
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
    }
}