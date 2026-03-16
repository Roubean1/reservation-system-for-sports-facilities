namespace reservation_system_for_sports_facilities_API.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public int SportId { get; set; }
        public string Membership { get; set; } = "BASIC";
        public decimal PricePerHour { get; set; }
        public string Currency { get; set; } = "CZK";

        public Facility? Facility { get; set; }
        public Sport? Sport { get; set; } 
    }
}
