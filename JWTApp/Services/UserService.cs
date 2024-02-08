using AutoMapper.Internal.Mappers;
using JWTApp.Mapper;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.MongoDB;
using JWTApp.SharedLibrary.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace JWTApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<MongoDbSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UserCollection);
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User { Email = createUserDto.Email, UserName = createUserDto.UserName , Password = createUserDto.Password, CreatedTime = DateTime.Now};

            await _userCollection.InsertOneAsync(user);

            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
        }

        public async Task<Response<UserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userCollection.Find(x => x.UserName == userName).SingleOrDefaultAsync();
            if (user == null)
            {
                return Response<UserDto>.Fail("User Name not found.", 404, true);
            }

            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
        }

        public async Task<Response<UserDto>> GetByIdAsync(string id)
        {
            var user = await _userCollection.Find(x => x.Id == id).SingleOrDefaultAsync();

            if (user == null)
            {
                return Response<UserDto>.Fail("User not found.", 404, true);
            }

            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
        }

        public async Task<Response<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();

            return Response<IEnumerable<UserDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<UserDto>>(users), 200);
        }


      


        public async Task<Response<UserDto>> AddAsync(UserDto entity)
        {
            var user = ObjectMapper.Mapper.Map<User>(entity);
            await _userCollection.InsertOneAsync(user);

            return Response<UserDto>.Success(ObjectMapper.Mapper.Map<UserDto>(user), 200);
        }

        public async Task<Response<NoDataDto>> Remove(string id)
        {
            var deleteResult = await _userCollection.DeleteOneAsync(x => x.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                return Response<NoDataDto>.Fail("User not found.", 404, true);
            }

            return Response<NoDataDto>.Success(200);
        }

        public async Task<Response<NoDataDto>> Update(UserDto entity, string id)
        {
            var user = ObjectMapper.Mapper.Map<User>(entity);
            user.Id = id;

            var replaceOneResult = await _userCollection.ReplaceOneAsync(x => x.Id == id, user);

            if (replaceOneResult.ModifiedCount == 0)
            {
                return Response<NoDataDto>.Fail("User not found.", 404, true);
            }

            return Response<NoDataDto>.Success(200);
        }

        public async Task<Response<IEnumerable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            var entities = await _userCollection.Find(predicate).ToListAsync();
            var dtos = ObjectMapper.Mapper.Map<IEnumerable<UserDto>>(entities);
            return Response<IEnumerable<UserDto>>.Success(dtos, 200);
        }

    }
}
