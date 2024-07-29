

using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;

namespace API.BusinessLayer.Services
{
    public class DailyTrendService : IDailyTrendService
    {
        private readonly ITransactionRepository _transactionRepository;

        public DailyTrendService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        
        public async Task<DailyTrend> ShowDailyTrend(MonthTransaction monthTransaction)
        {
            return await _transactionRepository.ShowDailyTrend(monthTransaction);
        }
    }
}