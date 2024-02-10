using System.Linq.Expressions;
using JWTApp.Mapper;
using JWTApp.Models.Entities;
using JWTApp.MongoDB;
using JWTApp.SharedLibrary.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JWTApp.Services;

public class UserRefreshTokenService:IUserRefreshTokenService
{
    private readonly IMongoCollection<UserRefreshToken> _userCollection;
    public UserRefreshTokenService(IOptions<MongoDbSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _userCollection = mongoDatabase.GetCollection<UserRefreshToken>(databaseSettings.Value.UserRefreshTokenCollection);
    }
    
    
    public async Task<Response<IEnumerable<UserRefreshToken>>> GetAllAsync()
    {
        var tokens = await _userCollection.Find(_ => true).ToListAsync();

        return Response<IEnumerable<UserRefreshToken>>.Success(ObjectMapper.Mapper.Map<IEnumerable<UserRefreshToken>>(tokens), 200);
    }

    public async Task<Response<IEnumerable<UserRefreshToken>>> Where(Expression<Func<UserRefreshToken, bool>> predicate)
    {
        var entities = await _userCollection.Find(predicate).ToListAsync();
        var dtos = ObjectMapper.Mapper.Map<IEnumerable<UserRefreshToken>>(entities);
        return Response<IEnumerable<UserRefreshToken>>.Success(dtos, 200);
    }

    public async Task<Response<UserRefreshToken>> AddAsync(UserRefreshToken entity)
    {
        var token = ObjectMapper.Mapper.Map<UserRefreshToken>(entity);
        await _userCollection.InsertOneAsync(token);
        return Response<UserRefreshToken>.Success(ObjectMapper.Mapper.Map<UserRefreshToken>(entity), 200);

    }

    public async Task<Response<NoDataDto>> Remove(UserRefreshToken userRefreshToken)
    {
        
        
        var deleteResult = await _userCollection.DeleteOneAsync(x => x.Code == userRefreshToken.Code);

        if (deleteResult.DeletedCount == 0)
        {
            return Response<NoDataDto>.Fail("User not found.", 404, true);
        }

        return Response<NoDataDto>.Success(200);
    }

    
}