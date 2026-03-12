namespace reservation_system_for_sports_facilities_API.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }

        public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
