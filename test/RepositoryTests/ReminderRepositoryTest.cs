using ReminderService.Models;
using ReminderService.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.InfraSetup;
using Xunit;

namespace Test.RepositoryTests
{
    [TestCaseOrderer("Test.PriorityOrderer", "test")]
    public class ReminderRepositoryTest:IClassFixture<ReminderDbFixture>
    {
        private readonly ReminderRepository repository;
        public ReminderRepositoryTest(ReminderDbFixture fixture)
        {
            repository = new ReminderRepository(fixture.context);
        }

        [Fact, TestPriority(1)]
        public async Task AddReminderShouldSuccess()
        {
            string userId = "John";
            string email = "john@gmail.com";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 101, Schedule = DateTime.Now };

            await repository.CreateReminder(userId,email,reminder);

            var status =await repository.IsReminderExists(userId, 101);
            Assert.True(status);
        }

        [Fact, TestPriority(2)]
        public async Task GetRemindersShouldReturnReminderList()
        {
            string userId = "Jack";

            var actual = await repository.GetReminders(userId);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<List<ReminderSchedule>>(actual);
            Assert.Single(actual);
        }

        [Fact, TestPriority(3)]
        public async Task UpdateReminderShouldSuccess()
        {
            string userId = "John";
            ReminderSchedule reminder = new ReminderSchedule { NewsId=101, Schedule=DateTime.Now.AddDays(7) };
            var actual = await repository.UpdateReminder(userId,reminder);
            Assert.True(actual);
        }

        [Fact, TestPriority(4)]
        public async Task RemoveReminderShouldSuccess()
        {
            string userId = "John";
            int newsId = 101;

            var actual = await repository.DeleteReminder(userId,newsId);
            Assert.True(actual);
        }

        [Fact, TestPriority(5)]
        public async Task GetRemindersShouldReturnNull()
        {
            string userId = "Kevin";

            var actual = await repository.GetReminders(userId);
            Assert.Null(actual);
        }

        [Fact, TestPriority(6)]
        public async Task UpdateReminderShouldFail()
        {
            string userId = "John";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 102, Schedule = Convert.ToDateTime("2019-12-30") };
            var actual = await repository.UpdateReminder(userId, reminder);
            Assert.False(actual);
        }

        [Fact, TestPriority(7)]
        public async Task RemoveReminderShouldFail()
        {
            string userId = "Kevin";
            int newsId = 101;

            var actual = await repository.DeleteReminder(userId, newsId);
            Assert.False(actual);
        }
    }
}
