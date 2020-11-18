using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using ReminderService.Exceptions;

namespace Test.ServiceTests
{
    public class ReminderServiceTest    {
        [Fact]
        public async Task AddReminderShouldReturnReminder()
        {
            string userId = "Jack";
            string email = "jack@ymail.com";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 102, Schedule = DateTime.Now.AddDays(1) };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.IsReminderExists(userId, reminder.NewsId)).Returns(Task.FromResult(false));
            mockRepo.Setup(repo => repo.CreateReminder(userId, email, reminder));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual= await service.CreateReminder(userId, email, reminder);
            Assert.True(actual);
        }

        [Fact]
        public async Task GetRemindersShouldReturnListOfReminder()
        {
            string userId = "Jack";
            List<ReminderSchedule> lstreminder = new List<ReminderSchedule> { new ReminderSchedule { NewsId = 102, Schedule = DateTime.Now.AddDays(1) } };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.GetReminders(userId)).Returns(Task.FromResult(lstreminder));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await service.GetReminders(userId);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<List<ReminderSchedule>>(actual);
            Assert.Single(actual);
        }
      

        [Fact]
        public async Task DeleteReminderShouldSuccess()
        {
            string userId = "Jack";
            int newsId = 101;
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.DeleteReminder(userId,newsId)).Returns(Task.FromResult(true));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await service.DeleteReminder(userId, newsId);
            Assert.True(actual);
        }

        [Fact]
        public async Task UpdateReminderShouldSuccess()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule {NewsId=101, Schedule=DateTime.Now.AddDays(3) };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.UpdateReminder(userId, reminder)).Returns(Task.FromResult(true));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await service.UpdateReminder(userId, reminder);
            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteReminderShouldThrowException()
        {
            string userId = "Jack";
            int newsId = 102;
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.DeleteReminder(userId, newsId)).Returns(Task.FromResult(false));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoReminderFoundException>(() => service.DeleteReminder(userId,newsId));
            Assert.Equal("No reminder found for this news", actual.Message);
        }

        [Fact]
        public async Task UpdateReminderShouldThrowException()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule { NewsId=102, Schedule=DateTime.Now.AddHours(10) };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.UpdateReminder(userId, reminder)).Returns(Task.FromResult(false));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoReminderFoundException>(() => service.UpdateReminder(userId, reminder));
            Assert.Equal("No reminder found for this news", actual.Message);
        }

        [Fact]
        public async Task AddReminderShouldThrowException()
        {
            string userId = "Jack";
            string email = "Jack@ymail.com";
            ReminderSchedule reminder = new ReminderSchedule {NewsId=101, Schedule=DateTime.Now.AddDays(3) };
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.IsReminderExists(userId, reminder.NewsId)).Returns(Task.FromResult(true));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<ReminderAlreadyExistsException>(() => service.CreateReminder(userId,email,reminder));
            Assert.Equal($"This News already have a reminder", actual.Message);
        }
        
        [Fact]
        public async Task GetRemindersShouldThrowException()
        {
            string userId = "Kevin";
            List<ReminderSchedule> lstreminder = null;
            var mockRepo = new Mock<IReminderRepository>();
            mockRepo.Setup(repo => repo.GetReminders(userId)).Returns(Task.FromResult(lstreminder));
            var service = new ReminderService.Services.ReminderService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<NoReminderFoundException>(() => service.GetReminders(userId));
            Assert.Equal("No reminders found for this user", actual.Message);
        }
    }
}
