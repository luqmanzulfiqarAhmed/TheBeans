using TheBeans.Application.Common.Responses;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean
{
 
/// <summary>
/// Represents the response returned after handling the <see cref="DeleteCoffeeBeanCommand"/>.
/// Inherits from <see cref="BaseResponse"/> to include common response properties like success status and messages.
/// </summary>
/// <remarks>
/// This response includes:
/// - The unique identifier (<see cref="Id"/>) of the deleted coffee bean.
/// - A success message (default: "Coffee bean Deleted successfully").
/// - Inherited properties from <see cref="BaseResponse"/> for validation errors and overall success status.
/// </remarks>
public class DeleteCoffeeBeanCommandResponse : BaseResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the deleted coffee bean.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCoffeeBeanCommandResponse"/> class.
    /// </summary>
    public DeleteCoffeeBeanCommandResponse() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCoffeeBeanCommandResponse"/> class with the deleted coffee bean's ID and a custom message.
    /// </summary>
    /// <param name="id">The unique identifier of the deleted coffee bean.</param>
    /// <param name="message">The success message (default: "Coffee bean Deleted successfully").</param>
    public DeleteCoffeeBeanCommandResponse(Guid id, string message = "Coffee bean Deleted successfully")
        : base(message)
    {
        Id = id;
    }
}

}

