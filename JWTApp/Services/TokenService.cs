using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JWTApp.Mapper;
using JWTApp.Models.Configuration;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;
using JWTApp.SharedLibrary.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTApp.Services;

public class TokenService:ITokenService
{
    private readonly CustomTokenOptions _tokenOptions;

    public TokenService( IOptions<CustomTokenOptions> options)
    {
       
        _tokenOptions = options.Value;
    }
    
    
    public TokenDto CreateToken(User user)
    {
        var acccessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);

        var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);
            
        SigningCredentials signingCredentials = new SigningCredentials(securityKey , SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _tokenOptions.Issuer,
            expires: acccessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(user, _tokenOptions.Audience),
            signingCredentials: signingCredentials);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var tokenDto = new TokenDto
        {
            AccessToken = token,
            RefreshToken = CreateRefreshToken(),
            AccessTokenExpiration = acccessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };
        return tokenDto;
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        var acccessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: _tokenOptions.Issuer,
            expires: acccessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaimsByClient(client),
            signingCredentials: signingCredentials);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var tokenDto = new ClientTokenDto
        {
            AccessToken = token,
            AccessTokenExpiration = acccessTokenExpiration
        };
        return tokenDto;
    }
    
    
    
    private string CreateRefreshToken()
    {
        var numberByte = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
    
    private IEnumerable<Claim> GetClaims(User userApp , List<String> audiences)
    {

        var userRoles = userApp.Role;
        
        var userlist = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier , userApp.Id),
            new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
            new Claim(ClaimTypes.Name , userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role , userApp.Role)
            

        };
        userlist.AddRange(audiences.Select(x=>new Claim(JwtRegisteredClaimNames.Aud,x)));   
       
        return userlist;

    }
    
    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
        new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
        return claims;


    }
}