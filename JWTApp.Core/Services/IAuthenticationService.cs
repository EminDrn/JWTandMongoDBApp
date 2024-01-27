using JWTApp.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);//eğer kullanıcı logout olursa refresh tokenı null yapmak için
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);//üyelik olmadan almak için
    }
}
