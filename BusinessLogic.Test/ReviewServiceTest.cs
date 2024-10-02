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
    public class ReviewServiceTest
    {
        private readonly ReviewService service;
        private readonly Mock<IReviewRepository> reviewRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly ReviewValidator _validator;

        public ReviewServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            reviewRepositoryMoq = new Mock<IReviewRepository>();

            repositoryWrapperMoq.Setup(x => x.Review)
                .Returns(reviewRepositoryMoq.Object);

            service = new ReviewService(repositoryWrapperMoq.Object);
            _validator = new ReviewValidator();
        }

        [Theory]
        [MemberData(nameof(GetIncorectReview))]
        public async Task Create_InvalidReview_ThrowsValidationException(Review review)
        {
            // Arrange
            var reviewNew = review;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(reviewNew));

            // Assert
            reviewRepositoryMoq.Verify(x => x.Create(It.IsAny<Review>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectReview()
        {
            return new List<object[]>
            {
                new object[] {new Review() {ReviewId = 0, ProductId = null, UserId = null, Rating = 0, Comment = "Test", ReviewDate = null}},
                new object[] {new Review() {ReviewId = 0, ProductId = 0, UserId = 0, Rating = 6, Comment = new string('a', 501), ReviewDate = DateTime.Now.AddDays(1)}},
                new object[] {new Review() {ReviewId = 1, ProductId = 1, UserId = 1, Rating = 3, Comment = "Test", ReviewDate = DateTime.Now.AddDays(1)}},
                new object[] {new Review() {ReviewId = 1, ProductId = 1, UserId = 1, Rating = 3, Comment = "Test", ReviewDate = null}},
            };
        }

        [Fact]
        public async Task Create_NullReview_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            reviewRepositoryMoq.Verify(x => x.Create(It.IsAny<Review>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task Update_ValidReview_UpdatesReviewSuccessfully()
        {
            // Arrange
            var review = new Review()
            {
                ReviewId = 1,
                ProductId = 1,
                UserId = 1,
                Rating = 3,
                Comment = "Test",
                ReviewDate = DateTime.Now.AddDays(-1)
            };

            // Act
            await service.Update(review);

            // Assert
            reviewRepositoryMoq.Verify(x => x.Update(It.IsAny<Review>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullReview_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            reviewRepositoryMoq.Verify(x => x.Update(It.IsAny<Review>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(GetIncorectReviewUpdate))]
        public async Task Update_InvalidReview_ThrowsValidationException(Review review)
        {
            // Arrange
            var reviewNew = review;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(reviewNew));

            // Assert
            reviewRepositoryMoq.Verify(x => x.Update(It.IsAny<Review>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectReviewUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Review() {ReviewId = 0, ProductId = null, UserId = null, Rating = 0, Comment = "Test", ReviewDate = null}},
                new object[] {new Review() {ReviewId = 0, ProductId = 0, UserId = 0, Rating = 6, Comment = new string('a', 501), ReviewDate = DateTime.Now.AddDays(1)}},
                new object[] {new Review() {ReviewId = 1, ProductId = 1, UserId = 1, Rating = 3, Comment = "Test", ReviewDate = DateTime.Now.AddDays(1)}},
                new object[] {new Review() {ReviewId = 1, ProductId = 1, UserId = 1, Rating = 3, Comment = "Test", ReviewDate = null}},
            };
        }

        [Fact]
        public async Task GetById_ValidReviewId_ReturnsReview()
        {
            // Arrange
            int reviewId = 1;
            var expectedReview = new Review()
            {
                ReviewId = 1,
                ProductId = 1,
                UserId = 1,
                Rating = 3,
                Comment = "Test",
                ReviewDate = DateTime.Now.AddDays(-1)
            };

            reviewRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync(new List<Review> { expectedReview });

            // Act
            var result = await service.GetById(reviewId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReview.ReviewId, result.ReviewId);
            Assert.Equal(expectedReview.ProductId, result.ProductId);
            Assert.Equal(expectedReview.UserId, result.UserId);
            Assert.Equal(expectedReview.Rating, result.Rating);
            Assert.Equal(expectedReview.Comment, result.Comment);
            Assert.Equal(expectedReview.ReviewDate, result.ReviewDate);
        }

        [Fact]
        public async Task GetById_InvalidReviewId_ThrowsInvalidOperationException()
        {
            // Arrange
            int reviewId = 999;

            // Act
            reviewRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync(new List<Review>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(reviewId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public async Task Delete_ValidReviewId_DeletesReviewSuccessfully()
        {
            // Arrange
            int reviewId = 1;
            var review = new Review()
            {
                ReviewId = 1,
                ProductId = 1,
                UserId = 1,
                Rating = 3,
                Comment = "Test",
                ReviewDate = DateTime.Now.AddDays(-1)
            };

            // Act
            reviewRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync(new List<Review> { review });
            await service.Delete(reviewId);

            // Assert
            reviewRepositoryMoq.Verify(x => x.Delete(It.IsAny<Review>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_InvalidReviewId_ThrowsInvalidOperationException()
        {
            // Arrange
            int reviewId = 999;

            // Act
            reviewRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync(new List<Review>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(reviewId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            reviewRepositoryMoq.Verify(x => x.Delete(It.IsAny<Review>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }
    }
}