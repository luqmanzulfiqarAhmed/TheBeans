using System;
namespace TheBeans.Core.Interfaces.Services
{
public interface IDailyBeanService
{
    Task<CoffeeBean> SelectBeanOfTheDayAsync();
    Task<CoffeeBean?> GetCurrentBeanOfTheDayAsync();
    Task<IEnumerable<CoffeeBean>> GetBeanOfTheDayHistoryAsync(int daysBack = 7);
}
}

