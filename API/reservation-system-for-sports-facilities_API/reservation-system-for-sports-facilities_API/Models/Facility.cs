namespace reservation_system_for_sports_facilities_API.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SportId { get; set; }

        public Venue? Venue { get; set; }
        public Sport? Sport { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();
    }
}
