using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheBeans.Core.Entities;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;

namespace TheBeans.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for managing daily bean selections and retrieving bean history.
    /// </summary>
    public class DailyBeanService : IDailyBeanService
    {
        private readonly IReadRepository<BeanOfTheDay> _repository;
        private readonly IWriteRepository<BeanOfTheDay> _writeRepository;
        private readonly IReadRepository<CoffeeBean> _coffeeBeanRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyBeanService"/> class.
        /// </summary>
        /// <param name="repository">Repository for accessing daily bean selection records.</param>
        /// <param name="coffeeBeanRepository">Repository for accessing available coffee beans.</param>
        public DailyBeanService(IReadRepository<BeanOfTheDay> repository, 
        IReadRepository<CoffeeBean> coffeeBeanRepository,IWriteRepository<BeanOfTheDay> writeRepository)
        {
            _repository = repository;
            _writeRepository = writeRepository;
            _coffeeBeanRepository = coffeeBeanRepository;
        }

        /// <summary>
        /// Retrieves the history of selected beans for the past specified number of days.
        /// </summary>
        /// <param name="daysBack">Number of days to look back for bean selections. Default is 7 days.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of <see cref="CoffeeBean"/> objects.</returns>
        public async Task<IEnumerable<CoffeeBean>> GetBeanOfTheDayHistoryAsync(int daysBack = 7)
        {
            var startDate = DateTime.UtcNow.AddDays(-daysBack).Date;

            var beansOfTheDay = await _repository.GetAll()
                .Where(b => b.SelectedDate >= startDate)
                .Include(b => b.CoffeeBean)
                .OrderByDescending(b => b.SelectedDate)
                .ToListAsync();

            return beansOfTheDay.Select(b => b.CoffeeBean);
        }

        /// <summary>
        /// Retrieves the currently selected bean of the day.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the current <see cref="CoffeeBean"/> object, or null if not selected.</returns>
        public async Task<CoffeeBean?> GetCurrentBeanOfTheDayAsync()
        {
            var today = DateTime.UtcNow.Date;

            var todayBean = await _repository.GetAll()
                .Include(b => b.CoffeeBean)
                .FirstOrDefaultAsync(b => b.SelectedDate == today);

            return todayBean?.CoffeeBean;
        }

        /// <summary>
        /// Selects a new bean of the day, ensuring it differs from the previous day's selection.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the selected <see cref="CoffeeBean"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no available coffee beans are found to select.</exception>
        public async Task<CoffeeBean> SelectBeanOfTheDayAsync()
        {
            var today = DateTime.UtcNow.Date;

            var todayBean = await _repository.GetAll()
                .FirstOrDefaultAsync(b => b.SelectedDate == today);

            if (todayBean != null)
            {
                return todayBean.CoffeeBean;
            }

            var yesterday = today.AddDays(-1);

            var yesterdayBean = await _repository.GetAll()
                .FirstOrDefaultAsync(b => b.SelectedDate == yesterday);

            var availableBeansQuery = _coffeeBeanRepository.GetAll();

            if (yesterdayBean != null)
            {
                availableBeansQuery = availableBeansQuery
                    .Where(b => b.Id != yesterdayBean.CoffeeBeanId);
            }

            var availableBeans = await availableBeansQuery.ToListAsync();

            if (!availableBeans.Any())
            {
                throw new InvalidOperationException("No available coffee beans to select.");
            }

            var random = new Random();
            var selectedBean = availableBeans[random.Next(availableBeans.Count)];

            var beanOfTheDay = new BeanOfTheDay
            {
                CoffeeBeanId = selectedBean.Id,
                SelectedDate = today
            };

            await _writeRepository.AddAsync(beanOfTheDay);
            await _writeRepository.SaveChangesAsync();

            return selectedBean;
        }
    }
}
