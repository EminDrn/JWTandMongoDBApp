using AutoMapper.Internal.Mappers;
using JWTApp.Mapper;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.MongoDB;
using JWTApp.SharedLibrary.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JWTApp.Services;

public class MovieService:IMovieService
{
    private readonly IMongoCollection<Movie> _movieCollection;

    public MovieService(IOptions<MongoDbSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _movieCollection = mongoDatabase.GetCollection<Movie>(databaseSettings.Value.MovieCollection);
    }
    
    
    
    public async Task<Response<MovieDto>> Create(MovieDto movieDto)
    {
        var movie = new Movie
        {
            MovieName = movieDto.MovieName, MovieDescription = movieDto.MovieDescription,
            MoviePhoto = movieDto.MoviePhoto, Rating = 0, ReleaseDate = DateTime.UtcNow, RateCounter = 0
        };
        
        await _movieCollection.InsertOneAsync(movie);
        return Response<MovieDto>.Success(ObjectMapper.Mapper.Map<MovieDto>(movie), 200);

    }

    public async Task<Response<MovieDto>> GetMovieByNameAsync(string movieName)
    {
        var movie = await _movieCollection.Find(x => x.MovieName == movieName).SingleOrDefaultAsync();
        if (movie == null)
        {
            return Response<MovieDto>.Fail("Movie Name not found.", 404, true);

        }

        return Response<MovieDto>.Success(ObjectMapper.Mapper.Map<MovieDto>(movie), 200);
    }

    public async Task<Response<MovieDto>> GetByIdAsync(string id)
    {
        var movie = await _movieCollection.Find(x => x.Id == id).SingleOrDefaultAsync();
        if (movie == null)
        {
            return Response<MovieDto>.Fail("Movie Not Found", 404, true);
        }

        return Response<MovieDto>.Success(ObjectMapper.Mapper.Map<MovieDto>(movie),200);
    }

    public async Task<Response<IEnumerable<MovieDto>>> GetAllAsync()
    {
        var movieList = await _movieCollection.Find(_ => true).ToListAsync();
        return Response<IEnumerable<MovieDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<MovieDto>>(movieList), 200);
    }

    public  async Task<Response<NoDataDto>> Remove(string id)
    {
        var user = await _movieCollection.Find(x => x.Id == id).SingleOrDefaultAsync();
        if (user == null)
        {
            return Response<NoDataDto>.Fail("Movie not found.", 404, true);

        }
        var movie = _movieCollection.DeleteOneAsync(x=>x.Id == id);
        return Response<NoDataDto>.Success(200);


    }

    public async Task<Response<NoDataDto>> Update(MovieDto entity, string id)
    {
        
        var movie = ObjectMapper.Mapper.Map<Movie>(entity);
        movie.Id = id;
        var replaceOneResult = await _movieCollection.ReplaceOneAsync(x => x.Id == id, movie);
        if (replaceOneResult.ModifiedCount ==0)
        {
            return Response<NoDataDto>.Fail("Movie not found.", 404, true);

        }
        return Response<NoDataDto>.Success(200);

    }

    public async Task<Response<NoDataDto>> RateMove(string id, double rate)
    {
        var movie = await _movieCollection.Find(x => x.Id == id).SingleOrDefaultAsync();
        var totalRating = movie.RateCounter * movie.Rating;
        totalRating = totalRating + rate;
        movie.RateCounter = movie.RateCounter + 1;



        movie.Rating = totalRating / movie.RateCounter;

        
        
        await _movieCollection.ReplaceOneAsync(x => x.Id == id, movie);
        return Response<NoDataDto>.Success(200);
    }
}