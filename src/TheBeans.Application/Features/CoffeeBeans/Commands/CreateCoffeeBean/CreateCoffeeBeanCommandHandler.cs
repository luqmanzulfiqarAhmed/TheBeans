using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Common.Exceptions;
using TheBeans.Core.Interfaces.Repositories;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean
{

    /// <summary>
    /// Handles the creation of a new coffee bean by processing the CreateCoffeeBeanCommand.
    /// </summary>
    public class CreateCoffeeBeanCommandHandler : IRequestHandler<CreateCoffeeBeanCommand, CreateCoffeeBeanCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCoffeeBeanCommand> _logger;
        private readonly IWriteRepository<CoffeeBean> _repository;

        /// <summary>
        /// Constructor to initialize dependencies for handling the CreateCoffeeBeanCommand.
        /// </summary>
        /// <param name="mapper">Instance of AutoMapper for object mapping.</param>
        /// <param name="repository">Repository interface for performing write operations.</param>
        /// <param name="logger">Logger instance for logging information and errors.</param>
        
        public CreateCoffeeBeanCommandHandler(IMapper mapper, IWriteRepository<CoffeeBean> repository,
            ILogger<CreateCoffeeBeanCommand> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Handles the CreateCoffeeBeanCommand by validating the request, mapping it to an entity,
        /// and persisting it in the database.
        /// </summary>
        /// <param name="request">The CreateCoffeeBeanCommand containing the coffee bean details.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>Response object containing success status and messages.</returns>
        public async Task<CreateCoffeeBeanCommandResponse> Handle(CreateCoffeeBeanCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCoffeeBeanCommandResponse();
            var validator = new CreateCoffeeBeanCommandValidator();

            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                _logger.LogError("Valdiation failed for CreateCoffeeBeanCommand");

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
                    _logger.LogInformation("Handling CreateCoffeeBeanCommand for {Name}", request.Name);
                    var coffeeBean = _mapper.Map<CoffeeBean>(request);
                    await _repository.AddAsync(coffeeBean);
                    await _repository.SaveChangesAsync();

                    response = _mapper.Map<CreateCoffeeBeanCommandResponse>(coffeeBean);
                    response.Message = "Coffee bean created successfully";

                    _logger.LogInformation("Successfully created coffee bean with ID {Id}", coffeeBean.Id);

                    return response;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "An error occurred while creating the coffee bean: {Message}", ex.Message);
                    // Return a failure response
                    return new CreateCoffeeBeanCommandResponse
                    {
                        Success = false,
                        Message = "An error occurred while creating the coffee bean.",
                        ValidationErrors = new List<string> { ex.Message }
                    };
                }
            }
        }
    }
}

