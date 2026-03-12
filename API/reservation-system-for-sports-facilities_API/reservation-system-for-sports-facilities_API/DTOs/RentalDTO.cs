namespace reservation_system_for_sports_facilities_API.DTOs
{
    public class CreateRentalRequestDto
    {
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        public int? ReservationId { get; set; }
        public int Qty { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
