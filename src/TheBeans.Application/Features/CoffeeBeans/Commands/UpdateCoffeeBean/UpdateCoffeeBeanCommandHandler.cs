using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Common.Exceptions;
using TheBeans.Core.Interfaces.Repositories;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean
{

    /// <summary>
    /// Handles the <see cref="UpdateCoffeeBeanCommand"/> to update an existing coffee bean entity.
    /// Implements MediatR's <see cref="IRequestHandler{TRequest, TResponse}"/> for CQRS pattern.
    /// </summary>
    /// <remarks>
    /// This handler performs the following steps:
    /// 1. Validates the incoming request using <see cref="UpdateCoffeeBeanCommandValidator"/>.
    /// 2. Logs validation errors if the request is invalid and throws an <see cref="AppValidationException"/>.
    /// 3. Retrieves the existing coffee bean entity from the database using <see cref="IReadRepository{T}"/>.
    /// 4. Maps the updated data from the request to the entity using AutoMapper.
    /// 5. Updates the entity in the database using <see cref="IWriteRepository{T}"/>.
    /// 6. Returns a <see cref="UpdateCoffeeBeanCommandResponse"/> with the updated entity details.
    /// </remarks>
    public class UpdateCoffeeBeanCommandHandler : IRequestHandler<UpdateCoffeeBeanCommand, UpdateCoffeeBeanCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCoffeeBeanCommand> _logger;
        private readonly IWriteRepository<CoffeeBean> _writeRepository;
        private readonly IReadRepository<CoffeeBean> _readRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCoffeeBeanCommandHandler"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper instance for object mapping.</param>
        /// <param name="writeRepository">Repository for write operations.</param>
        /// <param name="readRepository">Repository for read operations.</param>
        /// <param name="logger">Logger for capturing runtime events and errors.</param>
        public UpdateCoffeeBeanCommandHandler(
            IMapper mapper,
            IWriteRepository<CoffeeBean> writeRepository,
            IReadRepository<CoffeeBean> readRepository,
            ILogger<UpdateCoffeeBeanCommand> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        /// <summary>
        /// Handles the <see cref="UpdateCoffeeBeanCommand"/> request.
        /// </summary>
        /// <param name="request">The update command containing the new coffee bean data.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A <see cref="UpdateCoffeeBeanCommandResponse"/> indicating success or failure.</returns>
        /// <exception cref="AppValidationException">Thrown when validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the coffee bean entity is not found.</exception>
        public async Task<UpdateCoffeeBeanCommandResponse> Handle(UpdateCoffeeBeanCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateCoffeeBeanCommandResponse();
            var validator = new UpdateCoffeeBeanCommandValidator();

            // Validate the incoming request
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                _logger.LogError("Validation failed for UpdateCoffeeBeanCommand");

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
                    _logger.LogInformation("Handling UpdateCoffeeBeanCommand for {Name}", request.Name);

                    // Retrieve the existing CoffeeBean from the database
                    var coffeeBean = await _readRepository.GetByIdAsync(request.Id);
                    if (coffeeBean == null)
                    {
                        throw new KeyNotFoundException("CoffeeBean not found");
                    }

                    // Map the updated data to the entity
                    coffeeBean = _mapper.Map<CoffeeBean>(request);

                    // Update the entity in the database
                    //await _writeRepository.UpdateAsync(coffeeBean);
                    await _writeRepository.SaveChangesAsync();

                    // Prepare the response
                    response = _mapper.Map<UpdateCoffeeBeanCommandResponse>(coffeeBean);
                    response.Message = "Coffee bean updated successfully";

                    _logger.LogInformation("Successfully updated coffee bean with ID {Id}", coffeeBean.Id);

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the coffee bean: {Message}", ex.Message);

                    // Return a failure response
                    return new UpdateCoffeeBeanCommandResponse
                    {
                        Success = false,
                        Message = "An error occurred while updating the coffee bean.",
                        ValidationErrors = new List<string> { ex.Message }
                    };
                }
            }
        }
    }
}

