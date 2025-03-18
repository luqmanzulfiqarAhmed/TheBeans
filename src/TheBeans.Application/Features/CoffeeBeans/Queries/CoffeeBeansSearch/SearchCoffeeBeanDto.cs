
namespace TheBeans.Application.Features.CoffeeBeans.Queries.CoffeeBeansSearch
{
    public class SearchCoffeeBeanDto
    {
        public string Id { get; set; } // Mapped from GUID
        public bool IsBOTD { get; set; }
        public string Cost { get; set; } // Storing as a string to keep "£" symbol
        public string Image { get; set; }
        public string Colour { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}