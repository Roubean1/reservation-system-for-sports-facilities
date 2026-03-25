using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public EquipmentsController(AppDataContext context)
        {
            _context = context;
        }

        //Get All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentResponseDto>>> GetEquipments()
        {
            return await _context.Equipments
                .Select(e => new EquipmentResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    QuantityAvailable = e.Quantity,
                    PricePerHour = e.PricePerHour
                })
                .ToListAsync();
        }

        // Přidání vybavení
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<ActionResult<EquipmentResponseDto>> CreateEquipment(CreateEquipmentRequestDto dto)
        {
            var venueExists = await _context.Venues.AnyAsync(v => v.Id == dto.VenueId);
            if (!venueExists) return BadRequest("Zadané VenueId neexistuje.");

            var equipment = new Equipment
            {
                VenueId = dto.VenueId,
                Name = dto.Name,
                Quantity = dto.Quantity,
                PricePerHour = dto.PricePerHour
            };

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();

            var response = new EquipmentResponseDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                QuantityAvailable = equipment.Quantity,
                PricePerHour = equipment.PricePerHour
            };

            return CreatedAtAction(nameof(GetEquipments), new { id = equipment.Id }, response);
        }

        // Editace vybavení
        [Authorize(Roles = "Admin,Employee")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, CreateEquipmentRequestDto dto)
        {
            var existingEquipment = await _context.Equipments.FindAsync(id);
            if (existingEquipment == null) return NotFound();

            var venueExists = await _context.Venues.AnyAsync(v => v.Id == dto.VenueId);
            if (!venueExists) return BadRequest("Zadané VenueId neexistuje.");

            existingEquipment.Name = dto.Name;
            existingEquipment.Quantity = dto.Quantity;
            existingEquipment.PricePerHour = dto.PricePerHour;
            existingEquipment.VenueId = dto.VenueId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        // delete
        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null) return NotFound();

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}