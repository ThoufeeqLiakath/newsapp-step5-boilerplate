using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Configuration;

namespace UserService.Models
{
    public class UserContext
    {
        //declare variables to connect to MongoDB database
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _db;
        
        public UserContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            var server = configuration.GetSection("MongoDB:ConnectionString").Value;
            var db = configuration.GetSection("MongoDB:UserDatabase").Value;
            _mongoClient = new MongoClient(server);
            _db=_mongoClient.GetDatabase(db);           
            
        }
        //Define a MongoCollection to represent the Users collection of MongoDB based on UserProfile type
        public IMongoCollection<UserProfile> Users => _db.GetCollection<UserProfile>("Users");
    }
}
