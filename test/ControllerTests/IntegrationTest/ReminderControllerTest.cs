using Newtonsoft.Json;
using ReminderService;
using ReminderService.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace Test.ControllerTests.IntegrationTest
{

    [TestCaseOrderer("Test.PriorityOrderer", "test")]
    public class ReminderControllerTest:IClassFixture<ReminderWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public ReminderControllerTest(ReminderWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact, TestPriority(1)]
        public async Task GetByUserIdShouldReturnListOfReminder()
        {
            string userId = "Jack";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync($"/api/reminder/{userId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var reminder = JsonConvert.DeserializeObject<List<ReminderSchedule>>(stringResponse);
            Assert.NotNull(reminder);
            Assert.IsAssignableFrom<List<ReminderSchedule>>(reminder);
            Assert.Single(reminder);
            Assert.Equal(101,reminder[0].NewsId);
        }

        [Fact, TestPriority(2)]
        public async Task PostShouldReturnTrue()
        {
            Reminder reminder = new Reminder 
            {
                UserId="Jack",
                Email="jack@ymail.com",
                NewsReminders=new List<ReminderSchedule>
                {
                    new ReminderSchedule
                    { NewsId = 102, Schedule = DateTime.Now.AddDays(2) }
                } 
            };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync($"/api/reminder", reminder, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }
        [Fact, TestPriority(3)]
        public async Task DeleteShouldSuccess()
        {
            string userId = "Jack";
            int newsId = 102;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/reminder/{userId}/{newsId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        [Fact, TestPriority(4)]
        public async Task UpdateShouldSuccess()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule {NewsId=101, Schedule=DateTime.Now.AddDays(3) };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync($"/api/reminder/{userId}",reminder,formatter);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        [Fact, TestPriority(5)]
        public async Task GetByUserIdShouldReturnNotFound()
        {
            string userId = "Kevin";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync($"/api/reminder/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal("No reminders found for this user", stringResponse);
        }

        [Fact, TestPriority(6)]
        public async Task PostShouldReturnConflict()
        {
            // The endpoint or route of the controller action.
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
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            var httpResponse = await _client.PostAsync($"/api/reminder", reminder, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, httpResponse.StatusCode);
            Assert.Equal($"This News already have a reminder", stringResponse);
        }
        [Fact, TestPriority(7)]
        public async Task DeleteShouldReturnNotFound()
        {
            string userId = "Jack";
            int newsId = 102;
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/reminder/{userId}/{newsId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal("No reminder found for this news", stringResponse);
        }

        [Fact, TestPriority(8)]
        public async Task UpdateShouldReturnNotFound()
        {
            string userId = "Jack";
            ReminderSchedule reminder = new ReminderSchedule { NewsId = 102, Schedule = DateTime.Now.AddDays(3) };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync($"/api/reminder/{userId}", reminder, formatter);


            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal("No reminder found for this news", stringResponse);
        }
    }
}
