
using API.Models;

namespace API.Repositories.Interface
{
    public interface ITransactionRepository
    {
        public Task<ResultDTO> AddData(Transaction transaction);
        public Task<TotalCredDebt> ShowData(MonthTransaction monthTransaction);
        public Task<DailyTrend> ShowDailyTrend(MonthTransaction monthTransaction);
        public Task<TotalCredDebt> ShowDayToDay(MonthTransaction monthTransaction);
        public Task<Dictionary<string,string>> GetLatestXpCode();
    }
}