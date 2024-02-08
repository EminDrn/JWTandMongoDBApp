using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JWTApp.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace JWTApp.Services;

public class TokenHandler:ITokenHandler
{

    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public Token CreateAccessToken(int minute)
    {
        Token token = new();
        //security keyin simetriğini alma işlemi
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        //şifrelenmiş kimlik oluşturma
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        
        //oluşturulacak tokenın ayarları 
        token.Expiration = DateTime.UtcNow.AddMinutes(minute);
        JwtSecurityToken securityToken = new(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials : signingCredentials
            );

        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);
        return token;
    }
}