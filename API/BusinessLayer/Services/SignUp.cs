using API.BusinessLayer.Interfaces;
using API.Models;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.BusinessLayer.Services
{
    public class SignUp : ISignUp
    {
        private readonly ISignUpRepository _signUpRepository;

        public SignUp(ISignUpRepository signUpRepository)
        {
            _signUpRepository = signUpRepository;
        }

        public async Task<ResultDTO> CreateUser(Users user)
        {
            return await _signUpRepository.CreateUser(user);
        }
    }
}