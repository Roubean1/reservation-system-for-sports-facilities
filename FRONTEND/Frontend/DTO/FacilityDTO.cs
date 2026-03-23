using System.Collections.Generic;

namespace Frontend.DTO
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
    }
}
