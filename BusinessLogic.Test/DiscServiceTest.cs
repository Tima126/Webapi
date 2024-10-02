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
    public class DiscountServiceTest
    {
        private readonly DiscountService service;
        private readonly Mock<IDiscountRepository> discountRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly DiscountValid _validator;

        public DiscountServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            discountRepositoryMoq = new Mock<IDiscountRepository>();

            repositoryWrapperMoq.Setup(x => x.Discount)
                .Returns(discountRepositoryMoq.Object);

            service = new DiscountService(repositoryWrapperMoq.Object);
            _validator = new DiscountValid();
        }

        [Theory]
        [MemberData(nameof(GetIncorectDiscount))]
        public async Task Create_InvalidDiscount_ThrowsValidationException(Discount discount)
        {
            // Arrange
            var discountNew = discount;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(discountNew));

            // Assert
            discountRepositoryMoq.Verify(x => x.Create(It.IsAny<Discount>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectDiscount()
        {
            return new List<object[]>
            {
                new object[] {new Discount() {DiscountId = 0, Code = " ", DiscountPercentage = -1, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 0, Code = "", DiscountPercentage = 101, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 1, Code = "333", DiscountPercentage = 50, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 1, Code = "1111", DiscountPercentage = 50, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
            };
        }

        [Fact]
        public async Task Create_NullDiscount_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            discountRepositoryMoq.Verify(x => x.Create(It.IsAny<Discount>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidDiscount_UpdatesDiscountSuccessfully()
        {
            // Arrange
            var discount = new Discount()
            {
                Code = "123",
                DiscountPercentage = 10,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            // Act
            await service.Update(discount);

            // Assert
            discountRepositoryMoq.Verify(x => x.Update(It.IsAny<Discount>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullDiscount_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            discountRepositoryMoq.Verify(x => x.Update(It.IsAny<Discount>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectDiscountUpdate))]
        public async Task Update_InvalidDiscount_ThrowsValidationException(Discount discount)
        {
            // Arrange
            var discountNew = discount;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(discountNew));

            // Assert
            discountRepositoryMoq.Verify(x => x.Update(It.IsAny<Discount>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectDiscountUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Discount() {DiscountId = 0, Code = " ", DiscountPercentage = -1, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 0, Code = "", DiscountPercentage = 101, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 1, Code = "333", DiscountPercentage = 50, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
                new object[] {new Discount() {DiscountId = 1, Code = "1111", DiscountPercentage = 50, ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))}},
            };
        }

        [Fact]
        public async Task GetById_ValidDiscountId_ReturnsDiscount()
        {
            // Arrange
            int discountId = 1;
            var expectedDiscount = new Discount()
            {
                DiscountId = 1,
                Code = "123",
                DiscountPercentage = 10,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            discountRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Discount, bool>>>()))
                .ReturnsAsync(new List<Discount> { expectedDiscount });

            // Act
            var result = await service.GetById(discountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDiscount.DiscountId, result.DiscountId);
            Assert.Equal(expectedDiscount.Code, result.Code);
            Assert.Equal(expectedDiscount.DiscountPercentage, result.DiscountPercentage);
            Assert.Equal(expectedDiscount.ExpiryDate, result.ExpiryDate);
        }

        [Fact]
        public async Task GetById_InvalidDiscountId_ThrowsInvalidOperationException()
        {
            // Arrange
            int discountId = 999;

            // Act
            discountRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Discount, bool>>>()))
                .ReturnsAsync(new List<Discount>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(discountId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidDiscountId_DeletesDiscountSuccessfully()
        {
            // Arrange
            int discountId = 1;
            var discount = new Discount()
            {
                DiscountId = 1,
                Code = "123",
                DiscountPercentage = 10,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            // Act
            discountRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Discount, bool>>>()))
                .ReturnsAsync(new List<Discount> { discount });
            await service.Delete(discountId);

            // Assert
            discountRepositoryMoq.Verify(x => x.Delete(It.IsAny<Discount>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidDiscountId_ThrowsInvalidOperationException()
        {
            // Arrange
            int discountId = 999;

            // Act
            discountRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Discount, bool>>>()))
                .ReturnsAsync(new List<Discount>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(discountId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            discountRepositoryMoq.Verify(x => x.Delete(It.IsAny<Discount>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}