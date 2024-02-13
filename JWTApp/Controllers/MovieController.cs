using JWTApp.Models.DTOs;
using JWTApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTApp.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MovieController : BaseController
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(MovieDto movieDto)
    {
        return ActionResultInstance(await _movieService.Create(movieDto));
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return ActionResultInstance(await _movieService.GetAllAsync());
        
    }
    [HttpGet]
    public async Task<IActionResult> GetMovieByName(string name)
    {
        return ActionResultInstance(await _movieService.GetMovieByNameAsync(name));
    }
    [HttpGet]
    public async Task<IActionResult> GetByMovieId(string id)
    {
        return ActionResultInstance(await _movieService.GetByIdAsync(id));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveMovie(string id)
    {
        return ActionResultInstance(await _movieService.Remove(id));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMovie(MovieDto movieDto , string id)
    {
        return ActionResultInstance(await _movieService.Update(movieDto , id));
    }
    
    [HttpPost]
    public async Task<IActionResult> MovieRate( string id , double rate)
    {
        return ActionResultInstance(await _movieService.RateMove(id , rate));
    }
}