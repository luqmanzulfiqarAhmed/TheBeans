using Moq;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Application.Common.Exceptions;

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
        // Arrange: Create a valid CreateCoffeeBeanCommand
        var command = new CreateCoffeeBeanCommand(
            10.5m,   // Cost
            "USD",   // Currency
            "image.jpg",   // Image
            "Brown",   // Colour
            "Arabica",   // Name
            "High quality coffee bean",    // Description
            "Brazil"    // Country
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


        _mapperMock.Setup(m => m.Map<CoffeeBean>(It.IsAny<CreateCoffeeBeanCommand>()))
            .Returns(coffeeBean);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<CoffeeBean>()));
        _repositoryMock.Setup(r => r.SaveChangesAsync());

        // Act: Handle the command
        var result = await _handler.Handle(command, CancellationToken.None);
Console.WriteLine($"Actual Result: Success={result.Success}, Message={result.Message}");
        // Assert: Check if the response is as expected
        Assert.True(result.Success);
        Assert.Equal("Coffee bean created successfully", result.Message);
        //Assert.Equal(coffeeBean.Id, new Guid(result.Id));

            // Assert
    Assert.True(result.Success, "Expected result to be success but it was not.");
    Assert.Equal("Coffee bean created successfully", result.Message);
    _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CoffeeBean>()), Times.Once);
    _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);

    // If the above assertions fail, print the actual result to debug:
    
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange: Create an invalid CreateCoffeeBeanCommand (e.g., missing required fields)
        var command = new CreateCoffeeBeanCommand(
            0,   // Invalid cost (should be greater than 0)
            "",  // Invalid currency
            "",  // Invalid image
            "",  // Invalid colour
            "",  // Invalid name
            "",  // Invalid description
            ""   // Invalid country
        );

        // Act & Assert: The handler should throw a validation exception
        await Assert.ThrowsAsync<AppValidationException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }
}
