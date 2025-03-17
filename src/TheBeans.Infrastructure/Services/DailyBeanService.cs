using Microsoft.EntityFrameworkCore;
using TheBeans.Core.Entities;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;

namespace TheBeans.Infrastructure.Services
{
public class DailyBeanService : IDailyBeanService
{
    private readonly IReadRepository<BeanOfTheDay> _repository;

    public DailyBeanService(IReadRepository<BeanOfTheDay> repository)
    {
        _repository = repository;
    }

        public Task<IEnumerable<CoffeeBean>> GetBeanOfTheDayHistoryAsync(int daysBack = 7)
        {
            throw new NotImplementedException();
        }

        public Task<CoffeeBean?> GetCurrentBeanOfTheDayAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CoffeeBean> SelectBeanOfTheDayAsync()
    {
        var yesterday = DateTime.UtcNow.AddDays(-1).Date;
        var yesterdayBean = await _repository.GetAll()
            .Include(b => b.CoffeeBean)
            .FirstOrDefaultAsync(b => b.SelectedDate == yesterday);

        var availableBeans = await _repository.GetAll()
            .Where(b => b.CoffeeBeanId != yesterdayBean.CoffeeBeanId)
            .ToListAsync();

        var random = new Random();
        var selectedBean = availableBeans[random.Next(availableBeans.Count)];

        return selectedBean.CoffeeBean;
    }
}
}