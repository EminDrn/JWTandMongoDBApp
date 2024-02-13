using JWTApp.Models.DTOs;
using JWTApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTApp.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CommentController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentDto commentDto)
    {
        return ActionResultInstance(await _commentService.CreateComment(commentDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateComment(CommentDto commentDto, string id)
    {
        return ActionResultInstance(await _commentService.UpdateComment(commentDto, id));
    }

    [HttpGet]
    public async Task<IActionResult> GetCommentsByMovieId(string id)
    {
        return ActionResultInstance(await _commentService.GetCommentsByMovieId(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetCommentsByUserId(string id)
    {
        return ActionResultInstance( await _commentService.GetCommentsByUserId(id));
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveComment(string id)
    {
        return ActionResultInstance(await _commentService.RemoveComment(id));
    }
}