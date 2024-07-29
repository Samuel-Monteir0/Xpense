using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;

namespace API.BusinessLayer.Services
{
    public class ShowTransaction : IShowTransaction
    {
        private readonly ITransactionRepository _transactionRepository;

        public ShowTransaction(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        
        public async Task<TotalCredDebt> ShowData(MonthTransaction monthTransaction)
        {
            return await _transactionRepository.ShowData(monthTransaction);
        }
        public async Task<Dictionary<string,string>> GetLatestXpCode()
        {
            return await _transactionRepository.GetLatestXpCode();
        }
    }
}