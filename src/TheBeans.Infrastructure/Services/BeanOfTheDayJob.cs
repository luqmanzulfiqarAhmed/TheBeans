using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using TheBeans.Core.Interfaces.Services;

namespace TheBeans.Infrastructure.Services
{
    public class BeanOfTheDayJob : IJob
    {
        private readonly IDailyBeanService _dailyBeanService;
        private readonly ILogger<BeanOfTheDayJob> _logger;

        public BeanOfTheDayJob(IDailyBeanService dailyBeanService, ILogger<BeanOfTheDayJob> logger)
        {
            _dailyBeanService = dailyBeanService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executing Bean of the Day selection at {Time}", DateTime.UtcNow);

            await _dailyBeanService.SelectBeanOfTheDayAsync();

            _logger.LogInformation("Bean of the Day selection completed.");
        }
    }
}