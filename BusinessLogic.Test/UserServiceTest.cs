using BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using Domain.interfaces.Repository;
using Domain.interfaces;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Test
{
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public UserServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();

            repositoryWrapperMoq.Setup(x => x.User)
                .Returns(userRepositoryMoq.Object);

            service = new UserService(repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task NullUser()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUservalidation()
        {
            var newUser = new User()
            {
                FirstName = "",
                LastName = "",
                Email = "",
                PasswordHash = "",
                PhoneNumber = "",
                AddressId = 0,
            };
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Create(newUser));
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserCreate()
        {
            var newUser = new User()
            {
                FirstName = "cvcv",
                LastName = "cvcv",
                Email = "cvcv",
                PasswordHash = "ccc",
                PhoneNumber = "23445",
                AddressId = 1,
            };

            await service.Create(newUser);

            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var user = new User()
            {
                FirstName = "gg",
                LastName = "gg",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                AddressId = 1,
            };

            await service.Update(user);

            userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullUser()
        {
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            Assert.IsType<ArgumentNullException>(ex);
            userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task GetById_Success()
        {
            int userId = 1;
            var expectedUser = new User()
            {
                UserId = userId,
                FirstName = "gg",
                LastName = "gg",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                AddressId = 1,
            };

            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            var result = await service.GetById(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.FirstName, result.FirstName);
            Assert.Equal(expectedUser.LastName, result.LastName);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.PasswordHash, result.PasswordHash);
            Assert.Equal(expectedUser.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedUser.AddressId, result.AddressId);
        }

        [Fact]
        public async Task GetById_UserNotFound()
        {
            int userId = 999;

            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(userId));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task DeleteAsync_UserNotFound()
        {
            int userId = 999;

            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(userId));

            Assert.IsType<InvalidOperationException>(ex);
            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            int userId = 1;
            var user = new User()
            {
                UserId = userId,
                FirstName = "gg",
                LastName = "gg",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                AddressId = 1,
            };

            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            await service.Delete(userId);

            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

    }
}