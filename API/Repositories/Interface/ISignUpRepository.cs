using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Repositories.Interface
{
    public interface ISignUpRepository
    {
         public Task<ResultDTO> CreateUser(Users user);
    }
}