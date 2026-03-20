
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace reservation_system_for_sports_facilities_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtKey = builder.Configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,   // Pro vývoj zatím vypnuto
                    ValidateAudience = false, // Pro vývoj zatím vypnuto
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization();


            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseSqlite("Data Source=database.db"));

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

            app.UseAuthentication();
            app.UseAuthorization();

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
