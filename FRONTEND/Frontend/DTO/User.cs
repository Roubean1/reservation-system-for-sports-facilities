using System;
using System.Collections.Generic;

namespace reservation_system_for_sports_facilities_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Membership { get; set; } = "standard";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relace: Jeden uživatel může mít mnoho rezervací
        //public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
