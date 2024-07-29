

using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;

namespace API.BusinessLayer.Services
{
    public class Login : ILogin
    {
        private readonly ILoginRepository _loginRepository;

        public Login(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        
        public async Task<CurrentUser> LoginUser(Users user)
        {
            return await _loginRepository.LoginUser(user);
        }
        
    }
}