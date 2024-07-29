using API.Models;

namespace API.Repositories.Interface
{
    public interface ILoginRepository
    {
        public Task<CurrentUser> LoginUser(Users user);
    }
}