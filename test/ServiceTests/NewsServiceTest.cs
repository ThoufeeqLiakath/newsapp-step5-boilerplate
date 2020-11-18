using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using System.Threading.Tasks;
using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;

namespace Test.ServiceTests
{
    public class NewsServiceTest
    {
        [Fact]
        public async Task AddNewsShouldReturnNews()
        {
            string userId = "Jack";
            News news = new News { Title = "chandrayaan2-spacecraft", Content = "The Lander of Chandrayaan-2 was named Vikram after Dr Vikram A Sarabhai", PublishedAt = DateTime.Now};
            int newsId = 103;
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.IsNewsExist(userId,news.Title)).Returns(Task.FromResult(false));
            mockRepo.Setup(repo => repo.CreateNews(userId,news)).Returns(Task.FromResult(newsId));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await service.CreateNews(userId,news);
            Assert.IsAssignableFrom<int>(actual);
            Assert.Equal(103, actual);
        }

        [Fact]
        public async Task FindAllNewsShouldReturnListOfNews()
        {
            string userId = "Jack";
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.FindAllNewsByUserId(userId)).Returns(Task.FromResult(this.newsList));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await service.FindAllNewsByUserId(userId);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<List<News>>(actual);
            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public async Task DeleteNewsShouldSuccess()
        {
            string userId = "Jack";
            int newsId = 101;
            News news = new News { NewsId = 101, Title = "IT industry in 2020", Content = "It is expected to have positive growth in 2020.", PublishedAt = DateTime.Now, UrlToImage = null, Url = null };
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.DeleteNews(userId,newsId)).Returns(Task.FromResult(true));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await service.DeleteNews(userId,newsId);
            Assert.True(actual);
        }

        [Fact]
        public async Task AddOrUpdateReminderShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 101;
            Reminder reminder = new Reminder { Schedule=DateTime.Now.AddDays(2) };
            News mockNews = new News { NewsId = 101, Title = "chandrayaan2-spacecraft" };

            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.GetNewsById(userId, newsId)).Returns(Task.FromResult(mockNews));
            mockRepo.Setup(repo => repo.AddOrUpdateReminder(userId, newsId,reminder)).Returns(Task.FromResult(true));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual =await service.AddOrUpdateReminder(userId, newsId, reminder);
            Assert.True(actual);
        }
        
        [Fact]
        public async Task DeleteReminderShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 101;

            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.IsReminderExists(userId, newsId)).Returns(Task.FromResult(true));
            mockRepo.Setup(repo => repo.DeleteReminder(userId, newsId)).Returns(Task.FromResult(true));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await service.DeleteReminder(userId, newsId);
            Assert.True(actual);
        }

        [Fact]
        public async Task AddNewsShouldThrowException()
        {
            string userId = "Jack";
            News news = new News { Title = "chandrayaan2-spacecraft", Content = "The Lander of Chandrayaan-2 was named Vikram after Dr Vikram A Sarabhai", PublishedAt = DateTime.Now };
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.IsNewsExist(userId,news.Title)).Returns(Task.FromResult(true));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NewsAlreadyExistsException>(() => service.CreateNews(userId,news));
            Assert.Equal($"{userId} have already added this news", actual.Message);
        }

        [Fact]
        public async Task FindAllNewsShouldThrowException()
        {
            string userId = "Jack";
            List<News> lstnews = null;
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.FindAllNewsByUserId(userId)).Returns(Task.FromResult(lstnews));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoNewsFoundException>(() => service.FindAllNewsByUserId(userId));
            Assert.Equal($"No news found for {userId}", actual.Message);
        }

        [Fact]
        public async Task DeleteNewsShouldThrowException()
        {
            string userId = "Jack";
            int newsId = 101;
            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.DeleteNews(userId,newsId)).Returns(Task.FromResult(false));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoNewsFoundException>(() => service.DeleteNews(userId,newsId));
            Assert.Equal($"NewsId {newsId} for {userId} doesn't exist", actual.Message);
        }
        [Fact]
        public async Task AddOrUpdateReminderShouldThrowException()
        {
            string userId = "Sam";
            int newsId = 101;
            Reminder reminder = new Reminder { Schedule = DateTime.Now.AddDays(2) };
            News mockNews = null;

            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.GetNewsById(userId, newsId)).Returns(Task.FromResult(mockNews));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoNewsFoundException>(()=> service.AddOrUpdateReminder(userId, newsId, reminder));
            Assert.Equal($"NewsId {newsId} for {userId} doesn't exist",actual.Message);
        }

        [Fact]
        public async Task DeleteReminderShouldThrowException()
        {
            string userId = "Sam";
            int newsId = 101;

            var mockRepo = new Mock<INewsRepository>();
            mockRepo.Setup(repo => repo.IsReminderExists(userId, newsId)).Returns(Task.FromResult(false));
            var service = new NewsService.Services.NewsService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoReminderFoundException>(() => service.DeleteReminder(userId, newsId));
            Assert.Equal("No reminder found for this news", actual.Message);
        }
        List<News> newsList = new List<News>{
            new News { NewsId = 101, Title = "IT industry in 2020", Content = "It is expected to have positive growth in 2020.", PublishedAt = DateTime.Now, UrlToImage = null, Url=null },
            new News {NewsId=102, Title = "2020 FIFA U-17 Women World Cup", Content = "The tournament will be held in India between 2 and 21 November 2020", PublishedAt = DateTime.Now, UrlToImage=null }
            };
    }
}
