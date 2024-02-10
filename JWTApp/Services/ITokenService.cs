using JWTApp.Models.Configuration;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;

namespace JWTApp.Services;

public interface ITokenService
{
    TokenDto CreateToken(User user);
    ClientTokenDto CreateTokenByClient(Client client);
}