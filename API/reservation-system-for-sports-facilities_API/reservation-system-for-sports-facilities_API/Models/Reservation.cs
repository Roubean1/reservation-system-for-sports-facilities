namespace reservation_system_for_sports_facilities_API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public int SportId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public decimal TotalPrice { get; set; }

        public User? User { get; set; }
        public Facility? Facility { get; set; }
        public Sport? Sport { get; set; } 
    }
}
