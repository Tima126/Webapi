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
using System.ComponentModel.DataAnnotations;
using FluentValidation.TestHelper;

namespace BusinessLogic.Test
{
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly UserValidator _validator;

        public UserServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();

            repositoryWrapperMoq.Setup(x => x.User)
                .Returns(userRepositoryMoq.Object);

            service = new UserService(repositoryWrapperMoq.Object);
            _validator = new UserValidator();
        }


        [Fact]
        public async Task NullUserCreate()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }


        [Theory]
        [MemberData(nameof(GetIncorectUsers))]
        public async Task CreateAsyncNewUserCreate(User user)
        {
            // Arrange
            var Usernew = user;

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() => service.Create(Usernew));

            // Assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            Assert.IsType<ValidationException>(ex);
        }
        [Theory]
        [MemberData(nameof(GetIncorectUsers))]
        public async Task UserCreate_Space(User user)
        {
            // Arrange
            var Userspace = user;

            // Act
            var ex = await Assert.ThrowsAnyAsync<ValidationException>(() => service.Create(Userspace));

            // Assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);

        }


        [Fact]
        public async Task UpdateAsync_Success()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "gg",
                LastName = "gg",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                AddressId = 1,
            };
            // Act
            await service.Update(user);
            // Assert
            userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullUser()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetById_Success()
        {
            // Arrange
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

            // Act
            var result = await service.GetById(userId);

            // Assert
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
        {   // Arange
            int userId = 999;

            // Act
            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(userId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task DeleteAsync_UserNotFound()
        {   // Arange
            int userId = 999;
            // Act
            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            // Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(userId));

            Assert.IsType<InvalidOperationException>(ex);
            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {   // Arange
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
            // Act
            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });
            await service.Delete(userId);

            // Assert
            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }



        public static IEnumerable<object[]> GetIncorectUsers()
        {
            return new List<object[]>
            {
                new object[] {new User() {UserId=1, FirstName ="", LastName="", Email="", PasswordHash="", PhoneNumber = "", AddressId=0} },
                new object[] {new User() {UserId=0, FirstName ="dfdfdf", LastName="", Email="", PasswordHash="", PhoneNumber = "", AddressId=1} },
                new object[] {new User() {UserId=1, FirstName ="", LastName="dfdfd", Email="", PasswordHash="", PhoneNumber = "", AddressId=0} },
                new object[] {new User() {UserId=0, FirstName ="", LastName="", Email="dfdfdf", PasswordHash="", PhoneNumber = "dfdfd", AddressId=0} },
                new object[] {new User() {UserId=1, FirstName ="", LastName="", Email="", PasswordHash="dfdfdf", PhoneNumber = "", AddressId=2} },
            };
        }

        public static IEnumerable<object[]> GetSpaceUsers()
        {
            return new List<object[]>
            {
                new object[] {new User() {UserId=1, FirstName =" ", LastName="sfgf", Email="dgdgd", PasswordHash="dfdfdf", PhoneNumber = "dfdfdf", AddressId=1} },
                new object[] {new User() {UserId=0, FirstName ="dfdfdf", LastName=" ", Email="", PasswordHash="", PhoneNumber = "", AddressId=1} },
                new object[] {new User() {UserId=1, FirstName ="", LastName="dfdfd", Email="", PasswordHash="", PhoneNumber = "", AddressId=0} },
                new object[] {new User() {UserId=0, FirstName ="", LastName="", Email="dfdfdf", PasswordHash="", PhoneNumber = "dfdfd", AddressId=0} },
                new object[] {new User() {UserId=1, FirstName ="", LastName="", Email="", PasswordHash="dfdfdf", PhoneNumber = "", AddressId=2} },
            };
        }

        [Fact]
        public void FirstName_ShouldNotBeEmpty()
        {
            var user = new User { FirstName = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.FirstName)
                .WithErrorMessage("First name is required.");
        }

        [Fact]
        public void FirstName_ShouldNotExceedMaxLength()
        {
            var user = new User { FirstName = new string('a', 21) };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
        }

        [Fact]
        public void FirstName_ShouldContainOnlyLetters()
        {
            var user = new User { FirstName = "John123" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.FirstName)
                .WithErrorMessage("First name must contain only letters and no spaces.");
        }

        [Fact]
        public void LastName_ShouldNotBeEmpty()
        {
            var user = new User { LastName = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.LastName)
                .WithErrorMessage("Last name is required.");
        }

        [Fact]
        public void LastName_ShouldNotExceedMaxLength()
        {
            var user = new User { LastName = new string('a', 21) };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public void LastName_ShouldContainOnlyLetters()
        {
            var user = new User { LastName = "Doe123" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public void Email_ShouldNotBeEmpty()
        {
            var user = new User { Email = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email);
            
        }

        [Fact]
        public void Email_ShouldBeValid()
        {
            var user = new User { Email = "invalid-email" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Email);
              
        }

        [Fact]
        public void PhoneNumber_ShouldNotBeEmpty()
        {
            var user = new User { PhoneNumber = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.PhoneNumber);

        }

        [Fact]
        public void PhoneNumber_ShouldContainOnlyDigits()
        {
            var user = new User { PhoneNumber = "123-456-7890" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.PhoneNumber);
        }

        [Fact]
        public void AddressId_ShouldBeGreaterThanZero()
        {
            var user = new User { AddressId = 0 };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.AddressId);

        }

        [Fact]
        public void ValidUser_ShouldPassValidation()
        {
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                AddressId = 1
            };
            var result = _validator.TestValidate(user);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
