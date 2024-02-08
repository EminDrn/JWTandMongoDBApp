using System.Linq.Expressions;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.SharedLibrary.DTOs;

namespace JWTApp.Services;

public interface IUserService
{
    Task<Response<UserDto>> CreateUserAsync(CreateUserDto createUserDTo);
    Task<Response<UserDto>> GetUserByNameAsync(string userName);
    Task<Response<UserDto>> GetByIdAsync(string id);
    Task<Response<IEnumerable<UserDto>>> GetAllAsync();
    Task<Response<IEnumerable<UserDto>>> Where(Expression<Func<User, bool>> predicate);
    Task<Response<UserDto>> AddAsync(UserDto entity);
    Task<Response<NoDataDto>> Remove(string id);
    Task<Response<NoDataDto>> Update(UserDto entity, string id);
}