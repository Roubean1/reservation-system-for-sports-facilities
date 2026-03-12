namespace reservation_system_for_sports_facilities_API.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerHour { get; set; }

        public Venue? Venue { get; set; }
        public ICollection<EquipmentRental> Rentals { get; set; } = new List<EquipmentRental>();
    }
}
