using Microsoft.EntityFrameworkCore;
using reservation_system_for_sports_facilities_API.Models;

namespace reservation_system_for_sports_facilities_API
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<Sport> Sports => Set<Sport>();
        public DbSet<Facility> Facilities => Set<Facility>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Equipment> Equipments => Set<Equipment>();
        public DbSet<EquipmentRental> EquipmentRentals => Set<EquipmentRental>();
        public DbSet<PriceList> PriceLists => Set<PriceList>();
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SQLite nepodporuje typ decimal přímo, EF Core ho může převést na double automaticky:
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                }
            }
        }
    }
}
