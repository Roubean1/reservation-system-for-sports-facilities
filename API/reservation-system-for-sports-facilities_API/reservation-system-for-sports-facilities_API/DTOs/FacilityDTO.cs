namespace reservation_system_for_sports_facilities_API.DTOs
{
    public class FacilityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VenueId { get; set; }
        public List<SportDto> Sports { get; set; } = new List<SportDto>();
    }

    public class CreateFacilityRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int VenueId { get; set; }
        public int SportId { get; set; }
        public List<int> SportIds { get; set; } = new List<int>();
    }
}
