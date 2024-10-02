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
    public class OrderServiceTest
    {
        private readonly OrderService service;
        private readonly Mock<IOrderRepository> orderRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly OrderValidator _validator;

        public OrderServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            orderRepositoryMoq = new Mock<IOrderRepository>();

            repositoryWrapperMoq.Setup(x => x.Order)
                .Returns(orderRepositoryMoq.Object);

            service = new OrderService(repositoryWrapperMoq.Object);
            _validator = new OrderValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrder))]
        public async Task Create_InvalidOrder_ThrowsValidationException(Order order)
        {
            // Arrange
            var orderNew = order;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(orderNew));

            // Assert
            orderRepositoryMoq.Verify(x => x.Create(It.IsAny<Order>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrder()
        {
            return new List<object[]>
            {
                new object[] {new Order() {OrderId = 0, UserId = null, OrderDate = null, TotalAmount = 0, StatusId = null, DiscountId = null}},
                new object[] {new Order() {OrderId = 0, UserId = 0, OrderDate = DateTime.Now.AddDays(1), TotalAmount = 0, StatusId = 0, DiscountId = -1}},
                new object[] {new Order() {OrderId = 1, UserId = 1, OrderDate = DateTime.Now.AddDays(-1), TotalAmount = -1, StatusId = 1, DiscountId = 1}},
                new object[] {new Order() {OrderId = 1, UserId = 1, OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 100, StatusId = 0, DiscountId = 1}},
            };
        }

        [Fact]
        public async Task Create_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            orderRepositoryMoq.Verify(x => x.Create(It.IsAny<Order>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidOrder_UpdatesOrderSuccessfully()
        {
            // Arrange
            var order = new Order()
            {
                OrderId = 1,
                UserId = 1,
                OrderDate = DateTime.Now.AddDays(-1),
                TotalAmount = 100,
                StatusId = 1,
                DiscountId = 1
            };

            // Act
            await service.Update(order);

            // Assert
            orderRepositoryMoq.Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            orderRepositoryMoq.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrderUpdate))]
        public async Task Update_InvalidOrder_ThrowsValidationException(Order order)
        {
            // Arrange
            var orderNew = order;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(orderNew));

            // Assert
            orderRepositoryMoq.Verify(x => x.Update(It.IsAny<Order>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrderUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Order() {OrderId = 0, UserId = null, OrderDate = null, TotalAmount = 0, StatusId = null, DiscountId = null}},
                new object[] {new Order() {OrderId = 0, UserId = 0, OrderDate = DateTime.Now.AddDays(1), TotalAmount = 0, StatusId = 0, DiscountId = -1}},
                new object[] {new Order() {OrderId = 1, UserId = 1, OrderDate = DateTime.Now.AddDays(-1), TotalAmount = -1, StatusId = 1, DiscountId = 1}},
                new object[] {new Order() {OrderId = 1, UserId = 1, OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 100, StatusId = 0, DiscountId = 1}},
            };
        }

        [Fact]
        public async Task GetById_ValidOrderId_ReturnsOrder()
        {
            // Arrange
            int orderId = 1;
            var expectedOrder = new Order()
            {
                OrderId = 1,
                UserId = 1,
                OrderDate = DateTime.Now.AddDays(-1),
                TotalAmount = 100,
                StatusId = 1,
                DiscountId = 1
            };

            orderRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new List<Order> { expectedOrder });

            // Act
            var result = await service.GetById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.OrderId, result.OrderId);
            Assert.Equal(expectedOrder.UserId, result.UserId);
            Assert.Equal(expectedOrder.OrderDate, result.OrderDate);
            Assert.Equal(expectedOrder.TotalAmount, result.TotalAmount);
            Assert.Equal(expectedOrder.StatusId, result.StatusId);
            Assert.Equal(expectedOrder.DiscountId, result.DiscountId);
        }

        [Fact]
        public async Task GetById_InvalidOrderId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderId = 999;

            // Act
            orderRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new List<Order>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(orderId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidOrderId_DeletesOrderSuccessfully()
        {
            // Arrange
            int orderId = 1;
            var order = new Order()
            {
                OrderId = 1,
                UserId = 1,
                OrderDate = DateTime.Now.AddDays(-1),
                TotalAmount = 100,
                StatusId = 1,
                DiscountId = 1
            };

            // Act
            orderRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new List<Order> { order });
            await service.Delete(orderId);

            // Assert
            orderRepositoryMoq.Verify(x => x.Delete(It.IsAny<Order>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidOrderId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderId = 999;

            // Act
            orderRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new List<Order>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(orderId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            orderRepositoryMoq.Verify(x => x.Delete(It.IsAny<Order>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}