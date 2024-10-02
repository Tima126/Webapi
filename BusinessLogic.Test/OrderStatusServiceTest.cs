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
    public class OrderStatusServiceTest
    {
        private readonly OrderStatusService service;
        private readonly Mock<IOrderStatusRepository> orderStatusRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly OrderStatusValidator _validator;

        public OrderStatusServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            orderStatusRepositoryMoq = new Mock<IOrderStatusRepository>();

            repositoryWrapperMoq.Setup(x => x.OrderStatus)
                .Returns(orderStatusRepositoryMoq.Object);

            service = new OrderStatusService(repositoryWrapperMoq.Object);
            _validator = new OrderStatusValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrderStatus))]
        public async Task Create_InvalidOrderStatus_ThrowsValidationException(OrderStatus orderStatus)
        {
            // Arrange
            var orderStatusNew = orderStatus;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(orderStatusNew));

            // Assert
            orderStatusRepositoryMoq.Verify(x => x.Create(It.IsAny<OrderStatus>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrderStatus()
        {
            return new List<object[]>
            {
                new object[] {new OrderStatus() {StatusId = 0, StatusName = " "}},
                new object[] {new OrderStatus() {StatusId = 0, StatusName = ""}},
                new object[] {new OrderStatus() {StatusId = 1, StatusName = new string('a', 51)}},
            };
        }

        [Fact]
        public async Task Create_NullOrderStatus_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            orderStatusRepositoryMoq.Verify(x => x.Create(It.IsAny<OrderStatus>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidOrderStatus_UpdatesOrderStatusSuccessfully()
        {
            // Arrange
            var orderStatus = new OrderStatus()
            {
                StatusId = 1,
                StatusName = "Valid Status"
            };

            // Act
            await service.Update(orderStatus);

            // Assert
            orderStatusRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderStatus>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullOrderStatus_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            orderStatusRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderStatus>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrderStatusUpdate))]
        public async Task Update_InvalidOrderStatus_ThrowsValidationException(OrderStatus orderStatus)
        {
            // Arrange
            var orderStatusNew = orderStatus;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(orderStatusNew));

            // Assert
            orderStatusRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderStatus>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrderStatusUpdate()
        {
            return new List<object[]>
            {
                new object[] {new OrderStatus() {StatusId = 0, StatusName = " "}},
                new object[] {new OrderStatus() {StatusId = 0, StatusName = ""}},
                new object[] {new OrderStatus() {StatusId = 1, StatusName = new string('a', 51)}},
            };
        }

        [Fact]
        public async Task GetById_ValidOrderStatusId_ReturnsOrderStatus()
        {
            // Arrange
            int orderStatusId = 1;
            var expectedOrderStatus = new OrderStatus()
            {
                StatusId = 1,
                StatusName = "Valid Status"
            };

            orderStatusRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                .ReturnsAsync(new List<OrderStatus> { expectedOrderStatus });

            // Act
            var result = await service.GetById(orderStatusId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrderStatus.StatusId, result.StatusId);
            Assert.Equal(expectedOrderStatus.StatusName, result.StatusName);
        }

        [Fact]
        public async Task GetById_InvalidOrderStatusId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderStatusId = 999;

            // Act
            orderStatusRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                .ReturnsAsync(new List<OrderStatus>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(orderStatusId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidOrderStatusId_DeletesOrderStatusSuccessfully()
        {
            // Arrange
            int orderStatusId = 1;
            var orderStatus = new OrderStatus()
            {
                StatusId = 1,
                StatusName = "Valid Status"
            };

            // Act
            orderStatusRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                .ReturnsAsync(new List<OrderStatus> { orderStatus });
            await service.Delete(orderStatusId);

            // Assert
            orderStatusRepositoryMoq.Verify(x => x.Delete(It.IsAny<OrderStatus>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidOrderStatusId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderStatusId = 999;

            // Act
            orderStatusRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderStatus, bool>>>()))
                .ReturnsAsync(new List<OrderStatus>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(orderStatusId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            orderStatusRepositoryMoq.Verify(x => x.Delete(It.IsAny<OrderStatus>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}