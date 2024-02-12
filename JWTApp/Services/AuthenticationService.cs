using JWTApp.Models.Configuration;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.MongoDB;
using JWTApp.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JWTApp.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IUserRefreshTokenService _userRefreshTokenService;
    private readonly IMongoCollection<UserRefreshToken> _userCollection;
    
    public AuthenticationService( IOptions<MongoDbSettings> databaseSettings,IOptions<List<Client>> optionsClients, ITokenService tokenService, IUserRefreshTokenService userRefreshTokenService , IUserService userService)
    {
        _clients = optionsClients.Value;
        _tokenService = tokenService;
        _userRefreshTokenService = userRefreshTokenService;
        _userService = userService;
        
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _userCollection = mongoDatabase.GetCollection<UserRefreshToken>(databaseSettings.Value.UserRefreshTokenCollection);
        
    }
    
    
    
    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentException(nameof(loginDto));
        var user = await _userService.FindByEmail(loginDto.Email);

        if (user == null) return Response<TokenDto>.Fail("Email or password is wrong",400,true);

        if (!await _userService.CheckPasswordAsync(user.Id,loginDto.Password) )
        {
            return Response<TokenDto>.Fail("Email or password is wrong", 400, true);
            
        }

        var token = _tokenService.CreateToken(user);
        var userRefreshToken = await  _userCollection.Find(x => x.UserId == user.Id).SingleOrDefaultAsync();
        if (userRefreshToken == null)
        {
            await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
            await _userCollection.ReplaceOneAsync(x => x.UserId == user.Id, userRefreshToken);
        }

        return Response<TokenDto>.Success(token, 200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefrehToken =  _userCollection.Find(x => x.Code == refreshToken).FirstOrDefault();
        if (existRefrehToken == null)
        {
            return Response<TokenDto>.Fail("Refresh Token Not Found", 404, true);
        }
        
        
        var user = await  _userService.GetByUserIdAsync(existRefrehToken.UserId);
        if (user==null)
        {
            return Response<TokenDto>.Fail("UserId not found", 404, true);
        }

        var tokenDto = _tokenService.CreateToken(user);
        existRefrehToken.Code = tokenDto.RefreshToken;
        existRefrehToken.Expiration = tokenDto.RefreshTokenExpiration;
        await _userCollection.ReplaceOneAsync(x => x.UserId == user.Id, existRefrehToken);

        return Response<TokenDto>.Success(tokenDto , 200);
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken =    _userCollection.Find(x => x.Code == refreshToken).FirstOrDefault();
        if (existRefreshToken ==null)
        {
            return Response<NoDataDto>.Fail("Refresh Token Not Found",404,true);
        }

        _userRefreshTokenService.Remove(existRefreshToken);
        return Response<NoDataDto>.Success(204);
    }

    public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients.Find(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
        if (client == null)
        {
            return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404 , true);
            
        }

        var token = _tokenService.CreateTokenByClient(client);
        return Response<ClientTokenDto>.Success(token, 200);
    }
}