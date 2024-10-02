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
    public class SupplierServiceTest
    {
        private readonly SupplierService service;
        private readonly Mock<ISupplierRepository> supplierRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly SupplierValidator _validator;

        public SupplierServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            supplierRepositoryMoq = new Mock<ISupplierRepository>();

            repositoryWrapperMoq.Setup(x => x.Supplier)
                .Returns(supplierRepositoryMoq.Object);

            service = new SupplierService(repositoryWrapperMoq.Object);
            _validator = new SupplierValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectSupplier))]
        public async Task Create_InvalidSupplier_ThrowsValidationException(Supplier supplier)
        {
            // Arrange
            var supplierNew = supplier;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(supplierNew));

            // Assert
            supplierRepositoryMoq.Verify(x => x.Create(It.IsAny<Supplier>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectSupplier()
        {
            return new List<object[]>
            {
                new object[] {new Supplier() {SupplierId = 0, SupplierName = " ", ContactName = "Test", PhoneNumber = "1234567890", Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 0, SupplierName = "", ContactName = new string('a', 21), PhoneNumber = "1234567890", Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 1, SupplierName = new string('a', 21), ContactName = "Test", PhoneNumber = new string('1', 12), Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 1, SupplierName = "Valid Name", ContactName = "Test", PhoneNumber = "1234567890a", Email = "invalid-email"}},
            };
        }

        [Fact]
        public async Task Create_NullSupplier_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            supplierRepositoryMoq.Verify(x => x.Create(It.IsAny<Supplier>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidSupplier_UpdatesSupplierSuccessfully()
        {
            // Arrange
            var supplier = new Supplier()
            {
                SupplierId = 1,
                SupplierName = "Valid Name",
                ContactName = "Test",
                PhoneNumber = "1234567890",
                Email = "test@example.com"
            };

            // Act
            await service.Update(supplier);

            // Assert
            supplierRepositoryMoq.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullSupplier_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            supplierRepositoryMoq.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectSupplierUpdate))]
        public async Task Update_InvalidSupplier_ThrowsValidationException(Supplier supplier)
        {
            // Arrange
            var supplierNew = supplier;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(supplierNew));

            // Assert
            supplierRepositoryMoq.Verify(x => x.Update(It.IsAny<Supplier>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectSupplierUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Supplier() {SupplierId = 0, SupplierName = " ", ContactName = "Test", PhoneNumber = "1234567890", Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 0, SupplierName = "", ContactName = new string('a', 21), PhoneNumber = "1234567890", Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 1, SupplierName = new string('a', 21), ContactName = "Test", PhoneNumber = new string('1', 12), Email = "test@example.com"}},
                new object[] {new Supplier() {SupplierId = 1, SupplierName = "Valid Name", ContactName = "Test", PhoneNumber = "1234567890a", Email = "invalid-email"}},
            };
        }

        [Fact]
        public async Task GetById_ValidSupplierId_ReturnsSupplier()
        {
            // Arrange
            int supplierId = 1;
            var expectedSupplier = new Supplier()
            {
                SupplierId = 1,
                SupplierName = "Valid Name",
                ContactName = "Test",
                PhoneNumber = "1234567890",
                Email = "test@example.com"
            };

            supplierRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(new List<Supplier> { expectedSupplier });

            // Act
            var result = await service.GetById(supplierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSupplier.SupplierId, result.SupplierId);
            Assert.Equal(expectedSupplier.SupplierName, result.SupplierName);
            Assert.Equal(expectedSupplier.ContactName, result.ContactName);
            Assert.Equal(expectedSupplier.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedSupplier.Email, result.Email);
        }

        [Fact]
        public async Task GetById_InvalidSupplierId_ThrowsInvalidOperationException()
        {
            // Arrange
            int supplierId = 999;

            // Act
            supplierRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(new List<Supplier>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(supplierId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidSupplierId_DeletesSupplierSuccessfully()
        {
            // Arrange
            int supplierId = 1;
            var supplier = new Supplier()
            {
                SupplierId = 1,
                SupplierName = "Valid Name",
                ContactName = "Test",
                PhoneNumber = "1234567890",
                Email = "test@example.com"
            };

            // Act
            supplierRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(new List<Supplier> { supplier });
            await service.Delete(supplierId);

            // Assert
            supplierRepositoryMoq.Verify(x => x.Delete(It.IsAny<Supplier>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidSupplierId_ThrowsInvalidOperationException()
        {
            // Arrange
            int supplierId = 999;

            // Act
            supplierRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(new List<Supplier>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(supplierId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            supplierRepositoryMoq.Verify(x => x.Delete(It.IsAny<Supplier>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}