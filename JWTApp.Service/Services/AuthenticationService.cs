using JWTApp.Core.Configuration;
using JWTApp.Core.DTOs;
using JWTApp.Core.Models;
using JWTApp.Core.Repository;
using JWTApp.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IMongoCollection<UserRefreshToken> _userRefreshTokenCollection;
        private readonly IMongoDatabase _mongoDatabase;

        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<User> userManager, IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _userRefreshTokenCollection = mongoDatabase.GetCollection<UserRefreshToken>("UserRefreshTokens");
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenCollection.Find(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenCollection.InsertOneAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                var update = Builders<UserRefreshToken>.Update
                    .Set(x => x.Code, token.RefreshToken)
                    .Set(x => x.Expiration, token.RefreshTokenExpiration);

                await _userRefreshTokenCollection.UpdateOneAsync(x => x.UserId == user.Id, update);
            }

            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenCollection.Find(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh Token Not Found", 404, true);
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            var update = Builders<UserRefreshToken>.Update
                .Set(x => x.Code, tokenDto.RefreshToken)
                .Set(x => x.Expiration, tokenDto.RefreshTokenExpiration);

            await _userRefreshTokenCollection.UpdateOneAsync(x => x.UserId == user.Id, update);

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenCollection.Find(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh Token Not Found", 404, true);
            }

            await _userRefreshTokenCollection.DeleteOneAsync(x => x.Code == refreshToken);

            return Response<NoDataDto>.Success(200);
        }
    }

}
