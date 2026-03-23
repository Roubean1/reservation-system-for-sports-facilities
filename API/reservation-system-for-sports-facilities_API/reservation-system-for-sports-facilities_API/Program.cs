using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using reservation_system_for_sports_facilities_API;

namespace reservation_system_for_sports_facilities_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- 1. KONFIGURACE SLUŽEB (Dependency Injection) ---

            // JWT Konfigurace
            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT Key is missing in configuration.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

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
                    ValidateIssuer = false,   // Pro produkci doporučeno zapnout
                    ValidateAudience = false, // Pro produkci doporučeno zapnout
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Odstraní výchozí 5min toleranci expirace
                };
            });

            builder.Services.AddAuthorization();

            // Databáze (SQLite)
            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=database.db"));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 1. CORS MUSÍ BÝT PRVNÍ (před redirektem a autentizací)
            app.UseCors();

            // 2. HTTPS Redirection až POTÉ, co CORS povolil komunikaci
            // TIP: Pokud vyvíjíš lokálně a frontend máš na HTTP, 
            // můžeš tento řádek pro testování zakomentovat.
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // --- 3. INICIALIZACE DATABÁZE ---

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<AppDataContext>();

                    // Provede migrace
                    db.Database.Migrate();

                    // Seedování dat, pokud je tabulka Sports prázdná
                    if (!db.Sports.Any())
                    {
                        var sqlPath = Path.Combine(AppContext.BaseDirectory, "test_data_insert.sql");

                        // Záložní cesta, pokud soubor není v BaseDirectory (např. při vývoji)
                        if (!File.Exists(sqlPath))
                        {
                            sqlPath = "test_data_insert.sql";
                        }

                        if (File.Exists(sqlPath))
                        {
                            var sql = File.ReadAllText(sqlPath);
                            db.Database.ExecuteSqlRaw(sql);
                            Console.WriteLine("Databáze byla úspěšně naplněna daty.");
                        }
                        else
                        {
                            Console.WriteLine("Varování: SQL soubor pro inicializaci nebyl nalezen.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Došlo k chybě při inicializaci databáze.");
                }
            }

            app.Run();
        }
    }
}