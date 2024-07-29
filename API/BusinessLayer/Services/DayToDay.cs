using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;

namespace API.BusinessLayer.Services
{
    public class DayToDay : IDayToDay
    {
        private readonly ITransactionRepository _transactionRepository;

        public DayToDay(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<TotalCredDebt> ShowDayToDay(MonthTransaction monthTransaction)
        {
            return await _transactionRepository.ShowDayToDay(monthTransaction);
        }
    }
}