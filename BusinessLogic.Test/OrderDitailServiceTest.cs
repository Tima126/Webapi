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
    public class OrderDetailServiceTest
    {
        private readonly OrderDetailsService service;
        private readonly Mock<IOrderDetailRepository> orderDetailRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly OrderDetailsValid _validator;

        public OrderDetailServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            orderDetailRepositoryMoq = new Mock<IOrderDetailRepository>();

            repositoryWrapperMoq.Setup(x => x.OrderDetail)
                .Returns(orderDetailRepositoryMoq.Object);

            service = new OrderDetailsService(repositoryWrapperMoq.Object);
            _validator = new OrderDetailsValid();
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrderDetail))]
        public async Task Create_InvalidOrderDetail_ThrowsValidationException(OrderDetail orderDetail)
        {
            // Arrange
            var orderDetailNew = orderDetail;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(orderDetailNew));

            // Assert
            orderDetailRepositoryMoq.Verify(x => x.Create(It.IsAny<OrderDetail>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrderDetail()
        {
            return new List<object[]>
            {
                new object[] {new OrderDetail() {OrderDetailId = 0, OrderId = null, ProductId = null, Quantity = 0, Price = 0}},
                new object[] {new OrderDetail() {OrderDetailId = 0, OrderId = 0, ProductId = 0, Quantity = 0, Price = 0}},
                new object[] {new OrderDetail() {OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = -1, Price = 100}},
                new object[] {new OrderDetail() {OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = -100}},
            };
        }

        [Fact]
        public async Task Create_NullOrderDetail_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            orderDetailRepositoryMoq.Verify(x => x.Create(It.IsAny<OrderDetail>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidOrderDetail_UpdatesOrderDetailSuccessfully()
        {
            // Arrange
            var orderDetail = new OrderDetail()
            {
                OrderDetailId = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 100
            };

            // Act
            await service.Update(orderDetail);

            // Assert
            orderDetailRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderDetail>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullOrderDetail_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            orderDetailRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderDetail>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectOrderDetailUpdate))]
        public async Task Update_InvalidOrderDetail_ThrowsValidationException(OrderDetail orderDetail)
        {
            // Arrange
            var orderDetailNew = orderDetail;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(orderDetailNew));

            // Assert
            orderDetailRepositoryMoq.Verify(x => x.Update(It.IsAny<OrderDetail>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectOrderDetailUpdate()
        {
            return new List<object[]>
            {
                new object[] {new OrderDetail() {OrderDetailId = 0, OrderId = null, ProductId = null, Quantity = 0, Price = 0}},
                new object[] {new OrderDetail() {OrderDetailId = 0, OrderId = 0, ProductId = 0, Quantity = 0, Price = 0}},
                new object[] {new OrderDetail() {OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = -1, Price = 100}},
                new object[] {new OrderDetail() {OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 1, Price = -100}},
            };
        }

        [Fact]
        public async Task GetById_ValidOrderDetailId_ReturnsOrderDetail()
        {
            // Arrange
            int orderDetailId = 1;
            var expectedOrderDetail = new OrderDetail()
            {
                OrderDetailId = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 100
            };

            orderDetailRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .ReturnsAsync(new List<OrderDetail> { expectedOrderDetail });

            // Act
            var result = await service.GetById(orderDetailId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrderDetail.OrderDetailId, result.OrderDetailId);
            Assert.Equal(expectedOrderDetail.OrderId, result.OrderId);
            Assert.Equal(expectedOrderDetail.ProductId, result.ProductId);
            Assert.Equal(expectedOrderDetail.Quantity, result.Quantity);
            Assert.Equal(expectedOrderDetail.Price, result.Price);
        }

        [Fact]
        public async Task GetById_InvalidOrderDetailId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderDetailId = 999;

            // Act
            orderDetailRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .ReturnsAsync(new List<OrderDetail>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(orderDetailId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidOrderDetailId_DeletesOrderDetailSuccessfully()
        {
            // Arrange
            int orderDetailId = 1;
            var orderDetail = new OrderDetail()
            {
                OrderDetailId = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 100
            };

            // Act
            orderDetailRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .ReturnsAsync(new List<OrderDetail> { orderDetail });
            await service.Delete(orderDetailId);

            // Assert
            orderDetailRepositoryMoq.Verify(x => x.Delete(It.IsAny<OrderDetail>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidOrderDetailId_ThrowsInvalidOperationException()
        {
            // Arrange
            int orderDetailId = 999;

            // Act
            orderDetailRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
                .ReturnsAsync(new List<OrderDetail>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(orderDetailId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            orderDetailRepositoryMoq.Verify(x => x.Delete(It.IsAny<OrderDetail>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}