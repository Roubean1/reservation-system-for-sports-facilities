namespace reservation_system_for_sports_facilities_API.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    }
}
