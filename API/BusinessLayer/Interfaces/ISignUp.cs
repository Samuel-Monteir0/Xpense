using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.BusinessLayer.Interfaces
{
    public interface ISignUp
    {
         public Task<ResultDTO> CreateUser(Users user);
    }
}