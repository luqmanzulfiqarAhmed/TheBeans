using MediatR;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean
{
    /// <summary>
    /// Command for creating a new coffee bean.
    /// This record represents the data required to create a coffee bean entry.
    /// </summary>
    /// <param name="Cost">The cost of the coffee bean, must be greater than zero.</param>
    /// <param name="Currency">The currency in which the cost is specified.</param>
    /// <param name="Image">The URL or path to the coffee bean's image.</param>
    /// <param name="Colour">The colour description of the coffee bean.</param>
    /// <param name="Name">The name of the coffee bean, must not be empty and should have a maximum length of 100 characters.</param>
    /// <param name="Description">A brief description of the coffee bean.</param>
    /// <param name="Country">The country of origin of the coffee bean.</param>
    public record CreateCoffeeBeanCommand
    (
        decimal Cost,
        string Currency,
        string Image,
        string Colour,
        string Name,
        string Description,
        string Country
    ) : IRequest<CreateCoffeeBeanCommandResponse>;
}

