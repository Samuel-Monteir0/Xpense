using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface ILogin
    {
         public Task<CurrentUser> LoginUser(Users user);        
    }
}