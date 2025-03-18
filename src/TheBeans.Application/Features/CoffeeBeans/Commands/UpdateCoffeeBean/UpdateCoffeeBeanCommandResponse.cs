using TheBeans.Application.Common.Responses;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean
{
 
/// <summary>
/// Represents the response returned after handling the <see cref="UpdateCoffeeBeanCommand"/>.
/// Inherits from <see cref="BaseResponse"/> to include common response properties like success status and messages.
/// </summary>
/// <remarks>
/// This response includes:
/// - The unique identifier (<see cref="Id"/>) of the updated coffee bean.
/// - A success message (default: "Coffee bean Updated successfully").
/// - Inherited properties from <see cref="BaseResponse"/> for validation errors and overall success status.
/// </remarks>
public class UpdateCoffeeBeanCommandResponse : BaseResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the updated coffee bean.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCoffeeBeanCommandResponse"/> class.
    /// </summary>
    public UpdateCoffeeBeanCommandResponse() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCoffeeBeanCommandResponse"/> class with the updated coffee bean's ID and a custom message.
    /// </summary>
    /// <param name="id">The unique identifier of the updated coffee bean.</param>
    /// <param name="message">The success message (default: "Coffee bean Updated successfully").</param>
    public UpdateCoffeeBeanCommandResponse(Guid id, string message = "Coffee bean Updated successfully")
        : base(message)
    {
        Id = id;
    }
}

}

