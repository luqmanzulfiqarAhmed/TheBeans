namespace TheBeans.Application.Common.Responses
{
    /// <summary>
    /// Base response class that serves as a standard response structure for application requests.
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// Defaults to true unless explicitly set otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A message describing the result of the operation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A list of validation errors, if any, encountered during the operation.
        /// </summary>
        public List<string> ValidationErrors { get; set; }

        /// <summary>
        /// Default constructor that initializes Success to true.
        /// </summary>
        public BaseResponse()
        {
            Success = true;
        }

        /// <summary>
        /// Constructor that initializes the response with a message.
        /// </summary>
        /// <param name="message">The response message.</param>
        public BaseResponse(string message = null)
        {
            Message = message;
            Success = true;
        }

        /// <summary>
        /// Constructor that initializes the response with a message and success status.
        /// </summary>
        /// <param name="message">The response message.</param>
        /// <param name="success">Indicates whether the operation was successful.</param>
        public BaseResponse(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }

}

