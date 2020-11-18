using Moq;
using System;
using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
using Xunit;


namespace Test.ServiceTests
{
    public class UserServiceTest
    {
        [Fact]
        public async Task AddUserShouldSuccess()
        {
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "7869543210", Email = "john@gmail.com", CreatedAt = DateTime.Now };
            UserProfile _user = null;
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(user.UserId)).Returns(Task.FromResult(_user));
            mockRepo.Setup(repo => repo.AddUser(user)).Returns(Task.FromResult(true));
            
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual =await service.AddUser(user);
            Assert.True(actual);
        }

        [Fact]
        public async Task GetUserShouldReturnUser()
        {
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "7869543210", Email = "john@gmail.com", CreatedAt = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(user.UserId)).Returns(Task.FromResult(user));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual = await service.GetUser(user.UserId);
            Assert.NotNull(actual);
            Assert.IsAssignableFrom<UserProfile>(actual);
            Assert.Equal("Johnson",actual.FirstName);
        }

        [Fact]
        public async Task DeleteUserShouldSuccess()
        {
            string userId = "John";
            UserProfile user = new UserProfile { UserId = "John", FirstName = "Johnson", LastName = "dsouza", Contact = "7869543210", Email = "john@gmail.com", CreatedAt = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(user));
            mockRepo.Setup(repo => repo.DeleteUser(userId)).Returns(Task.FromResult(true));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual = await service.DeleteUser(userId);
            Assert.True(actual);
        }

        [Fact]
        public async Task UpdateUserShouldSuccess()
        {
            string userId = "Jack";
            UserProfile user = new UserProfile { UserId = "Jack", FirstName = "Jackson", LastName = "James", Contact = "9812345670", Email = "jack@ymail.com", CreatedAt = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(user));
            mockRepo.Setup(repo => repo.UpdateUser(user)).Returns(Task.FromResult(true));
            var service = new UserService.Services.UserService(mockRepo.Object);

            user.Contact = "9888776655";
            user.Email = "jackson@ymail.com";
            var actual = await service.UpdateUser(userId, user);
            Assert.True(actual);
        }

        [Fact]
        public async Task AddUserShouldThrowException()
        {
            UserProfile user = new UserProfile { UserId = "Jack", FirstName = "Jackson", LastName = "James", Contact = "9812345670", Email = "jack@ymail.com", CreatedAt = DateTime.Now };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(user.UserId)).Returns(Task.FromResult(user));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual =await Assert.ThrowsAsync<UserAlreadyExistsException>(()=>service.AddUser(user));
            Assert.Equal($"{user.UserId} is already in use", actual.Message);
        }

        [Fact]
        public async Task GetUserShouldThrowException()
        {
            var mockRepo = new Mock<IUserRepository>();
            string userId = "Kevin";
            UserProfile user = null;
            mockRepo.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(user));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<UserNotFoundException>(() => service.GetUser(userId));
            Assert.Equal($"This user id doesn't exist", actual.Message);
        }

        [Fact]
        public async Task DeleteUserShouldThrowException()
        {
            string userId = "John";
            UserProfile user = null;
            var mockRepo = new Mock<IUserRepository>();                     
            mockRepo.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(user));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<UserNotFoundException>(() => service.DeleteUser(userId));
            Assert.Equal($"This user id doesn't exist", actual.Message);
        }

        [Fact]
        public async Task UpdateUserShouldThrowException()
        {
            string userId = "John";
            UserProfile user = null;
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(user));
            var service = new UserService.Services.UserService(mockRepo.Object);

            var actual = await Assert.ThrowsAsync<UserNotFoundException>(() => service.UpdateUser(userId,user));
            Assert.Equal($"This user id doesn't exist", actual.Message);
        }
    }
}
