using NewsService.Models;
using NewsService.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InfraSetup;
using Xunit;

namespace Test.RepositoryTests
{
    [TestCaseOrderer("Test.PriorityOrderer", "test")]
    public class NewsRepositoryTest:IClassFixture<NewsDbFixture>
    {
        private readonly NewsRepository repository;
        public NewsRepositoryTest(NewsDbFixture fixture)
        {
            repository = new NewsRepository(fixture.context);
        }

        [Fact, TestPriority(1)]
        public async Task AddNewsShouldSuccess()
        {
            string userId = "Sam";
            News news = new News { Title = "chandrayaan2-spacecraft", Content = "The Lander of Chandrayaan-2 was named Vikram after Dr Vikram A Sarabhai", PublishedAt = DateTime.Now};

            var actual =await repository.CreateNews(userId,news);
            Assert.IsAssignableFrom<int>(actual);
            Assert.Equal(102, actual);
        }

        [Fact, TestPriority(2)]
        public async Task AddNewsForNewsUserShouldSuccess()
        {
            string userId = "Kevin";
            News news = new News { Title = "chandrayaan2-spacecraft", Content = "The Lander of Chandrayaan-2 was named Vikram after Dr Vikram A Sarabhai", PublishedAt = DateTime.Now };

            var actual = await repository.CreateNews(userId, news);
            Assert.IsAssignableFrom<int>(actual);
            Assert.Equal(101, actual);
        }
        [Fact, TestPriority(3)]
        public async Task GetAllNewsShouldReturnList()
        {
            string userId = "Sam";
            var actual = await repository.FindAllNewsByUserId(userId);
            Assert.IsAssignableFrom<List<News>>(actual);
            Assert.Equal(2,actual.Count);
        }

        [Fact, TestPriority(4)]
        public async Task IsNewsExistShouldSuccess()
        {
            string userId = "Sam";
            string title = "chandrayaan2-spacecraft";

            var actual = await repository.IsNewsExist(userId,title);
            Assert.True(actual);
        }

        [Fact, TestPriority(5)]
        public async Task GetNewsByIdShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 102;
            string title = "chandrayaan2-spacecraft";

            var actual = await repository.GetNewsById(userId, newsId);
            Assert.IsAssignableFrom<News>(actual);
            Assert.NotNull(actual);
            Assert.Equal(title, actual.Title);
        }

        [Fact, TestPriority(6)]
        public async Task RemoveNewsShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 102;
            var deleted = await repository.DeleteNews(userId,newsId);
            Assert.True(deleted);
        }

        [Fact, TestPriority(7)]
        public async Task AddOrUpdateReminderShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 101;
            Reminder reminder = new Reminder { Schedule = DateTime.Now.AddDays(3) };

            var actual = await repository.AddOrUpdateReminder(userId, newsId, reminder);
            Assert.True(actual);
        }
        [Fact, TestPriority(8)]
        public async Task DeleteReminderShouldSuccess()
        {
            string userId = "Sam";
            int newsId = 101;

            var actual = await repository.DeleteReminder(userId, newsId);
            Assert.True(actual);
        }

        [Fact, TestPriority(9)]
        public async Task RemoveNewsShouldFail()
        {
            string userId = "Sam";
            int newsId = 102;
            var deleted = await repository.DeleteNews(userId, newsId);
            Assert.False(deleted);
        }
       
        [Fact, TestPriority(10)]
        public async Task IsNewsExistShouldFail()
        {
            string userId = "Sam";
            string title = "Demo Title";

            var actual = await repository.IsNewsExist(userId,title);
            Assert.False(actual);
        }
        
    }
}
