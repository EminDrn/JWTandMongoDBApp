using System.Linq.Expressions;
using JWTApp.Models.DTOs;
using JWTApp.SharedLibrary.DTOs;

namespace JWTApp.Services;

public interface IMovieService
{
    Task<Response<MovieDto>>  Create(MovieDto movieDto);
    Task<Response<MovieDto>> GetMovieByNameAsync(string movieName);
    Task<Response<MovieDto>> GetByIdAsync(string id);
    Task<Response<IEnumerable<MovieDto>>> GetAllAsync();
    Task<Response<NoDataDto>> Remove(string id);
    Task<Response<NoDataDto>> Update(MovieDto entity,string id);

    Task<Response<NoDataDto>> RateMove(string id, double rate);
}