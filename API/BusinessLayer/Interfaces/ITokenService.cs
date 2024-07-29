using API.Models;

namespace API.BusinessLayer.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Users user);
    }
}