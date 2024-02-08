namespace JWTApp.MongoDB
{
    public class MongoDbSettings
    {
        public string UserCollection { get; set; }

        public string MovieCollection { get; set; }

        public string TokenCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
