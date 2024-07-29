
using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface IDayToDay
    {
        public Task<TotalCredDebt> ShowDayToDay(MonthTransaction monthTransaction);
    }
}