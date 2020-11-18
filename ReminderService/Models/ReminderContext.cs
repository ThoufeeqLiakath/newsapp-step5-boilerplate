using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace ReminderService.Models
{
    public class ReminderContext
    {
        //declare variables to connect to MongoDB database
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _db;
        public ReminderContext(IConfiguration configuration)
        {
            //Initialize MongoClient and Database using connection string and database name from configuration
            var server = configuration.GetSection("MongoDB:ConnectionString").Value;
            var db = configuration.GetSection("MongoDB:ReminderDatabase").Value;
            _mongoClient = new MongoClient(server);
            _db = _mongoClient.GetDatabase(db);
        }
        //Define a MongoCollection to represent the Reminders collection of MongoDB based on Reminder type
        public IMongoCollection<Reminder> Reminders => _db.GetCollection<Reminder>("Reminders");
    }
}