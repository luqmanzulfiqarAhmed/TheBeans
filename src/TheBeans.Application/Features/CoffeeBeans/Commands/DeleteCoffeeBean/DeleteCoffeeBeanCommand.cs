using MediatR;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean
{

    /// <summary>
    /// Command DTO for deleting a coffee bean entity.
    /// Follows CQRS pattern with MediatR for request/response handling.
    /// </summary>
    /// <param name="Id">Global unique identifier for the coffee bean to delete.</param>
    public record DeleteCoffeeBeanCommand(
        Guid Id
    ) : IRequest<DeleteCoffeeBeanCommandResponse>;
}

