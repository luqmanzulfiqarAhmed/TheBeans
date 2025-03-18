using AutoMapper;
using MediatR;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;

namespace TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans
{
    /// <summary>
    /// Handles the <see cref="GetCoffeeBeansQuery"/> to retrieve a list of coffee beans.
    /// Implements MediatR's <see cref="IRequestHandler{TRequest, TResponse}"/> for CQRS pattern.
    /// </summary>
    /// <remarks>
    /// This handler performs the following steps:
    /// 1. Retrieves all coffee beans from the database using <see cref="IReadRepository{T}"/>.
    /// 2. Fetches the history of the "Bean of the Day" using <see cref="IDailyBeanService"/>.
    /// 3. Maps the coffee beans to <see cref="CoffeeBeanDto"/> objects using AutoMapper.
    /// 4. Flags each coffee bean as "Bean of the Day" (BOTD) if it appears in the BOTD history.
    /// 5. Returns the list of mapped DTOs.
    /// </remarks>
    public class GetCoffeeBeansQueryHandler : IRequestHandler<GetCoffeeBeansQuery, List<CoffeeBeanDto>>
    {
        private readonly IReadRepository<CoffeeBean> _readRepository;
        private readonly IDailyBeanService _coffeeBeanService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCoffeeBeansQueryHandler"/> class.
        /// </summary>
        /// <param name="readRepository">Repository for read operations.</param>
        /// <param name="mapper">AutoMapper instance for object mapping.</param>
        /// <param name="coffeeBeanService">Service for fetching "Bean of the Day" history.</param>
        public GetCoffeeBeansQueryHandler(
            IReadRepository<CoffeeBean> readRepository,
            IMapper mapper,
            IDailyBeanService coffeeBeanService)
        {
            _readRepository = readRepository;
            _mapper = mapper;
            _coffeeBeanService = coffeeBeanService;
        }

        /// <summary>
        /// Handles the <see cref="GetCoffeeBeansQuery"/> request.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>A list of <see cref="CoffeeBeanDto"/> objects representing the coffee beans.</returns>
        public async Task<List<CoffeeBeanDto>> Handle(GetCoffeeBeansQuery request, CancellationToken cancellationToken)
        {
            // Get all coffee beans from the database
            var coffeeBeans = await _readRepository.GetAllAsync();

            // Get the history of the "Bean of the Day"
            var beanOfTheDay = await _coffeeBeanService.GetBeanOfTheDayHistoryAsync();

            // Map the coffee beans to DTOs
            var coffeeBeanDtos = _mapper.Map<List<CoffeeBeanDto>>(coffeeBeans);

            // Flag coffee beans that are in the "Bean of the Day" history
            coffeeBeanDtos = coffeeBeanDtos
                .Select(dto =>
                {
                    // Set IsBOTD flag based on whether the bean exists in the Bean of the Day history
                    dto.IsBOTD = beanOfTheDay.Any(b => b.Id == new Guid(dto.Id));
                    return dto; // Return the updated DTO
                })
                .ToList();

            // Return the list of DTOs
            return coffeeBeanDtos;
        }
    }
}