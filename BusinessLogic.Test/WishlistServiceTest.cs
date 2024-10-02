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
    public class WishlistServiceTest
    {
        private readonly WishlistService service;
        private readonly Mock<IWishlistRepository> wishlistRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly WishlistValidator _validator;

        public WishlistServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            wishlistRepositoryMoq = new Mock<IWishlistRepository>();

            repositoryWrapperMoq.Setup(x => x.Wishlist)
                .Returns(wishlistRepositoryMoq.Object);

            service = new WishlistService(repositoryWrapperMoq.Object);
            _validator = new WishlistValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectWishlist))]
        public async Task Create_InvalidWishlist_ThrowsValidationException(Wishlist wishlist)
        {
            // Arrange
            var wishlistNew = wishlist;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(wishlistNew));

            // Assert
            wishlistRepositoryMoq.Verify(x => x.Create(It.IsAny<Wishlist>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectWishlist()
        {
            return new List<object[]>
            {
                new object[] {new Wishlist() {WishlistId = 0, UserId = null, ProductId = null}},
                new object[] {new Wishlist() {WishlistId = 0, UserId = 0, ProductId = 0}},
                new object[] {new Wishlist() {WishlistId = 1, UserId = 1, ProductId = 0}},
                new object[] {new Wishlist() {WishlistId = 1, UserId = 0, ProductId = 1}},
            };
        }

        [Fact]
        public async Task Create_NullWishlist_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            wishlistRepositoryMoq.Verify(x => x.Create(It.IsAny<Wishlist>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidWishlist_UpdatesWishlistSuccessfully()
        {
            // Arrange
            var wishlist = new Wishlist()
            {
                WishlistId = 1,
                UserId = 1,
                ProductId = 1
            };

            // Act
            await service.Update(wishlist);

            // Assert
            wishlistRepositoryMoq.Verify(x => x.Update(It.IsAny<Wishlist>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullWishlist_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            wishlistRepositoryMoq.Verify(x => x.Update(It.IsAny<Wishlist>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectWishlistUpdate))]
        public async Task Update_InvalidWishlist_ThrowsValidationException(Wishlist wishlist)
        {
            // Arrange
            var wishlistNew = wishlist;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(wishlistNew));

            // Assert
            wishlistRepositoryMoq.Verify(x => x.Update(It.IsAny<Wishlist>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectWishlistUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Wishlist() {WishlistId = 0, UserId = null, ProductId = null}},
                new object[] {new Wishlist() {WishlistId = 0, UserId = 0, ProductId = 0}},
                new object[] {new Wishlist() {WishlistId = 1, UserId = 1, ProductId = 0}},
                new object[] {new Wishlist() {WishlistId = 1, UserId = 0, ProductId = 1}},
            };
        }

        [Fact]
        public async Task GetById_ValidWishlistId_ReturnsWishlist()
        {
            // Arrange
            int wishlistId = 1;
            var expectedWishlist = new Wishlist()
            {
                WishlistId = 1,
                UserId = 1,
                ProductId = 1
            };

            wishlistRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                .ReturnsAsync(new List<Wishlist> { expectedWishlist });

            // Act
            var result = await service.GetById(wishlistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWishlist.WishlistId, result.WishlistId);
            Assert.Equal(expectedWishlist.UserId, result.UserId);
            Assert.Equal(expectedWishlist.ProductId, result.ProductId);
        }

        [Fact]
        public async Task GetById_InvalidWishlistId_ThrowsInvalidOperationException()
        {
            // Arrange
            int wishlistId = 999;

            // Act
            wishlistRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                .ReturnsAsync(new List<Wishlist>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(wishlistId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidWishlistId_DeletesWishlistSuccessfully()
        {
            // Arrange
            int wishlistId = 1;
            var wishlist = new Wishlist()
            {
                WishlistId = 1,
                UserId = 1,
                ProductId = 1
            };

            // Act
            wishlistRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                .ReturnsAsync(new List<Wishlist> { wishlist });
            await service.Delete(wishlistId);

            // Assert
            wishlistRepositoryMoq.Verify(x => x.Delete(It.IsAny<Wishlist>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidWishlistId_ThrowsInvalidOperationException()
        {
            // Arrange
            int wishlistId = 999;

            // Act
            wishlistRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Wishlist, bool>>>()))
                .ReturnsAsync(new List<Wishlist>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(wishlistId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            wishlistRepositoryMoq.Verify(x => x.Delete(It.IsAny<Wishlist>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}