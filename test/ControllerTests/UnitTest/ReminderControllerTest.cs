using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReminderService.Services;
using ReminderService.Models;
using ReminderService.Exceptions;
using System.Collections.Generic;
using ReminderService.Controllers;

namespace Test.ControllerTests.UnitTest
{
    public class ReminderControllerTest
    {
        [Fact]
        public async Task GetShouldReturnOk()
        {
            string userId = "Jack";
            List<ReminderSchedule> reminder = new List<ReminderSchedule>
                {
                    new ReminderSchedule
                    { NewsId = 101, Schedule = DateTime.Now.AddDays(2) }
                };
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.GetReminders(userId)).Returns(Task.FromResult(reminder));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Get(userId);
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsAssignableFrom<List<ReminderSchedule>>(actionResult.Value);
        }
        [Fact]
        public async Task GetShouldReturnNotFound()
        {
            string userId = "Jack";
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.GetReminders(userId)).Throws(new NoReminderFoundException("No reminders found for this user"));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Get(userId);
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal("No reminders found for this user", actionResult.Value);
        }

        [Fact]
        public async Task PostShouldReturnCreated()
        {
            Reminder reminder = new Reminder
            {
                UserId = "Jack",
                Email = "jack@ymail.com",
                NewsReminders = new List<ReminderSchedule>
                {
                    new ReminderSchedule
                    { NewsId = 102, Schedule = DateTime.Now.AddDays(2) }
                }
            };
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.CreateReminder(reminder.UserId,reminder.Email,reminder.NewsReminders[0])).Returns(Task.FromResult(true));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Post(reminder);
            var actionresult = Assert.IsType<CreatedResult>(actual);
            Assert.True(Convert.ToBoolean(actionresult.Value));
        }
        
        [Fact]
        public async Task PostShouldReturnConflict()
        {
            Reminder reminder = new Reminder
            {
                UserId = "Jack",
                Email = "jack@ymail.com",
                NewsReminders = new List<ReminderSchedule>
                {
                    new ReminderSchedule
                    { NewsId = 101, Schedule = DateTime.Now.AddDays(2) }
                }
            };
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.CreateReminder(reminder.UserId, reminder.Email, reminder.NewsReminders[0])).Throws(new ReminderAlreadyExistsException($"This News already have a reminder"));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Post(reminder);
            var actionResult = Assert.IsType<ConflictObjectResult>(actual);
            Assert.Equal($"This News already have a reminder", actionResult.Value);
        }
        
        [Fact]
        public async Task DeleteShouldReturnOk()
        {
            string userId = "Jack";
            int newsId = 101;
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.DeleteReminder(userId,newsId)).Returns(Task.FromResult(true));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Delete(userId, newsId);
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.True(Convert.ToBoolean(actionResult.Value));
        }

        [Fact]
        public async Task DeleteShouldReturnNotFound()
        {
            string userId = "Jack";
            int newsId = 101;
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.DeleteReminder(userId, newsId)).Throws(new NoReminderFoundException("No reminder found for this news")); 
           
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Delete(userId, newsId);
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal("No reminder found for this news", actionResult.Value);
        }

        [Fact]
        public async Task PutShouldReturnOk()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 101, Schedule = DateTime.Now.AddDays(2) };
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.UpdateReminder(userId,reminder)).Returns(Task.FromResult(true));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Put(userId, reminder);
            var actionresult = Assert.IsType<OkObjectResult>(actual);
            Assert.True(Convert.ToBoolean(actionresult.Value));
        }

        [Fact]
        public async Task PutShouldReturnNotFound()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 102, Schedule = DateTime.Now.AddDays(2) };
            var mockService = new Mock<IReminderService>();
            mockService.Setup(svc => svc.UpdateReminder(userId, reminder)).Throws(new NoReminderFoundException("No reminder found for this news"));
            var controller = new ReminderController(mockService.Object);

            var actual = await controller.Put(userId, reminder);
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal("No reminder found for this news", actionResult.Value);
        }
    }
}
