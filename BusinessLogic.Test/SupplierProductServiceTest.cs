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
    public class SupplierProductServiceTest
    {
        private readonly SupplierProductService service;
        private readonly Mock<ISupplierProductRepository> supplierProductRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly SupplierProductValidator _validator;

        public SupplierProductServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            supplierProductRepositoryMoq = new Mock<ISupplierProductRepository>();

            repositoryWrapperMoq.Setup(x => x.SupplierProduct)
                .Returns(supplierProductRepositoryMoq.Object);

            service = new SupplierProductService(repositoryWrapperMoq.Object);
            _validator = new SupplierProductValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectSupplierProduct))]
        public async Task Create_InvalidSupplierProduct_ThrowsValidationException(SupplierProduct supplierProduct)
        {
            // Arrange
            var supplierProductNew = supplierProduct;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(supplierProductNew));

            // Assert
            supplierProductRepositoryMoq.Verify(x => x.Create(It.IsAny<SupplierProduct>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectSupplierProduct()
        {
            return new List<object[]>
            {
                new object[] {new SupplierProduct() {SupplierProductId = 0, SupplierId = null, ProductId = null}},
                new object[] {new SupplierProduct() {SupplierProductId = 0, SupplierId = 0, ProductId = 0}},
                new object[] {new SupplierProduct() {SupplierProductId = 1, SupplierId = 1, ProductId = 0}},
                new object[] {new SupplierProduct() {SupplierProductId = 1, SupplierId = 0, ProductId = 1}},
            };
        }

        [Fact]
        public async Task Create_NullSupplierProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            supplierProductRepositoryMoq.Verify(x => x.Create(It.IsAny<SupplierProduct>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidSupplierProduct_UpdatesSupplierProductSuccessfully()
        {
            // Arrange
            var supplierProduct = new SupplierProduct()
            {
                SupplierProductId = 1,
                SupplierId = 1,
                ProductId = 1
            };

            // Act
            await service.Update(supplierProduct);

            // Assert
            supplierProductRepositoryMoq.Verify(x => x.Update(It.IsAny<SupplierProduct>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullSupplierProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            supplierProductRepositoryMoq.Verify(x => x.Update(It.IsAny<SupplierProduct>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectSupplierProductUpdate))]
        public async Task Update_InvalidSupplierProduct_ThrowsValidationException(SupplierProduct supplierProduct)
        {
            // Arrange
            var supplierProductNew = supplierProduct;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(supplierProductNew));

            // Assert
            supplierProductRepositoryMoq.Verify(x => x.Update(It.IsAny<SupplierProduct>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectSupplierProductUpdate()
        {
            return new List<object[]>
            {
                new object[] {new SupplierProduct() {SupplierProductId = 0, SupplierId = null, ProductId = null}},
                new object[] {new SupplierProduct() {SupplierProductId = 0, SupplierId = 0, ProductId = 0}},
                new object[] {new SupplierProduct() {SupplierProductId = 1, SupplierId = 1, ProductId = 0}},
                new object[] {new SupplierProduct() {SupplierProductId = 1, SupplierId = 0, ProductId = 1}},
            };
        }

        [Fact]
        public async Task GetById_ValidSupplierProductId_ReturnsSupplierProduct()
        {
            // Arrange
            int supplierProductId = 1;
            var expectedSupplierProduct = new SupplierProduct()
            {
                SupplierProductId = 1,
                SupplierId = 1,
                ProductId = 1
            };

            supplierProductRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<SupplierProduct, bool>>>()))
                .ReturnsAsync(new List<SupplierProduct> { expectedSupplierProduct });

            // Act
            var result = await service.GetById(supplierProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSupplierProduct.SupplierProductId, result.SupplierProductId);
            Assert.Equal(expectedSupplierProduct.SupplierId, result.SupplierId);
            Assert.Equal(expectedSupplierProduct.ProductId, result.ProductId);
        }

        [Fact]
        public async Task GetById_InvalidSupplierProductId_ThrowsInvalidOperationException()
        {
            // Arrange
            int supplierProductId = 999;

            // Act
            supplierProductRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<SupplierProduct, bool>>>()))
                .ReturnsAsync(new List<SupplierProduct>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(supplierProductId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidSupplierProductId_DeletesSupplierProductSuccessfully()
        {
            // Arrange
            int supplierProductId = 1;
            var supplierProduct = new SupplierProduct()
            {
                SupplierProductId = 1,
                SupplierId = 1,
                ProductId = 1
            };

            // Act
            supplierProductRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<SupplierProduct, bool>>>()))
                .ReturnsAsync(new List<SupplierProduct> { supplierProduct });
            await service.Delete(supplierProductId);

            // Assert
            supplierProductRepositoryMoq.Verify(x => x.Delete(It.IsAny<SupplierProduct>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidSupplierProductId_ThrowsInvalidOperationException()
        {
            // Arrange
            int supplierProductId = 999;

            // Act
            supplierProductRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<SupplierProduct, bool>>>()))
                .ReturnsAsync(new List<SupplierProduct>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(supplierProductId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            supplierProductRepositoryMoq.Verify(x => x.Delete(It.IsAny<SupplierProduct>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}