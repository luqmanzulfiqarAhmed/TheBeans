using AutoMapper;
using MediatR;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;


namespace TheBeans.Application.Features.CoffeeBeans.Queries.CoffeeBeansSearch
{

    public class SearchCoffeeBeansQueryHandler : IRequestHandler<SearchCoffeeBeansQuery, List<SearchCoffeeBeanDto>>
    {
        private readonly IReadRepository<CoffeeBean> _readRepository;
        private readonly IMapper _mapper;
        private readonly IDailyBeanService _coffeeBeanService;


        public SearchCoffeeBeansQueryHandler(
            IReadRepository<CoffeeBean> readRepository, IDailyBeanService coffeeBeanService,
            IMapper mapper)
        {
            _readRepository = readRepository;
            _mapper = mapper;
            _coffeeBeanService = coffeeBeanService;
        }

        public async Task<List<SearchCoffeeBeanDto>> Handle(SearchCoffeeBeansQuery request, CancellationToken cancellationToken)
        {
            // Get all coffee beans from the database
            var coffeeBeans = await _readRepository.GetAllAsync();

            // Map the coffee beans to DTOs
            var coffeeBeanDtos = _mapper.Map<List<SearchCoffeeBeanDto>>(coffeeBeans);

            coffeeBeanDtos.Where(x => x.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));

            // Get the current "Bean of the Day"
            var beanOfTheDay = await _coffeeBeanService.GetCurrentBeanOfTheDayAsync();

            // Flag coffee beans that match the "Bean of the Day"
            foreach (var dto in coffeeBeanDtos)
            {
                dto.IsBOTD = beanOfTheDay != null && new Guid(dto.Id) == beanOfTheDay.Id;
            }

            // Return the list of DTOs
            return coffeeBeanDtos;
        }
    }
}