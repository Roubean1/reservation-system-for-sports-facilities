using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.DTOs;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportTicketsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public SupportTicketsController(AppDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SupportTicketResponseDto>> CreateTicket(SupportTicket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = "OPEN";

            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync();

            var response = new SupportTicketResponseDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                Email = ticket.Email,
                Subject = ticket.Subject,
                Message = ticket.Message,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt
            };

            return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupportTicketResponseDto>>> GetTickets()
        {
            return await _context.SupportTickets
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new SupportTicketResponseDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Email = t.Email,
                    Subject = t.Subject,
                    Message = t.Message,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.SupportTickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ticket.Status = "RESOLVED";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}