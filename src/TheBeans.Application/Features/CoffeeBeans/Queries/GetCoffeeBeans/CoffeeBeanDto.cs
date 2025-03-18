using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans
{
    public class CoffeeBeanDto
    {
        public string Id { get; set; } // Mapped from GUID
        public int Index { get; set; }
        public bool IsBOTD { get; set; }
        public string Cost { get; set; } // Storing as a string to keep "£" symbol
        public string Image { get; set; }
        public string Colour { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}