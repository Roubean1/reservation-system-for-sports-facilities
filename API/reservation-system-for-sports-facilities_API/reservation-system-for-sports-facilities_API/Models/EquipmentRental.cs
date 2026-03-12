namespace reservation_system_for_sports_facilities_API.Models
{
    public class EquipmentRental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        public int? ReservationId { get; set; } // Nullable, pokud půjčovné není vázané na rezervaci kurtu
        public int Qty { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public User? User { get; set; }
        public Equipment? Equipment { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
