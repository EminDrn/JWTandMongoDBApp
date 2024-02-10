using System.Linq.Expressions;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.SharedLibrary.DTOs;

namespace JWTApp.Services;

public interface IUserRefreshTokenService
{
    
    Task<Response<IEnumerable<UserRefreshToken>>> GetAllAsync();
    Task<Response<IEnumerable<UserRefreshToken>>> Where(Expression<Func<UserRefreshToken, bool>> predicate);
    Task<Response<UserRefreshToken>> AddAsync(UserRefreshToken entity);
    Task<Response<NoDataDto>> Remove(UserRefreshToken userRefreshToken);
}