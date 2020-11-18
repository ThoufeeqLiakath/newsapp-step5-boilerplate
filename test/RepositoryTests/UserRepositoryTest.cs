using System;
using System.Threading.Tasks;
using Test.InfraSetup;
using UserService.Models;
using UserService.Repository;
using Xunit;

namespace Test.RepositoryTests
{
    [TestCaseOrderer("Test.PriorityOrderer", "test")]
    public class UserRepositoryTest:IClassFixture<UserDbFixture>
    {
        private readonly UserRepository repository;
        public UserRepositoryTest(UserDbFixture fixture)
        {
            repository = new UserRepository(fixture.context);
        }

        [Fact, TestPriority(1)]
        public async Task AddUserShouldSuccess()
        {
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "7869543210", Email = "john@gmail.com", CreatedAt = DateTime.Now };

            var actual = await repository.AddUser(user);
            Assert.True(actual);

            var _user = await repository.GetUser("John");
            Assert.IsAssignableFrom<UserProfile>(_user);
            Assert.Equal("Johnson", _user.FirstName);
        }

        [Fact, TestPriority(2)]
        public async Task GetUserShouldSuccess()
        {
            var _user = await repository.GetUser("John");
            Assert.Equal("7869543210", _user.Contact);
            Assert.Equal("john@gmail.com", _user.Email);
        }

        [Fact, TestPriority(3)]
        public async Task UpdateUserShouldSuccess()
        {
            var user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "9988776655", Email = "john@gmail.com", CreatedAt = DateTime.Now };
            var actual = await repository.UpdateUser(user);
            Assert.True(actual);
        }

        [Fact, TestPriority(4)]
        public async Task DeleteUserShouldSuccess()
        {
            string userId = "John";
            var actual = await repository.DeleteUser(userId);
            Assert.True(actual);
        }

        [Fact, TestPriority(5)]
        public async Task DeleteUserShouldFail()
        {
            string userId = "John";
            var actual = await repository.DeleteUser(userId);
            Assert.False(actual);
        }
        [Fact, TestPriority(6)]
        public async Task GetUserShouldFail()
        {
            var _user = await repository.GetUser("John");
            Assert.Null(_user);
        }
    }
}
