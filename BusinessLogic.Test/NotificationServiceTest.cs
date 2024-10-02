using BusinessLogic.Services;
using BusinessLogic.Validation;
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
    public class NotificationServiceTest
    {
        private readonly NotificationService service;
        private readonly Mock<INotificationRepository> notificationRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly NotificationValid _validator;

        public NotificationServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            notificationRepositoryMoq = new Mock<INotificationRepository>();

            repositoryWrapperMoq.Setup(x => x.Notification)
                .Returns(notificationRepositoryMoq.Object);

            service = new NotificationService(repositoryWrapperMoq.Object);
            _validator = new NotificationValid();
        }

        [Theory]
        [MemberData(nameof(GetIncorectNotification))]
        public async Task Create_InvalidNotification_ThrowsValidationException(Notification notification)
        {
            // Arrange
            var notificationNew = notification;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(notificationNew));

            // Assert
            notificationRepositoryMoq.Verify(x => x.Create(It.IsAny<Notification>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectNotification()
        {
            return new List<object[]>
            {
                new object[] {new Notification() {NotificationId = 0, UserId = null, Message = " ", SentDate = null}},
                new object[] {new Notification() {NotificationId = 0, UserId = null, Message = "", SentDate = DateTime.Now.AddDays(1)}},
                new object[] {new Notification() {NotificationId = 1, UserId = 1, Message = new string('a', 501), SentDate = DateTime.Now.AddDays(-1)}},
                new object[] {new Notification() {NotificationId = 1, UserId = 1, Message = "Valid message", SentDate = DateTime.Now.AddDays(1)}},
            };
        }

        [Fact]
        public async Task Create_NullNotification_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            notificationRepositoryMoq.Verify(x => x.Create(It.IsAny<Notification>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidNotification_UpdatesNotificationSuccessfully()
        {
            // Arrange
            var notification = new Notification()
            {
                NotificationId = 1,
                UserId = 1,
                Message = "Valid message",
                SentDate = DateTime.Now.AddDays(-1)
            };

            // Act
            await service.Update(notification);

            // Assert
            notificationRepositoryMoq.Verify(x => x.Update(It.IsAny<Notification>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullNotification_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            notificationRepositoryMoq.Verify(x => x.Update(It.IsAny<Notification>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectNotificationUpdate))]
        public async Task Update_InvalidNotification_ThrowsValidationException(Notification notification)
        {
            // Arrange
            var notificationNew = notification;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(notificationNew));

            // Assert
            notificationRepositoryMoq.Verify(x => x.Update(It.IsAny<Notification>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectNotificationUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Notification() {NotificationId = 0, UserId = null, Message = " ", SentDate = null}},
                new object[] {new Notification() {NotificationId = 0, UserId = null, Message = "", SentDate = DateTime.Now.AddDays(1)}},
                new object[] {new Notification() {NotificationId = 1, UserId = 1, Message = new string('a', 501), SentDate = DateTime.Now.AddDays(-1)}},
                new object[] {new Notification() {NotificationId = 1, UserId = 1, Message = "Valid message", SentDate = DateTime.Now.AddDays(1)}},
            };
        }

        [Fact]
        public async Task GetById_ValidNotificationId_ReturnsNotification()
        {
            // Arrange
            int notificationId = 1;
            var expectedNotification = new Notification()
            {
                NotificationId = 1,
                UserId = 1,
                Message = "Valid message",
                SentDate = DateTime.Now.AddDays(-1)
            };

            notificationRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification> { expectedNotification });

            // Act
            var result = await service.GetById(notificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNotification.NotificationId, result.NotificationId);
            Assert.Equal(expectedNotification.UserId, result.UserId);
            Assert.Equal(expectedNotification.Message, result.Message);
            Assert.Equal(expectedNotification.SentDate, result.SentDate);
        }

        [Fact]
        public async Task GetById_InvalidNotificationId_ThrowsInvalidOperationException()
        {
            // Arrange
            int notificationId = 999;

            // Act
            notificationRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(notificationId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidNotificationId_DeletesNotificationSuccessfully()
        {
            // Arrange
            int notificationId = 1;
            var notification = new Notification()
            {
                NotificationId = 1,
                UserId = 1,
                Message = "Valid message",
                SentDate = DateTime.Now.AddDays(-1)
            };

            // Act
            notificationRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification> { notification });
            await service.Delete(notificationId);

            // Assert
            notificationRepositoryMoq.Verify(x => x.Delete(It.IsAny<Notification>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidNotificationId_ThrowsInvalidOperationException()
        {
            // Arrange
            int notificationId = 999;

            // Act
            notificationRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .ReturnsAsync(new List<Notification>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(notificationId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            notificationRepositoryMoq.Verify(x => x.Delete(It.IsAny<Notification>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}