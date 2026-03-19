namespace reservation_system_for_sports_facilities_API.DTOs
{
    public class EquipmentRentalResponseDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public int Qty { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int? ReservationId { get; set; }
    }
}
