using JWTApp.Mapper;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.MongoDB;
using JWTApp.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JWTApp.Services;

public class CommentService: ICommentService
{
    private readonly IMongoCollection<CommentMovie> _commentCollection;

    public CommentService(IOptions<MongoDbSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _commentCollection = mongoDatabase.GetCollection<CommentMovie>(databaseSettings.Value.CommentCollection);
    }
    
    
    public async Task<Response<CommentDto>> CreateComment(CommentDto commentDto)
    {
        var comment = new CommentMovie
        {
            Comment = commentDto.Comment, UserId = commentDto.UserId, CreateTime = DateTime.UtcNow,
            MovieId = commentDto.MovieId
        };
        await _commentCollection.InsertOneAsync(comment);
        return Response<CommentDto>.Success(ObjectMapper.Mapper.Map<CommentDto>(comment),200);
    }

    public  async Task<Response<CommentDto>> UpdateComment(CommentDto commentDto, string id)
    {
        var comment = ObjectMapper.Mapper.Map<CommentMovie>(commentDto);
        comment.Id = id;
        var replaceOneResult = await _commentCollection.ReplaceOneAsync(x => x.Id == id, comment);
        if (replaceOneResult.ModifiedCount ==0)
        {
            return Response<CommentDto>.Fail("Comment Not Found", 404,true);
        }

        return Response<CommentDto>.Success(200);
    }

    public async Task<Response<IEnumerable<CommentDto>>> GetCommentsByMovieId(string movieId)
    {
        var comments = await _commentCollection.Find(x => x.MovieId == movieId).ToListAsync();
        if (comments == null)
        {
            return Response<IEnumerable<CommentDto>>.Fail("Movie not found",404,true);
        }

        return Response<IEnumerable<CommentDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<CommentDto>>(comments),
            200);
    }

    public async Task<Response<NoDataDto>> RemoveComment(string commentId)
    {
        var comment = await _commentCollection.Find(x => x.Id == commentId).SingleOrDefaultAsync();
        if (comment== null)
        {
            return Response<NoDataDto>.Fail("Comment Not Found", 404, true);
            
        }

        var cmt = _commentCollection.DeleteOneAsync(x => x.Id == commentId);
        return Response<NoDataDto>.Success(200); 
    }

    public async Task<Response<IEnumerable<CommentDto>>> GetCommentsByUserId(string userId)
    {
        var comments = await _commentCollection.Find(x => x.UserId == userId).ToListAsync();
        if (comments == null)
        {
            return Response<IEnumerable<CommentDto>>.Fail("User Not Found", 404, true);
            
        }

        return Response<IEnumerable<CommentDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<CommentDto>>(comments),
            200);
    }
}