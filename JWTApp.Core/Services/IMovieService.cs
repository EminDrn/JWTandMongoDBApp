using System.Linq.Expressions;
using JWTApp.Core.DTOs;
using SharedLibrary.Dtos;

namespace JWTApp.Core.Services;

public interface IMovieService
{
    Task<Response<MovieDto>> CreateMovieAsync(MovieDto movieDto);
    Task<Response<MovieDto>> GetMovieByNameAsync(string movieName);
    Task<Response<MovieDto>> GetByIdAsync(string id);
    Task<Response<IEnumerable<MovieDto>>> GetAllAsync();
    Task<Response<IEnumerable<MovieDto>>> Where(Expression<Func<MovieDto, bool>> predicate);
    Task<Response<MovieDto>> AddAsync(MovieDto entity);
    Task<Response<NoDataDto>> Remove(string id);
    Task<Response<NoDataDto>> Update(MovieDto entity, string id);
}