using JWTApp.Models.DTOs;
using JWTApp.SharedLibrary.DTOs;

namespace JWTApp.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);  //refresh token çalındığında lazım olabilir
    Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
}