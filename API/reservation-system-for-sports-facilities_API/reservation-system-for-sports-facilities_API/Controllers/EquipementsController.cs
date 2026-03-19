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

        //Add equipment
        [HttpPost]
        public async Task<ActionResult<EquipmentResponseDto>> CreateEquipment(Equipment equipment)
        {
            // Validace existence areálu
            var venueExists = await _context.Venues.AnyAsync(v => v.Id == equipment.VenueId);
            if (!venueExists) return BadRequest("Zadané VenueId neexistuje.");

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

        // update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id) return BadRequest();

            var existingEquipment = await _context.Equipments.FindAsync(id);
            if (existingEquipment == null) return NotFound();

            existingEquipment.Name = equipment.Name;
            existingEquipment.Quantity = equipment.Quantity;
            existingEquipment.PricePerHour = equipment.PricePerHour;
            existingEquipment.VenueId = equipment.VenueId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Equipments.AnyAsync(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // delete
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