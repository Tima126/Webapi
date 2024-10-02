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
    public class PaymentMethodServiceTest
    {
        private readonly PaymentMethodService service;
        private readonly Mock<IPaymentMethodRepository> paymentMethodRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly PaymentMethodValidator _validator;

        public PaymentMethodServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            paymentMethodRepositoryMoq = new Mock<IPaymentMethodRepository>();

            repositoryWrapperMoq.Setup(x => x.PaymentMethod)
                .Returns(paymentMethodRepositoryMoq.Object);

            service = new PaymentMethodService(repositoryWrapperMoq.Object);
            _validator = new PaymentMethodValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectPaymentMethod))]
        public async Task Create_InvalidPaymentMethod_ThrowsValidationException(PaymentMethod paymentMethod)
        {
            // Arrange
            var paymentMethodNew = paymentMethod;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(paymentMethodNew));

            // Assert
            paymentMethodRepositoryMoq.Verify(x => x.Create(It.IsAny<PaymentMethod>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectPaymentMethod()
        {
            return new List<object[]>
            {
                new object[] {new PaymentMethod() {PaymentMethodId = 0, MethodName = " "}},
                new object[] {new PaymentMethod() {PaymentMethodId = 0, MethodName = ""}},
                new object[] {new PaymentMethod() {PaymentMethodId = 1, MethodName = new string('a', 51)}},
            };
        }

        [Fact]
        public async Task Create_NullPaymentMethod_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            paymentMethodRepositoryMoq.Verify(x => x.Create(It.IsAny<PaymentMethod>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidPaymentMethod_UpdatesPaymentMethodSuccessfully()
        {
            // Arrange
            var paymentMethod = new PaymentMethod()
            {
                PaymentMethodId = 1,
                MethodName = "Valid Method"
            };

            // Act
            await service.Update(paymentMethod);

            // Assert
            paymentMethodRepositoryMoq.Verify(x => x.Update(It.IsAny<PaymentMethod>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullPaymentMethod_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            paymentMethodRepositoryMoq.Verify(x => x.Update(It.IsAny<PaymentMethod>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectPaymentMethodUpdate))]
        public async Task Update_InvalidPaymentMethod_ThrowsValidationException(PaymentMethod paymentMethod)
        {
            // Arrange
            var paymentMethodNew = paymentMethod;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(paymentMethodNew));

            // Assert
            paymentMethodRepositoryMoq.Verify(x => x.Update(It.IsAny<PaymentMethod>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectPaymentMethodUpdate()
        {
            return new List<object[]>
            {
                new object[] {new PaymentMethod() {PaymentMethodId = 0, MethodName = " "}},
                new object[] {new PaymentMethod() {PaymentMethodId = 0, MethodName = ""}},
                new object[] {new PaymentMethod() {PaymentMethodId = 1, MethodName = new string('a', 51)}},
            };
        }

        [Fact]
        public async Task GetById_ValidPaymentMethodId_ReturnsPaymentMethod()
        {
            // Arrange
            int paymentMethodId = 1;
            var expectedPaymentMethod = new PaymentMethod()
            {
                PaymentMethodId = 1,
                MethodName = "Valid Method"
            };

            paymentMethodRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<PaymentMethod, bool>>>()))
                .ReturnsAsync(new List<PaymentMethod> { expectedPaymentMethod });

            // Act
            var result = await service.GetById(paymentMethodId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPaymentMethod.PaymentMethodId, result.PaymentMethodId);
            Assert.Equal(expectedPaymentMethod.MethodName, result.MethodName);
        }

        [Fact]
        public async Task GetById_InvalidPaymentMethodId_ThrowsInvalidOperationException()
        {
            // Arrange
            int paymentMethodId = 999;

            // Act
            paymentMethodRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<PaymentMethod, bool>>>()))
                .ReturnsAsync(new List<PaymentMethod>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(paymentMethodId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidPaymentMethodId_DeletesPaymentMethodSuccessfully()
        {
            // Arrange
            int paymentMethodId = 1;
            var paymentMethod = new PaymentMethod()
            {
                PaymentMethodId = 1,
                MethodName = "Valid Method"
            };

            // Act
            paymentMethodRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<PaymentMethod, bool>>>()))
                .ReturnsAsync(new List<PaymentMethod> { paymentMethod });
            await service.Delete(paymentMethodId);

            // Assert
            paymentMethodRepositoryMoq.Verify(x => x.Delete(It.IsAny<PaymentMethod>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidPaymentMethodId_ThrowsInvalidOperationException()
        {
            // Arrange
            int paymentMethodId = 999;

            // Act
            paymentMethodRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<PaymentMethod, bool>>>()))
                .ReturnsAsync(new List<PaymentMethod>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(paymentMethodId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            paymentMethodRepositoryMoq.Verify(x => x.Delete(It.IsAny<PaymentMethod>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}