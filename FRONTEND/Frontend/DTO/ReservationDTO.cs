using System;

namespace Frontend.DTO
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Status { get; set; } = "pending";
        public decimal TotalPrice { get; set; }
    }

    public class CreateReservationRequestDto
    {
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public int SportId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
