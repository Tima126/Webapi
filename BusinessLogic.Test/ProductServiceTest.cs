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
    public class ProductServiceTest
    {
        private readonly ProductService service;
        private readonly Mock<IProductRepository> productRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly ProductValidator _validator;

        public ProductServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            productRepositoryMoq = new Mock<IProductRepository>();

            repositoryWrapperMoq.Setup(x => x.Product)
                .Returns(productRepositoryMoq.Object);

            service = new ProductService(repositoryWrapperMoq.Object);
            _validator = new ProductValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectProduct))]
        public async Task Create_InvalidProduct_ThrowsValidationException(Product product)
        {
            // Arrange
            var productNew = product;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(productNew));

            // Assert
            productRepositoryMoq.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectProduct()
        {
            return new List<object[]>
            {
                new object[] {new Product() {ProductId = 0, ProductName = " ", Description = "Test", Price = 0, StockQuantity = -1, CategoryId = 0, DiscountId = -1}},
                new object[] {new Product() {ProductId = 0, ProductName = "", Description = new string('a', 501), Price = 0, StockQuantity = 0, CategoryId = null, DiscountId = 0}},
                new object[] {new Product() {ProductId = 1, ProductName = new string('a', 101), Description = "Test", Price = -1, StockQuantity = 1, CategoryId = 1, DiscountId = 1}},
                new object[] {new Product() {ProductId = 1, ProductName = "Valid Name", Description = "Test", Price = 100, StockQuantity = 1, CategoryId = 0, DiscountId = 1}},
            };
        }

        [Fact]
        public async Task Create_NullProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            productRepositoryMoq.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidProduct_UpdatesProductSuccessfully()
        {
            // Arrange
            var product = new Product()
            {
                ProductId = 1,
                ProductName = "Valid Name",
                Description = "Test",
                Price = 100,
                StockQuantity = 1,
                CategoryId = 1,
                DiscountId = 1
            };

            // Act
            await service.Update(product);

            // Assert
            productRepositoryMoq.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullProduct_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            productRepositoryMoq.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectProductUpdate))]
        public async Task Update_InvalidProduct_ThrowsValidationException(Product product)
        {
            // Arrange
            var productNew = product;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(productNew));

            // Assert
            productRepositoryMoq.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectProductUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Product() {ProductId = 0, ProductName = " ", Description = "Test", Price = 0, StockQuantity = -1, CategoryId = 0, DiscountId = -1}},
                new object[] {new Product() {ProductId = 0, ProductName = "", Description = new string('a', 501), Price = 0, StockQuantity = 0, CategoryId = null, DiscountId = 0}},
                new object[] {new Product() {ProductId = 1, ProductName = new string('a', 101), Description = "Test", Price = -1, StockQuantity = 1, CategoryId = 1, DiscountId = 1}},
                new object[] {new Product() {ProductId = 1, ProductName = "Valid Name", Description = "Test", Price = 100, StockQuantity = 1, CategoryId = 0, DiscountId = 1}},
            };
        }

        [Fact]
        public async Task GetById_ValidProductId_ReturnsProduct()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new Product()
            {
                ProductId = 1,
                ProductName = "Valid Name",
                Description = "Test",
                Price = 100,
                StockQuantity = 1,
                CategoryId = 1,
                DiscountId = 1
            };

            productRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product> { expectedProduct });

            // Act
            var result = await service.GetById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.ProductId, result.ProductId);
            Assert.Equal(expectedProduct.ProductName, result.ProductName);
            Assert.Equal(expectedProduct.Description, result.Description);
            Assert.Equal(expectedProduct.Price, result.Price);
            Assert.Equal(expectedProduct.StockQuantity, result.StockQuantity);
            Assert.Equal(expectedProduct.CategoryId, result.CategoryId);
            Assert.Equal(expectedProduct.DiscountId, result.DiscountId);
        }

        [Fact]
        public async Task GetById_InvalidProductId_ThrowsInvalidOperationException()
        {
            // Arrange
            int productId = 999;

            // Act
            productRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(productId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidProductId_DeletesProductSuccessfully()
        {
            // Arrange
            int productId = 1;
            var product = new Product()
            {
                ProductId = 1,
                ProductName = "Valid Name",
                Description = "Test",
                Price = 100,
                StockQuantity = 1,
                CategoryId = 1,
                DiscountId = 1
            };

            // Act
            productRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product> { product });
            await service.Delete(productId);

            // Assert
            productRepositoryMoq.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidProductId_ThrowsInvalidOperationException()
        {
            // Arrange
            int productId = 999;

            // Act
            productRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(new List<Product>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(productId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            productRepositoryMoq.Verify(x => x.Delete(It.IsAny<Product>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}