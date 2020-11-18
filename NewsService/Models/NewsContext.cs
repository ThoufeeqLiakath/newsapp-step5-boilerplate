using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace NewsService.Models
{
    public class NewsContext
    {
        //declare variables to connect to MongoDB database
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _db;

        public NewsContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            var server = configuration.GetSection("MongoDB:ConnectionString").Value;
            var db = configuration.GetSection("MongoDB:NewsDatabase").Value;
            _mongoClient = new MongoClient(server);
            _db = _mongoClient.GetDatabase(db);
        }
        //Define a MongoCollection to represent the News collection of MongoDB based on UserNews type
        public IMongoCollection<UserNews> News => _db.GetCollection<UserNews>("UserNews");

    }
}
