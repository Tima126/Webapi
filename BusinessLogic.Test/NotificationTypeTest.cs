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
    public class NotificationTypeTest
    {
        private readonly NotificationTypeService service;
        private readonly Mock<INotificationTypeRepository> notificationTypeRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly NotificationTypeValid _validator;

        public NotificationTypeTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            notificationTypeRepositoryMoq = new Mock<INotificationTypeRepository>();

            repositoryWrapperMoq.Setup(x => x.Notificationtype)
                .Returns(notificationTypeRepositoryMoq.Object);

            service = new NotificationTypeService(repositoryWrapperMoq.Object);
            _validator = new NotificationTypeValid();
        }

        [Theory]
        [MemberData(nameof(GetIncorectNotificationType))]
        public async Task Create_InvalidNotificationType_ThrowsValidationException(NotificationType notificationType)
        {
            // Arrange
            var notificationTypeNew = notificationType;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(notificationTypeNew));

            // Assert
            notificationTypeRepositoryMoq.Verify(x => x.Create(It.IsAny<NotificationType>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectNotificationType()
        {
            return new List<object[]>
            {
                new object[] {new NotificationType() {NotificationTypeId = 0, TypeName = " "}},
                new object[] {new NotificationType() {NotificationTypeId = 0, TypeName = ""}},
                new object[] {new NotificationType() {NotificationTypeId = 1, TypeName = new string('a', 21)}},
            };
        }

        [Fact]
        public async Task Create_NullNotificationType_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            notificationTypeRepositoryMoq.Verify(x => x.Create(It.IsAny<NotificationType>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidNotificationType_UpdatesNotificationTypeSuccessfully()
        {
            // Arrange
            var notificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                TypeName = "Valid Type"
            };

            // Act
            await service.Update(notificationType);

            // Assert
            notificationTypeRepositoryMoq.Verify(x => x.Update(It.IsAny<NotificationType>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullNotificationType_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            notificationTypeRepositoryMoq.Verify(x => x.Update(It.IsAny<NotificationType>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectNotificationTypeUpdate))]
        public async Task Update_InvalidNotificationType_ThrowsValidationException(NotificationType notificationType)
        {
            // Arrange
            var notificationTypeNew = notificationType;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(notificationTypeNew));

            // Assert
            notificationTypeRepositoryMoq.Verify(x => x.Update(It.IsAny<NotificationType>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectNotificationTypeUpdate()
        {
            return new List<object[]>
            {
                new object[] {new NotificationType() {NotificationTypeId = 0, TypeName = " "}},
                new object[] {new NotificationType() {NotificationTypeId = 0, TypeName = ""}},
                new object[] {new NotificationType() {NotificationTypeId = 1, TypeName = new string('a', 21)}},
            };
        }

        [Fact]
        public async Task GetById_ValidNotificationTypeId_ReturnsNotificationType()
        {
            // Arrange
            int notificationTypeId = 1;
            var expectedNotificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                TypeName = "Valid Type"
            };

            notificationTypeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<NotificationType, bool>>>()))
                .ReturnsAsync(new List<NotificationType> { expectedNotificationType });

            // Act
            var result = await service.GetById(notificationTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNotificationType.NotificationTypeId, result.NotificationTypeId);
            Assert.Equal(expectedNotificationType.TypeName, result.TypeName);
        }

        [Fact]
        public async Task GetById_InvalidNotificationTypeId_ThrowsInvalidOperationException()
        {
            // Arrange
            int notificationTypeId = 999;

            // Act
            notificationTypeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<NotificationType, bool>>>()))
                .ReturnsAsync(new List<NotificationType>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(notificationTypeId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidNotificationTypeId_DeletesNotificationTypeSuccessfully()
        {
            // Arrange
            int notificationTypeId = 1;
            var notificationType = new NotificationType()
            {
                NotificationTypeId = 1,
                TypeName = "Valid Type"
            };

            // Act
            notificationTypeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<NotificationType, bool>>>()))
                .ReturnsAsync(new List<NotificationType> { notificationType });
            await service.Delete(notificationTypeId);

            // Assert
            notificationTypeRepositoryMoq.Verify(x => x.Delete(It.IsAny<NotificationType>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidNotificationTypeId_ThrowsInvalidOperationException()
        {
            // Arrange
            int notificationTypeId = 999;

            // Act
            notificationTypeRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<NotificationType, bool>>>()))
                .ReturnsAsync(new List<NotificationType>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(notificationTypeId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            notificationTypeRepositoryMoq.Verify(x => x.Delete(It.IsAny<NotificationType>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}