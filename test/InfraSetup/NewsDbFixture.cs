using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using NewsService.Models;
using System;
using System.Collections.Generic;

namespace Test.InfraSetup
{
    public class NewsDbFixture : IDisposable
    {
        private IConfigurationRoot configuration;
        public NewsContext context;
        public NewsDbFixture()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            configuration = builder.Build();
            context = new NewsContext(configuration);
            context.News.DeleteMany(Builders<UserNews>.Filter.Empty);
            context.News.InsertMany(new List<UserNews>
            {
                new UserNews{ UserId="Sam", NewsList=new List<News>{
                    new News{ NewsId=101, Title = "IT industry in 2020", Content = "It is expected to have positive growth in 2020.", PublishedAt = DateTime.Now, UrlToImage = null,Url=null }
                }}
            });
        }
        public void Dispose()
        {
            context.News.DeleteMany(Builders<UserNews>.Filter.Empty);
            context = null;
        }
    }
}
