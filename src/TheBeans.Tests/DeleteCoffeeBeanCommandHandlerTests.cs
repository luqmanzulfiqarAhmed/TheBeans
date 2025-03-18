using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Entities;
using System.Collections.Generic;
using TheBeans.Application.Common.Exceptions;

namespace TheBeans.Tests
{
    public class DeleteCoffeeBeanCommandHandlerTests
    {
        private readonly Mock<IWriteRepository<CoffeeBean>> _mockWriteRepo;
        private readonly Mock<IReadRepository<CoffeeBean>> _mockReadRepo;
        private readonly Mock<ILogger<DeleteCoffeeBeanCommand>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DeleteCoffeeBeanCommandHandler _handler;

        public DeleteCoffeeBeanCommandHandlerTests()
        {
            _mockWriteRepo = new Mock<IWriteRepository<CoffeeBean>>();
            _mockReadRepo = new Mock<IReadRepository<CoffeeBean>>();
            _mockLogger = new Mock<ILogger<DeleteCoffeeBeanCommand>>();
            _mockMapper = new Mock<IMapper>();

            _handler = new DeleteCoffeeBeanCommandHandler(
                _mockMapper.Object,
                _mockWriteRepo.Object,
                _mockReadRepo.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_CoffeeBean_Successfully()
        {
            // Arrange
            var coffeeBeanId = Guid.NewGuid();
            var coffeeBean = new CoffeeBean { Name = "Espresso" };

            _mockReadRepo.Setup(r => r.GetByIdAsync(coffeeBeanId)).ReturnsAsync(coffeeBean);
            _mockWriteRepo.Setup(r => r.DeleteAsync(coffeeBean)).Returns(Task.CompletedTask);
            _mockWriteRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var command = new DeleteCoffeeBeanCommand(coffeeBeanId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Coffee bean deleted successfully", result.Message);

            _mockWriteRepo.Verify(r => r.DeleteAsync(coffeeBean), Times.Once);
            _mockWriteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_CoffeeBean_Not_Found()
        {
            // Arrange
            var command = new DeleteCoffeeBeanCommand(Guid.NewGuid());
            _mockReadRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CoffeeBean)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
            Assert.Equal("CoffeeBean not found", exception.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_AppValidationException_When_ValidationFails()
        {
            // Arrange
            var command = new DeleteCoffeeBeanCommand(Guid.Empty); // Invalid ID for validation

            var response = new DeleteCoffeeBeanCommandResponse { Success = false };
            _mockMapper.Setup(m => m.Map<DeleteCoffeeBeanCommandResponse>(It.IsAny<CoffeeBean>()))
                .Returns(response);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AppValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));
            Assert.False(response.Success);
            Assert.NotEmpty(response.ValidationErrors);
        }
    }
}
