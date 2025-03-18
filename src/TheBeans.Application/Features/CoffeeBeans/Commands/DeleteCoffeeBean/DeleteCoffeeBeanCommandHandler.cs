using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Common.Exceptions;
using TheBeans.Core.Interfaces.Repositories;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean
{

    /// <summary>
    /// Handles the <see cref="DeleteCoffeeBeanCommand"/> to delete an existing coffee bean entity.
    /// Implements MediatR's <see cref="IRequestHandler{TRequest, TResponse}"/> for CQRS pattern.
    /// </summary>
    /// <remarks>
    /// This handler performs the following steps:
    /// 1. Validates the incoming request using <see cref="DeleteCoffeeBeanCommandValidator"/>.
    /// 2. Logs validation errors if the request is invalid and throws an <see cref="AppValidationException"/>.
    /// 3. Retrieves the existing coffee bean entity from the database using <see cref="IReadRepository{T}"/>.
    /// 4. Deletes the entity from the database using <see cref="IWriteRepository{T}"/>.
    /// 5. Returns a <see cref="DeleteCoffeeBeanCommandResponse"/> indicating success or failure.
    /// </remarks>
    public class DeleteCoffeeBeanCommandHandler : IRequestHandler<DeleteCoffeeBeanCommand, DeleteCoffeeBeanCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCoffeeBeanCommand> _logger;
        private readonly IWriteRepository<CoffeeBean> _writeRepository;
        private readonly IReadRepository<CoffeeBean> _readRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCoffeeBeanCommandHandler"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper instance for object mapping.</param>
        /// <param name="writeRepository">Repository for write operations.</param>
        /// <param name="readRepository">Repository for read operations.</param>
        /// <param name="logger">Logger for capturing runtime events and errors.</param>
        public DeleteCoffeeBeanCommandHandler(
            IMapper mapper,
            IWriteRepository<CoffeeBean> writeRepository,
            IReadRepository<CoffeeBean> readRepository,
            ILogger<DeleteCoffeeBeanCommand> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        /// <summary>
        /// Handles the <see cref="DeleteCoffeeBeanCommand"/> request.
        /// </summary>
        /// <param name="request">The delete command containing the ID of the coffee bean to delete.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A <see cref="DeleteCoffeeBeanCommandResponse"/> indicating success or failure.</returns>
        /// <exception cref="AppValidationException">Thrown when validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the coffee bean entity is not found.</exception>
        public async Task<DeleteCoffeeBeanCommandResponse> Handle(DeleteCoffeeBeanCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteCoffeeBeanCommandResponse();
            var validator = new DeleteCoffeeBeanCommandValidator();

            // Validate the incoming request
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                _logger.LogError("Validation failed for DeleteCoffeeBeanCommand");

                response.Success = false;
                response.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                    response.ValidationErrors.Add(error.ErrorMessage);
                }

                throw new AppValidationException(response);
            }
            else
            {
                try
                {
                    _logger.LogInformation("Handling DeleteCoffeeBeanCommand for {Id}", request.Id);

                    // Retrieve the existing CoffeeBean from the database
                    var coffeeBean = await _readRepository.GetByIdAsync(request.Id);
                    if (coffeeBean == null)
                    {
                        throw new KeyNotFoundException("CoffeeBean not found");
                    }

                    // Delete the entity from the database
                    await _writeRepository.DeleteAsync(coffeeBean);
                    await _writeRepository.SaveChangesAsync();

                    // Prepare the response
                    response = _mapper.Map<DeleteCoffeeBeanCommandResponse>(coffeeBean);
                    response.Message = "Coffee bean deleted successfully";

                    _logger.LogInformation("Successfully deleted coffee bean with ID {Id}", coffeeBean.Id);

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while deleting the coffee bean: {Message}", ex.Message);

                    // Return a failure response
                    return new DeleteCoffeeBeanCommandResponse
                    {
                        Success = false,
                        Message = "An error occurred while deleting the coffee bean.",
                        ValidationErrors = new List<string> { ex.Message }
                    };
                }
            }
        }
    }
}

