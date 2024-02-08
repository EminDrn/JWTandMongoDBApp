using JWTApp.Models.Entities;

namespace JWTApp.Services;

public interface ITokenHandler
{
    Token CreateAccessToken(int minute);
}