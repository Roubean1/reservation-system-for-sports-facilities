namespace Frontend.DTO
{
    public class EquipmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int QuantityAvailable { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
