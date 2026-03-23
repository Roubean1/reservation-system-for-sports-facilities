namespace reservation_system_for_sports_facilities_API.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // admin, employee, customer


        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
