namespace reservation_system_for_sports_facilities_API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Cizí klíč
        public int FacilityId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Status { get; set; } = "pending";
        public decimal TotalPrice { get; set; }

        // Navigační vlastnosti
        public User? User { get; set; }
        public Facility? Facility { get; set; }
    }
}
