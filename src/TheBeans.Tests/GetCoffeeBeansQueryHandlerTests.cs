using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans;
using TheBeans.Core.Entities;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;

namespace TheBeans.Application.Tests
{
    public class GetCoffeeBeansQueryHandlerTests
    {
        private readonly Mock<IReadRepository<CoffeeBean>> _readRepositoryMock;
        private readonly Mock<IDailyBeanService> _dailyBeanServiceMock;
        private readonly IMapper _mapper;
        private readonly GetCoffeeBeansQueryHandler _handler;

        public GetCoffeeBeansQueryHandlerTests()
        {
            _readRepositoryMock = new Mock<IReadRepository<CoffeeBean>>();
            _dailyBeanServiceMock = new Mock<IDailyBeanService>();

            // Configure AutoMapper: Map CoffeeBean to CoffeeBeanDto.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoffeeBean, CoffeeBeanDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Origin))
                    .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.RoastLevel))
                    .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => $"{src.Currency}{src.Price}"))
                    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
                    .ForMember(dest => dest.IsBOTD, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();

            _handler = new GetCoffeeBeansQueryHandler(_readRepositoryMock.Object, _mapper, _dailyBeanServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCoffeeBeanDtos_WithBOTDFlagSet()
        {
            // Arrange: Create two coffee beans.
            var bean1 = new CoffeeBean
            {
                
                Name = "Bean 1",
                Description = "Desc 1",
                Origin = "Country1",
                RoastLevel = "Medium",
                Price = 10.5m,
                Currency = "£",
                ImageUrl = "bean1.jpg"
            };
            var bean2 = new CoffeeBean
            {
            
                Name = "Bean 2",
                Description = "Desc 2",
                Origin = "Country2",
                RoastLevel = "Dark",
                Price = 12.5m,
                Currency = "£",
                ImageUrl = "bean2.jpg"
            };

            var coffeeBeans = new List<CoffeeBean> { bean1, bean2 };

            // Simulate repository returning all coffee beans.
            _readRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(coffeeBeans);

            // Simulate daily bean service returning bean1 as part of BOTD history.
            _dailyBeanServiceMock.Setup(s => s.GetBeanOfTheDayHistoryAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CoffeeBean> { bean1 });

            var query = new GetCoffeeBeansQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            var dto1 = result.FirstOrDefault(x => x.Id == bean1.Id.ToString());
            var dto2 = result.FirstOrDefault(x => x.Id == bean2.Id.ToString());
            Assert.True(dto1.IsBOTD);
            Assert.False(dto2.IsBOTD);
        }
    }
}
