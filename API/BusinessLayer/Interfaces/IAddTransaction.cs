
using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface IAddTransaction
    {
        public Task<ResultDTO> AddData(Transaction transaction); 
    }
}