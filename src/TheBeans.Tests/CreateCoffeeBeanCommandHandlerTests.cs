using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TheBeans.Tests
{
    // Unit tests for the CreateCoffeeBeanCommandHandler class.
    // These tests ensure that the handler processes commands correctly and handles errors appropriately.
    public class CreateCoffeeBeanCommandHandlerTests
    {
        // Mock objects for dependencies:
        // - IWriteRepository<CoffeeBean>: Simulates the database repository.
        // - IMapper: Simulates AutoMapper for object mapping.
        private readonly Mock<IWriteRepository<CoffeeBean>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        // The handler being tested.
        private readonly CreateCoffeeBeanCommandHandler _handler;

        // Constructor to initialize the test environment.
        public CreateCoffeeBeanCommandHandlerTests()
        {
            // Initialize mock objects.
            _mockRepository = new Mock<IWriteRepository<CoffeeBean>>();
            _mockMapper = new Mock<IMapper>();

            // Create an instance of the handler with mocked dependencies.
            _handler = new CreateCoffeeBeanCommandHandler(_mockMapper.Object, _mockRepository.Object);
        }

        // Test case: Ensure the handler processes a valid command and returns a success response.
        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResponse()
        {
            // Arrange
            // Create a valid CreateCoffeeBeanCommand with sample data.
            var command = new CreateCoffeeBeanCommand(
                Cost: "£39.26",
                Image: "https://example.com/image.jpg",
                Colour: "dark roast",
                Name: "TURNABOUT",
                Description: "A delicious coffee",
                Country: "Peru"
            );

            // Create a CoffeeBean entity and a response object for mocking.
            var coffeeBean = new CoffeeBean { Id = command.Id };
            var response = new CreateCoffeeBeanCommandResponse(command.Id);

            // Set up mock behavior:
            // - AutoMapper maps the command to a CoffeeBean entity.
            // - AutoMapper maps the CoffeeBean entity to a response.
            // - Repository adds the entity and saves changes successfully.
            _mockMapper.Setup(m => m.Map<CoffeeBean>(command)).Returns(coffeeBean);
            _mockMapper.Setup(m => m.Map<CreateCoffeeBeanCommandResponse>(coffeeBean)).Returns(response);

            _mockRepository.Setup(r => r.AddAsync(coffeeBean)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            // Call the handler's Handle method with the command.
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            // Verify that the response indicates success and contains the correct data.
            Assert.True(result.Success);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal("Coffee bean created successfully", result.Message);

            // Verify that the repository methods were called exactly once.
            _mockRepository.Verify(r => r.AddAsync(coffeeBean), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        // Test case: Ensure the handler handles repository exceptions and returns a failure response.
        [Fact]
        public async Task Handle_RepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            // Create a valid CreateCoffeeBeanCommand with sample data.
            var command = new CreateCoffeeBeanCommand(
                Cost: "£39.26",
                Image: "https://example.com/image.jpg",
                Colour: "dark roast",
                Name: "TURNABOUT",
                Description: "A delicious coffee",
                Country: "Peru"
            );

            // Create a CoffeeBean entity for mocking.
            var coffeeBean = new CoffeeBean { Id = command.Id };

            // Set up mock behavior:
            // - AutoMapper maps the command to a CoffeeBean entity.
            // - Repository throws an exception when adding the entity.
            _mockMapper.Setup(m => m.Map<CoffeeBean>(command)).Returns(coffeeBean);
            _mockRepository.Setup(r => r.AddAsync(coffeeBean)).ThrowsAsync(new Exception("Database error"));

            // Act
            // Call the handler's Handle method with the command.
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            // Verify that the response indicates failure and contains the correct error message.
            Assert.False(result.Success);
            Assert.Equal("An error occurred while creating the coffee bean.", result.Message);
            Assert.Contains("Database error", result.ValidationErrors);
        }
    }
}