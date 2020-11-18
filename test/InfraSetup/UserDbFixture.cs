using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using UserService.Models;

namespace Test.InfraSetup
{
    public class UserDbFixture:IDisposable
    {
        private IConfigurationRoot configuration;
        public UserContext context;
        public UserDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            configuration = builder.Build();
            context = new UserContext(configuration);
            context.Users.DeleteMany(Builders<UserProfile>.Filter.Empty);
            //context.Users.InsertMany(new List<UserProfile>
            //{
            //   new UserProfile { UserId = "Jack", FirstName = "Jackson",LastName="James", Contact = "9812345670", Email="jack@ymail.com", CreatedAt=DateTime.Now }
            //});
        }
        public void Dispose()
        {
            context.Users.DeleteMany(Builders<UserProfile>.Filter.Empty);
            context = null;
        }
    }
}
