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
    public class PaymentServiceTest
    {
        private readonly PaymentService service;
        private readonly Mock<IPaymentRepository> paymentRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly PaymentValidator _validator;

        public PaymentServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            paymentRepositoryMoq = new Mock<IPaymentRepository>();

            repositoryWrapperMoq.Setup(x => x.Payment)
                .Returns(paymentRepositoryMoq.Object);

            service = new PaymentService(repositoryWrapperMoq.Object);
            _validator = new PaymentValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectPayment))]
        public async Task Create_InvalidPayment_ThrowsValidationException(Payment payment)
        {
            // Arrange
            var paymentNew = payment;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(paymentNew));

            // Assert
            paymentRepositoryMoq.Verify(x => x.Create(It.IsAny<Payment>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectPayment()
        {
            return new List<object[]>
            {
                new object[] {new Payment() {PaymentId = 0, OrderId = null, PaymentDate = null, Amount = 0, PaymentMethodId = null}},
                new object[] {new Payment() {PaymentId = 0, OrderId = 0, PaymentDate = DateTime.Now.AddDays(1), Amount = 0, PaymentMethodId = 0}},
                new object[] {new Payment() {PaymentId = 1, OrderId = 1, PaymentDate = DateTime.Now.AddDays(-1), Amount = -1, PaymentMethodId = 1}},
                new object[] {new Payment() {PaymentId = 1, OrderId = 1, PaymentDate = DateTime.Now.AddDays(-1), Amount = 100, PaymentMethodId = 0}},
            };
        }

        [Fact]
        public async Task Create_NullPayment_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            paymentRepositoryMoq.Verify(x => x.Create(It.IsAny<Payment>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidPayment_UpdatesPaymentSuccessfully()
        {
            // Arrange
            var payment = new Payment()
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentDate = DateTime.Now.AddDays(-1),
                Amount = 100,
                PaymentMethodId = 1
            };

            // Act
            await service.Update(payment);

            // Assert
            paymentRepositoryMoq.Verify(x => x.Update(It.IsAny<Payment>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullPayment_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            paymentRepositoryMoq.Verify(x => x.Update(It.IsAny<Payment>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectPaymentUpdate))]
        public async Task Update_InvalidPayment_ThrowsValidationException(Payment payment)
        {
            // Arrange
            var paymentNew = payment;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(paymentNew));

            // Assert
            paymentRepositoryMoq.Verify(x => x.Update(It.IsAny<Payment>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectPaymentUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Payment() {PaymentId = 0, OrderId = null, PaymentDate = null, Amount = 0, PaymentMethodId = null}},
                new object[] {new Payment() {PaymentId = 0, OrderId = 0, PaymentDate = DateTime.Now.AddDays(1), Amount = 0, PaymentMethodId = 0}},
                new object[] {new Payment() {PaymentId = 1, OrderId = 1, PaymentDate = DateTime.Now.AddDays(-1), Amount = -1, PaymentMethodId = 1}},
                new object[] {new Payment() {PaymentId = 1, OrderId = 1, PaymentDate = DateTime.Now.AddDays(-1), Amount = 100, PaymentMethodId = 0}},
            };
        }

        [Fact]
        public async Task GetById_ValidPaymentId_ReturnsPayment()
        {
            // Arrange
            int paymentId = 1;
            var expectedPayment = new Payment()
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentDate = DateTime.Now.AddDays(-1),
                Amount = 100,
                PaymentMethodId = 1
            };

            paymentRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .ReturnsAsync(new List<Payment> { expectedPayment });

            // Act
            var result = await service.GetById(paymentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPayment.PaymentId, result.PaymentId);
            Assert.Equal(expectedPayment.OrderId, result.OrderId);
            Assert.Equal(expectedPayment.PaymentDate, result.PaymentDate);
            Assert.Equal(expectedPayment.Amount, result.Amount);
            Assert.Equal(expectedPayment.PaymentMethodId, result.PaymentMethodId);
        }

        [Fact]
        public async Task GetById_InvalidPaymentId_ThrowsInvalidOperationException()
        {
            // Arrange
            int paymentId = 999;

            // Act
            paymentRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .ReturnsAsync(new List<Payment>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(paymentId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidPaymentId_DeletesPaymentSuccessfully()
        {
            // Arrange
            int paymentId = 1;
            var payment = new Payment()
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentDate = DateTime.Now.AddDays(-1),
                Amount = 100,
                PaymentMethodId = 1
            };

            // Act
            paymentRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .ReturnsAsync(new List<Payment> { payment });
            await service.Delete(paymentId);

            // Assert
            paymentRepositoryMoq.Verify(x => x.Delete(It.IsAny<Payment>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidPaymentId_ThrowsInvalidOperationException()
        {
            // Arrange
            int paymentId = 999;

            // Act
            paymentRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .ReturnsAsync(new List<Payment>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(paymentId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            paymentRepositoryMoq.Verify(x => x.Delete(It.IsAny<Payment>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}