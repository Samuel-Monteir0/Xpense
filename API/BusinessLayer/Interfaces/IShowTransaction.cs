using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface IShowTransaction
    {
        public Task<TotalCredDebt> ShowData(MonthTransaction monthTransaction);
        public Task<Dictionary<string,string>> GetLatestXpCode();
    }
}