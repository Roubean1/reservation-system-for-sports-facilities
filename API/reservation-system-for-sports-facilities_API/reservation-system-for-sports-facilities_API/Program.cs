
using Microsoft.EntityFrameworkCore;

namespace reservation_system_for_sports_facilities_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseSqlite("Data Source=database.db"));

            // PRIDANO: Nastavení CORS pro komunikaci s frontendem
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(); // PRIDANO: Aktivace CORS
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();
                db.Database.Migrate(); // ODKOMENTOVANO: Automatická migrace
            }

            app.Run();
        }
    }
}
