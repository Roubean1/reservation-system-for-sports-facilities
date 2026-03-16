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

            // KONFIGURACE M:N VAZBY (Facility <-> Sport)
            // V\tvoření facility_sports bez třídy
            modelBuilder.Entity<Facility>()
                .HasMany(f => f.Sports)
                .WithMany(s => s.Facilities)
                .UsingEntity<Dictionary<string, object>>(
                    "facility_sports", // Jméno tabulky v DB
                    j => j.HasOne<Sport>().WithMany().HasForeignKey("sport_id"),
                    j => j.HasOne<Facility>().WithMany().HasForeignKey("facility_id")
                );

            // KONVERZE DECIMAL NA DOUBLE (pro SQLite)
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