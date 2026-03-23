using System;

namespace Frontend.DTO
{
    public class SupportTicketResponseDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "OPEN";
        public DateTime CreatedAt { get; set; }
    }
}