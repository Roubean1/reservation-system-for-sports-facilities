namespace reservation_system_for_sports_facilities_API.DTOs
{
    public class PriceListResponseDto
    {
        public int Id { get; set; }
        public string Membership { get; set; } = "standard";
        public decimal PricePerHour { get; set; }
        public string Currency { get; set; } = "CZK";
    }
}
