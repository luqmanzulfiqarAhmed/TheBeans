using Moq;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheBeans.Core.Interfaces.Services;
using TheBeans.Infrastructure.Services;
using Quartz;

namespace TheBeans.Tests
{
    public class BeanOfTheDayJobTests
    {
        private readonly Mock<IDailyBeanService> _mockDailyBeanService;
        private readonly Mock<ILogger<BeanOfTheDayJob>> _mockLogger;
        private readonly BeanOfTheDayJob _job;

        public BeanOfTheDayJobTests()
        {
            _mockDailyBeanService = new Mock<IDailyBeanService>();
            _mockLogger = new Mock<ILogger<BeanOfTheDayJob>>();
            _job = new BeanOfTheDayJob(_mockDailyBeanService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Execute_ShouldCall_SelectRandomBeanOfTheDayAsync()
        {
            // Arrange
            var jobContext = new Mock<IJobExecutionContext>();

            // Act
            await _job.Execute(jobContext.Object);

            // Assert
            _mockDailyBeanService.Verify(service => service.SelectBeanOfTheDayAsync(), Times.Once);
        }
    }
}