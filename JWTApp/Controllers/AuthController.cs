using JWTApp.Models.DTOs;
using JWTApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTApp.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : BaseController
{
    // GET
    private readonly IAuthenticationService _authenticationService;
    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);
        return ActionResultInstance(result);
    }
}