using JWTApp.Models.DTOs;
using JWTApp.SharedLibrary.DTOs;

namespace JWTApp.Services;

public interface ICommentService
{
    Task<Response<CommentDto>> CreateComment(CommentDto commentDto);
    Task<Response<CommentDto>> UpdateComment(CommentDto commentDto,string id);
    Task<Response<IEnumerable<CommentDto>>> GetCommentsByMovieId(string movieId);
    Task<Response<NoDataDto>> RemoveComment(string commentId);
    Task<Response<IEnumerable<CommentDto>>> GetCommentsByUserId(string userId);
}