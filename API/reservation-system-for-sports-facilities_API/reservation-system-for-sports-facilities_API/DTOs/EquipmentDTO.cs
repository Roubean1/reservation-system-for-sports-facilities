namespace reservation_system_for_sports_facilities_API.DTOs
{
    public class EquipmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int QuantityAvailable { get; set; }
        public decimal PricePerHour { get; set; }
    }

    public class CreateEquipmentRequestDto
    {
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
