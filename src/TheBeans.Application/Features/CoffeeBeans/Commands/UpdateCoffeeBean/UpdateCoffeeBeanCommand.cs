using MediatR;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean
{

/// <summary>
/// Command DTO for updating a coffee bean entity.
/// Follows CQRS pattern with MediatR for request/response handling.
/// </summary>
/// <param name="Id">Global unique identifier for the coffee bean</param>
/// <param name="Cost">Updated price of the coffee bean</param>
/// <param name="Currency">ISO currency code (e.g., USD, GBP)</param>
/// <param name="Image">URL/path to product image</param>
/// <param name="Colour">HEX code or color name for UI display</param>
/// <param name="Name">Updated product name</param>
/// <param name="Description">Updated marketing description</param>
/// <param name="Country">Origin country (ISO 3166 format recommended)</param>
    public record UpdateCoffeeBeanCommand
    (
        Guid Id,
        decimal Cost,
        string Currency,
        string Image,
        string Colour,
        string Name,
        string Description,
        string Country
    ) : IRequest<UpdateCoffeeBeanCommandResponse>;
}

