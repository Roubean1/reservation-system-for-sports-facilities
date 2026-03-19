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


        //Endpointy pro všechny uživatele
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueResponseDto>>> GetVenues()
        {
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


        //Získání sporotovišť
        [HttpGet("{id}/Facilities")]
        public async Task<ActionResult<IEnumerable<FacilityResponseDto>>> GetFacilitiesByVenue(int id)
        {
            var venueExists = await _context.Venues.AnyAsync(v => v.Id == id);
            if (!venueExists) return NotFound($"Venue s ID {id} nenalezeno.");

            var facilities = await _context.Facilities
                .Include(f => f.Sports)
                .Where(f => f.VenueId == id)
                .Select(f => new FacilityResponseDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    VenueId = f.VenueId,
                    Sports = f.Sports.Select(s => new SportDto { Id = s.Id, Name = s.Name }).ToList()
                })
                .ToListAsync();

            return Ok(facilities);
        }

        //Endpointy pro admina
        //Vytvoření
        [HttpPost]
        public async Task<ActionResult<VenueResponseDto>> CreateVenue(CreateVenueRequestDto dto)
        {
            var venue = new Venue
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            var response = new VenueResponseDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City
            };

            return CreatedAtAction(nameof(GetVenues), new { id = venue.Id }, response);
        }

        //Editace
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenue(int id, CreateVenueRequestDto dto)
        {
            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                return NotFound($"Sportoviště s ID {id} neexistuje.");
            }

            venue.Name = dto.Name;
            venue.Address = dto.Address;
            venue.City = dto.City;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Chyba při ukládání do databáze.");
            }

            return NoContent();
        }

        // Smazání 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Získání vybavení konrétního areálu
        [HttpGet("{venueId}/equipments")]
        public async Task<ActionResult<IEnumerable<EquipmentResponseDto>>> GetEquipmentByVenue(int venueId)
        {
            var venueExists = await _context.Venues.AnyAsync(v => v.Id == venueId);
            if (!venueExists) return NotFound($"Areál s ID {venueId} neexistuje.");

            return await _context.Equipments
                .Where(e => e.VenueId == venueId)
                .Select(e => new EquipmentResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    QuantityAvailable = e.Quantity,
                    PricePerHour = e.PricePerHour
                })
                .ToListAsync();
        }

    }
}