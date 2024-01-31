using JWTApp.Core.DTOs;
using JWTApp.Core.Models;
using JWTApp.Core.Services;
using MongoDB.Driver;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoDatabase _mongoDatabase;

        public UserService(IMongoDatabase database)
        {
            _mongoDatabase = database;
            _userCollection = database.GetCollection<User>("Users");
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDTo createUserDto)
        {
            var user = new User { Email = createUserDto.Email, UserName = createUserDto.UserName };

            // MongoDB Identity kullanılıyorsa kayıt işlemi farklı bir şekilde yapılır
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
    }

}
