
using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface IDailyTrendService
    {
        public Task<DailyTrend> ShowDailyTrend(MonthTransaction monthTransaction);
    }
}