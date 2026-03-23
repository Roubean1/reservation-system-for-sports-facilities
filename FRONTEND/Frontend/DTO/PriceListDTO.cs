namespace Frontend.DTO
{
    public class PriceListResponseDto
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int SportId { get; set; }
        public string Membership { get; set; } = "BASIC";
        public decimal PricePerHour { get; set; }
        public string Currency { get; set; } = "CZK";
    }

    public class CreatePriceListRequestDto
    {
        public int FacilityId { get; set; }
        public int SportId { get; set; }
        public string Membership { get; set; } = "BASIC";
        public decimal PricePerHour { get; set; }
        public string Currency { get; set; } = "CZK";
    }
}