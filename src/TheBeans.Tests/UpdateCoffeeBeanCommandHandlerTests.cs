using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Entities;
using System.Collections.Generic;

public class UpdateCoffeeBeanCommandHandlerTests
{
    private readonly Mock<IWriteRepository<CoffeeBean>> _mockWriteRepository;
    private readonly Mock<IReadRepository<CoffeeBean>> _mockReadRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<UpdateCoffeeBeanCommand>> _mockLogger;
    private readonly UpdateCoffeeBeanCommandHandler _handler;

    public UpdateCoffeeBeanCommandHandlerTests()
    {
        _mockWriteRepository = new Mock<IWriteRepository<CoffeeBean>>();
        _mockReadRepository = new Mock<IReadRepository<CoffeeBean>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UpdateCoffeeBeanCommand>>();

        _handler = new UpdateCoffeeBeanCommandHandler(
            _mockMapper.Object,
            _mockWriteRepository.Object,
            _mockReadRepository.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Update_CoffeeBean_Successfully()
    {
        // ARRANGE: Create initial CoffeeBean

        var existingCoffeeBean = new CoffeeBean
        {
            Name = "Original Bean",
            Description = "Original Description",
            Origin = "Brazil",
            RoastLevel = "Brown",
            Price = 10.5m,
            Currency = "USD",
            ImageUrl = "old_image.jpg"
        };

        var coffeeBeanId = existingCoffeeBean.Id;

        // Simulate retrieving existing bean from DB
        _mockReadRepository
            .Setup(repo => repo.GetByIdAsync(coffeeBeanId))
            .ReturnsAsync(existingCoffeeBean);

        // Define updated values
        var updateCommand = new UpdateCoffeeBeanCommand(
            coffeeBeanId,
            12.99m,  // New Cost
            "EUR",   // New Currency
            "new_image.jpg",
            "Dark Brown",
            "Updated Bean",
            "Updated Description",
            "Colombia"
        );

        // Mock AutoMapper to update the existing entity
        _mockMapper
            .Setup(mapper => mapper.Map(It.IsAny<UpdateCoffeeBeanCommand>(), It.IsAny<CoffeeBean>()))
            .Callback<UpdateCoffeeBeanCommand, CoffeeBean>((cmd, bean) =>
            {
                bean.Price = cmd.Cost;
                bean.Currency = cmd.Currency;
                bean.ImageUrl = cmd.Image;
                bean.RoastLevel = cmd.Colour;
                bean.Name = cmd.Name;
                bean.Description = cmd.Description;
                bean.Origin = cmd.Country;
            });

        // Mock SaveChangesAsync to simulate a successful update
        _mockWriteRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // ACT: Call the handler
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // ASSERT: Check if data was updated
        Assert.True(result.Success);
        Assert.Equal("Coffee bean updated successfully", result.Message);
        Assert.Equal(updateCommand.Cost, existingCoffeeBean.Price);
        Assert.Equal(updateCommand.Currency, existingCoffeeBean.Currency);
        Assert.Equal(updateCommand.Image, existingCoffeeBean.ImageUrl);
        Assert.Equal(updateCommand.Colour, existingCoffeeBean.RoastLevel);
        Assert.Equal(updateCommand.Name, existingCoffeeBean.Name);
        Assert.Equal(updateCommand.Description, existingCoffeeBean.Description);
        Assert.Equal(updateCommand.Country, existingCoffeeBean.Origin);
    }
}
