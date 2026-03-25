using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public ReservationsController(AppDataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservations()
        {
            return await _context.Reservations
                .Include(r => r.Facility)
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
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReservationResponseDto>> CreateReservation(CreateReservationRequestDto dto)
        {
            // validate time
            if (dto.StartAt >= dto.EndAt)
                return BadRequest("Konec rezervace musí být po jejím začátku.");

            // validate data
            var user = await _context.Users.FindAsync(dto.UserId);
            var facility = await _context.Facilities.Include(f => f.Sports).FirstOrDefaultAsync(f => f.Id == dto.FacilityId);

            if (user == null || facility == null)
                return BadRequest("Uživatel nebo hala neexistuje.");

            
            // Is facility free?
            bool isOccupied = await _context.Reservations.AnyAsync(r =>
                r.FacilityId == dto.FacilityId &&
                r.Status == "ACTIVE" &&
                dto.StartAt < r.EndAt && dto.EndAt > r.StartAt);

            if (isOccupied)
                return Conflict("V tomto čase je sportoviště již obsazeno.");

            // Price calculate
            var priceSetting = await _context.PriceLists.FirstOrDefaultAsync(p =>
                p.FacilityId == dto.FacilityId &&
                p.Membership == user.Membership); 

            if (priceSetting == null)
                return BadRequest("Pro vaše členství a toto sportoviště není definována cena.");

            double hours = (dto.EndAt - dto.StartAt).TotalHours;
            decimal totalPrice = priceSetting.PricePerHour * (decimal)hours;

            // save
            var reservation = new Reservation
            {
                UserId = dto.UserId,
                FacilityId = dto.FacilityId,
                SportId = priceSetting.SportId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = "ACTIVE",
                TotalPrice = totalPrice
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var response = new ReservationResponseDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                FacilityId = reservation.FacilityId,
                FacilityName = facility.Name,
                StartAt = reservation.StartAt,
                EndAt = reservation.EndAt,
                Status = reservation.Status,
                TotalPrice = reservation.TotalPrice
            };

            return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, response);
        }

        //Change rezervation status to cancelled
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            reservation.Status = "CANCELLED";
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //Get rentals
        [Authorize]
        [HttpGet("{id}/equipmentrentals")]
        public async Task<ActionResult<IEnumerable<object>>> GetRentalsByReservation(int id)
        {
            var reservationExists = await _context.Reservations.AnyAsync(r => r.Id == id);
            if (!reservationExists)
            {
                return NotFound($"Rezervace s ID {id} nebyla nalezena.");
            }

            var rentals = await _context.EquipmentRentals
                .Include(r => r.Equipment) 
                .Where(r => r.ReservationId == id)
                .Select(r => new
                {
                    r.Id,
                    r.EquipmentId,
                    EquipmentName = r.Equipment != null ? r.Equipment.Name : "Neznámé vybavení",
                    r.Qty,
                    r.StartAt,
                    r.EndAt
                })
                .ToListAsync();

            return Ok(rentals);
        }

    }
}