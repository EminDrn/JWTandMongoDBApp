namespace JWTApp.Models.Entities;

public class JwtAyarlari
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}