using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserRepository by inheriting IUserRepository class 
    //which is used to implement all methods in the classs
    public class UserRepository:IUserRepository
    {
        //define a private variable to represent Reminder Database Context
        private readonly UserContext _userContext;
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public Task<bool> AddUser(UserProfile user)
        {
            _userContext.Users.InsertOne(user);
            var insertedUser = GetUser(user.UserId).Result;
            return Task.FromResult(insertedUser!=null);
        }

        public Task<bool> DeleteUser(string userId)
        {
            var deleted = _userContext.Users.DeleteOne(x => x.UserId == userId);
            return Task.FromResult(deleted.DeletedCount>0);
        }

        public Task<UserProfile> GetUser(string userId)
        {
            var user = _userContext.Users.Find(x => x.UserId == userId).FirstOrDefault();
            return Task.FromResult(user);
        }

        public Task<bool> UpdateUser(UserProfile user)
        {
            var filter = Builders<UserProfile>.Filter.Where(x => x.UserId == user.UserId);
            var update = Builders<UserProfile>.Update.Set(x => x.Email, user.Email)
                                                    .Set(x => x.Contact, user.Contact)
                                                    .Set(x => x.FirstName, user.FirstName)
                                                    .Set(x => x.LastName, user.LastName)
                                                    .Set(x => x.CreatedAt, user.CreatedAt);
            return Task.FromResult(_userContext.Users.UpdateOne(filter,update).ModifiedCount > 0);
        }        
    }
}
