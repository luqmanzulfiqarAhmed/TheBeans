using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheBeans.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(AppDbContext context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!await _context.CoffeeBeans.AnyAsync())
                {
                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "TheBeans.Infrastructure", "SeedData", "coffeeBeans.json");
                    if (!File.Exists(jsonPath))
                    {
                        _logger.LogError("JSON file not found: {JsonPath}", jsonPath);
                        return;
                    }

                    var jsonData = await File.ReadAllTextAsync(jsonPath);
                    var coffeeBeans = JsonSerializer.Deserialize<List<CoffeeBean>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (coffeeBeans != null)
                    {
                        await _context.CoffeeBeans.AddRangeAsync(coffeeBeans);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Seeded {Count} coffee beans.", coffeeBeans.Count);
                    }
                }
                else
                {
                    _logger.LogInformation("Database already seeded.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}