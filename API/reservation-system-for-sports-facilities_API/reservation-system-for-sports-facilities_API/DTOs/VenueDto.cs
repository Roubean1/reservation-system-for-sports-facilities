namespace reservation_system_for_sports_facilities_API.DTOs
{

    public class VenueResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
    }

    public class CreateVenueRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
