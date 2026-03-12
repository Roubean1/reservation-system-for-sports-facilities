using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        private readonly AppDataContext _context;

        public VenuesController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueResponseDto>>> GetVenues()
        {
            // Převedeme modely na DTO (ručně, zatím bez AutoMapperu)
            return await _context.Venues
                .Select(v => new VenueResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Address = v.Address,
                    City = v.City
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<VenueResponseDto>> CreateVenue(CreateVenueRequestDto dto)
        {
            // Mapování z DTO na Model
            var venue = new Venue
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            // Vrátíme zpět Response DTO (včetně nového ID)
            var response = new VenueResponseDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City
            };

            return CreatedAtAction(nameof(GetVenues), new { id = venue.Id }, response);
        }
    }
}
