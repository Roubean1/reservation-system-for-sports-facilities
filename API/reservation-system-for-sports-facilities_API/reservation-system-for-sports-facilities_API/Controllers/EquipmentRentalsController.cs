using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentRentalsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public EquipmentRentalsController(AppDataContext context)
        {
            _context = context;
        }

        // Get All
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRentals()
        {
            return await _context.EquipmentRentals
                .Include(r => r.User)
                .Include(r => r.Equipment)
                .OrderByDescending(r => r.StartAt)
                .Select(r => new {
                    r.Id,
                    r.UserId,
                    UserEmail = r.User != null ? r.User.Email : "Neznámý",
                    r.EquipmentId,
                    EquipmentName = r.Equipment != null ? r.Equipment.Name : "Neznámé",
                    r.Qty,
                    r.StartAt,
                    r.EndAt,
                    r.ReservationId
                })
                .ToListAsync();
        }

        // Create rental
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<object>> CreateRental(CreateRentalRequestDto dto)
        {
            // 1. Time check
            if (dto.StartAt >= dto.EndAt)
                return BadRequest("Konec výpůjčky musí být po jejím začátku.");

            var equipment = await _context.Equipments.FindAsync(dto.EquipmentId);
            if (equipment == null) return NotFound("Vybavení nebylo nalezeno.");

            // 2. Availability check 
            var rentedQty = await _context.EquipmentRentals
                .Where(r => r.EquipmentId == dto.EquipmentId &&
                            dto.StartAt < r.EndAt && dto.EndAt > r.StartAt)
                .SumAsync(r => r.Qty);

            int availableQty = equipment.Quantity - rentedQty;

            if (dto.Qty > availableQty)
            {
                return Conflict($"Nedostatek vybavení. K dispozici je pouze {availableQty} ks.");
            }

            // 3. Save
            var rental = new EquipmentRental
            {
                UserId = dto.UserId,
                EquipmentId = dto.EquipmentId,
                ReservationId = dto.ReservationId,
                Qty = dto.Qty,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt
            };

            _context.EquipmentRentals.Add(rental);
            await _context.SaveChangesAsync();

            var response = new
            {
                rental.Id,
                rental.UserId,
                rental.EquipmentId,
                rental.Qty,
                rental.StartAt,
                rental.EndAt,
                rental.ReservationId
            };

            return CreatedAtAction(nameof(GetRentals), new { id = rental.Id }, response);
        }

        [Authorize]
        //delete rental
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await _context.EquipmentRentals.FindAsync(id);
            if (rental == null) return NotFound();

            _context.EquipmentRentals.Remove(rental);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}