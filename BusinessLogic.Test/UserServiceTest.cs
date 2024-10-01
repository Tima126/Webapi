using BusinessLogic.Services;
using Domain.interfaces;
using Domain.interfaces.Repository;
using Domain.Models;
using FluentValidation.TestHelper;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Xunit;

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
        public async Task Create_NullUser_ThrowsArgumentNullException()
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
        public async Task Create_InvalidUser_ThrowsValidationException(User user)
        {
            // Arrange
            var Usernew = user;

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(() => service.Create(Usernew));

            // Assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            Assert.IsType<ValidationException>(ex);
        }


        [Fact]
        public async Task Update_ValidUser_UpdatesUserSuccessfully()
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
        public async Task Update_NullUser_ThrowsArgumentNullException()
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
        public async Task GetById_ValidUserId_ReturnsUser()
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
        public async Task GetById_InvalidUserId_ThrowsInvalidOperationException()
        {
            // Arrange
            int userId = 999;

            // Act
            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(userId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_InvalidUserId_ThrowsInvalidOperationException()
        {
            // Arrange
            int userId = 999;

            // Act
            userRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(userId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ValidUserId_DeletesUserSuccessfully()
        {
            // Arrange
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
                new object[] {new User() {UserId=0, FirstName ="dfdfdf", LastName=" ", Email="", PasswordHash="", PhoneNumber = " ", AddressId=1} },
                new object[] {new User() {UserId=1, FirstName ="", LastName="dfdfd", Email="", PasswordHash=" ", PhoneNumber = "   ", AddressId=0} },
                new object[] {new User() {UserId=0, FirstName ="", LastName=" ", Email="dfdfdf", PasswordHash="", PhoneNumber = "dfdfd", AddressId=0} },
                new object[] {new User() {UserId=1, FirstName =" ", LastName=" ", Email="    ", PasswordHash="dfdfdf", PhoneNumber = "", AddressId=2} },
            };
        }

        [Fact]
        public void FirstName_ShouldNotBeEmpty()
        {
            var user = new User { FirstName = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
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
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
                
        }

        [Fact]
        public void LastName_ShouldNotBeEmpty()
        {
            var user = new User { LastName = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.LastName);
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