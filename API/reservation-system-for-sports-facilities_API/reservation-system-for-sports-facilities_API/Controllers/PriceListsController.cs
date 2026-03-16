using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceListsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public PriceListsController(AppDataContext context)
        {
            _context = context;
        }


        // Add new price
        [HttpPost]
        public async Task<ActionResult<PriceListResponseDto>> CreatePrice(CreatePriceListRequestDto dto)
        {
            // Validate
            var facility = await _context.Facilities.Include(f => f.Sports).FirstOrDefaultAsync(f => f.Id == dto.FacilityId);
            if (facility == null) return BadRequest("Hala neexistuje.");

            if (!facility.Sports.Any(s => s.Id == dto.SportId))
            {
                return BadRequest("Tento sport se v dané hale neprovozuje.");
            }

            var priceList = new PriceList
            {
                FacilityId = dto.FacilityId,
                SportId = dto.SportId,
                Membership = dto.Membership.ToUpper(),
                PricePerHour = dto.PricePerHour,
                Currency = dto.Currency
            };

            _context.PriceLists.Add(priceList);
            await _context.SaveChangesAsync();

            var response = new PriceListResponseDto
            {
                Id = priceList.Id,
                FacilityId = priceList.FacilityId,
                SportId = priceList.SportId,
                Membership = priceList.Membership,
                PricePerHour = priceList.PricePerHour,
                Currency = priceList.Currency
            };

            return CreatedAtAction("GetPricesByFacility", "Facilities", new { facilityId = priceList.FacilityId },response);
        }

        // update price
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrice(int id, CreatePriceListRequestDto dto)
        {
            var priceList = await _context.PriceLists.FindAsync(id);
            if (priceList == null) return NotFound();

            priceList.Membership = dto.Membership.ToUpper();
            priceList.PricePerHour = dto.PricePerHour;
            priceList.Currency = dto.Currency;
            priceList.SportId = dto.SportId;
            priceList.FacilityId = dto.FacilityId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // delete price
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            var priceList = await _context.PriceLists.FindAsync(id);
            if (priceList == null) return NotFound();

            _context.PriceLists.Remove(priceList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}