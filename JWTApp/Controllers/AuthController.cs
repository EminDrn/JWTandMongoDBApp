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

    [HttpPost]
    public async Task<IActionResult> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result =  _authenticationService.CreateTokenByClient(clientLoginDto);
        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);
        return ActionResultInstance(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
        return ActionResultInstance(result);
    }
    
}