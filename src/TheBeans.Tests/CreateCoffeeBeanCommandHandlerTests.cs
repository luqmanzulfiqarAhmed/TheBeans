using Moq;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Application.Common.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheBeans.Tests
{
public class CreateCoffeeBeanCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IWriteRepository<CoffeeBean>> _repositoryMock;
    private readonly Mock<ILogger<CreateCoffeeBeanCommand>> _loggerMock;
    private readonly CreateCoffeeBeanCommandHandler _handler;

    public CreateCoffeeBeanCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IWriteRepository<CoffeeBean>>();
        _loggerMock = new Mock<ILogger<CreateCoffeeBeanCommand>>();

        // Initialize the handler with mocked dependencies
        _handler = new CreateCoffeeBeanCommandHandler(_mapperMock.Object, _repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = new CreateCoffeeBeanCommand(
            10.5m,  // Cost
            "USD",   // Currency
            "image.jpg",
            "Brown",
            "Arabica",
            "High quality coffee bean",
            "Brazil"
        );

        var coffeeBean = new CoffeeBean
        {
            Name = "Arabica",
            Price = 10.5m,
            Currency = "USD",
            ImageUrl = "coffee_image.png",
            RoastLevel = "Brown",
            Description = "Delicious coffee from Brazil",
            Origin = "Brazil"
        };

        // Ensure AutoMapper correctly maps the command to CoffeeBean
_mapperMock
    .Setup(m => m.Map<CoffeeBean>(It.IsAny<CreateCoffeeBeanCommand>()))
    .Returns((CreateCoffeeBeanCommand command) => new CoffeeBean
    {
        Name = command.Name,
        Price = command.Cost,
        Currency = command.Currency,
        ImageUrl = command.Image,
        RoastLevel = command.Colour,
        Description = command.Description,
        Origin = command.Country
    });

        // Ensure repository methods are awaited properly
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<CoffeeBean>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Debugging output
        Console.WriteLine($"Actual Result: Success={result.Success}, Message={result.Message}");

        // Assert
        Assert.True(result.Success, "Expected result to be success but it was not.");
        Assert.Equal("Coffee bean created successfully", result.Message);
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CoffeeBean>()), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateCoffeeBeanCommand(
            0, "", "", "", "", "", ""  // Invalid values
        );

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }
}

}