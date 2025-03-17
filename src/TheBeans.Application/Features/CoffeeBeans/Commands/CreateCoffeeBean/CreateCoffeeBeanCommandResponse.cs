using TheBeans.Application.Common.Responses;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean
{
    /// <summary>
    /// Response class for the CreateCoffeeBeanCommand.
    /// Inherits from BaseResponse to standardize response structure.
    /// </summary>
    public class CreateCoffeeBeanCommandResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the ID of the created coffee bean.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Default constructor that initializes the response with default success status.
        /// </summary>
        public CreateCoffeeBeanCommandResponse() : base()
        {
        }

        /// <summary>
        /// Constructor that initializes the response with a coffee bean ID and an optional message.
        /// </summary>
        /// <param name="id">The ID of the created coffee bean.</param>
        /// <param name="message">The response message, defaulting to "Coffee bean created successfully".</param>
        public CreateCoffeeBeanCommandResponse(string id, string message = "Coffee bean created successfully")
            : base(message)
        {
            Id = id;
        }
    }

}

