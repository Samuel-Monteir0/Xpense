
using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;

namespace API.BusinessLayer.Services
{
    public class AddTransaction : IAddTransaction
    {
        private readonly ITransactionRepository _transactionRepository;

        public AddTransaction(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ResultDTO> AddData(Transaction transaction)
        {
            return await _transactionRepository.AddData(transaction);
        }
    }
}