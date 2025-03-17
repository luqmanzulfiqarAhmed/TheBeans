using System.Text.Json;

namespace TheBeans.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.CoffeeBeans.Any())
            {
                return; // Database has already been seeded
            }

            // Load JSON data
            var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "AllTheBeans.json");
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize JSON into entities
            var coffeeBeans = JsonSerializer.Deserialize<List<CoffeeBean>>(jsonData);

            if (coffeeBeans == null || !coffeeBeans.Any())
            {
                throw new InvalidOperationException("No data found in the JSON file.");
            }

            // Add entities to the database
            context.CoffeeBeans.AddRange(coffeeBeans);
            context.SaveChanges();
        }
    }
}