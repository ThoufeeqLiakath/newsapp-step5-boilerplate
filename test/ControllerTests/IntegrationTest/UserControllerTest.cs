using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using UserService;
using UserService.Models;
using Xunit;

namespace Test.ControllerTests.IntegrationTest
{
    [TestCaseOrderer("Test.PriorityOrderer", "test")]
    public class UserControllerTest:IClassFixture<UserWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public UserControllerTest(UserWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact, TestPriority(1)]
        public async Task GetByIdShouldReturnUser()
        {
            // The endpoint or route of the controller action.
            string userId = "Jack";
            var httpResponse = await _client.GetAsync($"/api/user/{userId}");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserProfile>(stringResponse);
            Assert.Equal("Jackson", user.FirstName);
        }

        [Fact, TestPriority(2)]
        public async Task PostShouldSuccess()
        {
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "9877665544", Email = "john@gmail.com", CreatedAt = DateTime.Now };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync("/api/user", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        [Fact, TestPriority(3)]
        public async Task UpdateUserShouldSuccess()
        {
            string userId = "John";
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "9871546320", Email = "johnson@gmail.com", CreatedAt = DateTime.Now };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync($"/api/user/{userId}", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }
         
        [Fact, TestPriority(4)]
        public async Task DeleteUserShouldSuccess()
        {
            string userId = "John";
           
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(Convert.ToBoolean(stringResponse));
        }

        [Fact, TestPriority(5)]
        public async Task GetByIdShouldReturnNotFound()
        {
            string userId = "John";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id doesn't exist", stringResponse);
        }

        [Fact, TestPriority(6)]
        public async Task PostShouldReturnConflict()
        {
            UserProfile user = new UserProfile { UserId = "Jack", FirstName = "Jackson", LastName = "James", Contact = "9812345670", Email = "jack@ymail.com", CreatedAt = DateTime.Now };
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsync("/api/user", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Conflict, httpResponse.StatusCode);
            Assert.Equal($"{user.UserId} is already in use", stringResponse);
        }

        [Fact, TestPriority(7)]
        public async Task UpdateUserShouldReturnNotFound()
        {
            string userId = "Sam";
            UserProfile user = new UserProfile { UserId = "Sam", FirstName = "Sam", LastName = "Methews", Contact = "9812345677", Email = "sam@gmail.com", CreatedAt = DateTime.Now };
            HttpRequestMessage request = new HttpRequestMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PutAsync($"/api/user/{userId}", user, formatter);

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id doesn't exist", stringResponse);
        }

        [Fact, TestPriority(8)]
        public async Task DeleteUserShouldReturnNotFound()
        {
            string userId = "John";
            // The endpoint or route of the controller action.
            var httpResponse = await _client.DeleteAsync($"/api/user/{userId}");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.Equal($"This user id doesn't exist", stringResponse);
        }
    }
}
