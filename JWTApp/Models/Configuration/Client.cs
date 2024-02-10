namespace JWTApp.Models.Configuration;

public class Client
{
    public string Id { get; set; }
    public string Secret { get; set; }
  

    //www.myapi1.com www.myapi2.com
    public List<String> Audiences { get; set; }
}