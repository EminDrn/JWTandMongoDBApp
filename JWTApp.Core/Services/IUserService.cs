using JWTApp.Core.DTOs;
using JWTApp.Core.Models;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserDto>> CreateUserAsync(CreateUserDTo createUserDTo);
        Task<Response<UserDto>> GetUserByNameAsync(string userName);
        Task<Response<UserDto>> GetByIdAsync(string id);
        Task<Response<IEnumerable<UserDto>>> GetAllAsync();
        Task<Response<IEnumerable<UserDto>>> Where(Expression<Func<User, bool>> predicate);
        Task<Response<UserDto>> AddAsync(UserDto entity);
        Task<Response<NoDataDto>> Remove(string id);
        Task<Response<NoDataDto>> Update(UserDto entity, string id);
    }
}
