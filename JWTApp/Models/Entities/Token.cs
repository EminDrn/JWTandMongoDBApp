namespace JWTApp.Models.Entities;

public class Token
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}