
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

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();



            //Init DB
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDataContext>();

                
                db.Database.Migrate();

                // Inserting data, when DB is empty
                if (!db.Sports.Any())
                {
                    try
                    {
                        var sqlPath = Path.Combine(AppContext.BaseDirectory, "test_data_insert.sql");

                        if (!File.Exists(sqlPath))
                        {
                            sqlPath = "test_data_insert.sql";
                        }

                        var sql = File.ReadAllText(sqlPath);
                        db.Database.ExecuteSqlRaw(sql);
                        Console.WriteLine("Databáze byla úspěšně naplněna daty.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Chyba při plnění databáze: {ex.Message}");
                    }
                }
            }

            app.Run();
        }
    }
}
