using BusinessLogic.Services;
using BusinessLogic.Validation;
using Domain.interfaces.Repository;
using Domain.interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using System.Linq.Expressions;

namespace BusinessLogic.Test
{
	public class CategoryServiceTest
	{

		private readonly CategoryService service;
		private readonly Mock<ICategoryRepository> categoryRepositoryMoq;
		private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
		private readonly CategoryValid _validator;


		public CategoryServiceTest()
		{
			repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
			categoryRepositoryMoq = new Mock<ICategoryRepository>();

			repositoryWrapperMoq.Setup(x => x.Category)
				.Returns(categoryRepositoryMoq.Object);

			service = new CategoryService(repositoryWrapperMoq.Object);
			_validator = new CategoryValid();
		}


		[Fact]
		public async Task Create_NullCategory_ThrowsArgumentNullException()
		{
			// Arrange
			var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

			// Act
			categoryRepositoryMoq.Verify(x => x.Create(It.IsAny<Category>()), Times.Never);

			// Assert
			Assert.IsType<ArgumentNullException>(ex);
		}

		[Theory]
		[MemberData(nameof(GetIncorect_Create_Category))]
		public async Task Create_InvalidCategory_ThrowsValidationException(Category category)
		{
			// Arrange
			var Categorynew = category;

			// Act
			var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(Categorynew));

			// Assert
			categoryRepositoryMoq.Verify(x => x.Create(It.IsAny<Category>()), Times.Never);
			Assert.IsType<FluentValidation.ValidationException>(ex);
		}

		[Fact]
		public async Task Create_ValidCategory()
		{
			// Arrange
			var cat = new Category()
			{
				CategoryId=1,
				CategoryName ="Test",
			};
			// Act
			await service.Update(cat);
			// Assert
			categoryRepositoryMoq.Verify(x => x.Update(It.IsAny<Category>()), Times.Once);
			repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
		}

		[Theory]
		[MemberData(nameof(GetIncorect_Update_Category))]
		public async Task Update_InvalidCategory_ThrowsValidationException(Category category)
		{
			// Arrange
			var Categorynew = category;

			// Act
			var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(Categorynew));

			// Assert
			categoryRepositoryMoq.Verify(x => x.Create(It.IsAny<Category>()), Times.Never);
			Assert.IsType<FluentValidation.ValidationException>(ex);
		}

		public static IEnumerable<object[]> GetIncorect_Update_Category()
		{
			return new List<object[]>
			{
				new object[] {new Category() {CategoryId=0, CategoryName =" ",}},
				new object[] {new Category() {CategoryId=1, CategoryName ="_kklk  df444",}},
				new object[] {new Category() {CategoryId=0, CategoryName ="      ",}},
				new object[] {new Category() {CategoryId=0, CategoryName =" 23323kkjggg",}},
				new object[] {new Category() {CategoryId=1, CategoryName ="0-...,cdf ",}},
			};
		}

		public static IEnumerable<object[]> GetIncorect_Create_Category()
		{
			return new List<object[]>
			{
				new object[] {new Category() {CategoryId=0, CategoryName =" ",}},
				new object[] {new Category() {CategoryId=1, CategoryName ="_kklk  df444",}},
				new object[] {new Category() {CategoryId=0, CategoryName ="      ",}},
				new object[] {new Category() {CategoryId=0, CategoryName =" 23323kkjggg",}},
				new object[] {new Category() {CategoryId=1, CategoryName ="0-...,cdf ",}},
			};
		}


		[Fact]
		public async Task Update_ValidCategory()
		{
			// Arrange
			var cat = new Category()
			{
				CategoryId = 1,
				CategoryName = "Test",
			};
			// Act
			await service.Update(cat);
			// Assert
			categoryRepositoryMoq.Verify(x => x.Update(It.IsAny<Category>()), Times.Once);
			repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
		}

		[Fact]
		public async Task Update_NullCategory_ThrowsArgumentNullException()
		{
			// Arrange
			var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

			// Act
			categoryRepositoryMoq.Verify(x => x.Update(It.IsAny<Category>()), Times.Never);
			repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

			// Assert
			Assert.IsType<ArgumentNullException>(ex);
		}

		[Fact]
		public async Task GetById_ValidCategoryId_ReturnsUser()
		{
			// Arrange
			int categoryid = 1;
			var expectedCategory = new Category()
			{
				CategoryId= categoryid,
				CategoryName = "Test",
			};

			categoryRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Category, bool>>>()))
			 .ReturnsAsync(new List<Category> { expectedCategory });

			// Act
			var result = await service.GetById(categoryid);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expectedCategory.CategoryId, result.CategoryId);
			Assert.Equal(expectedCategory.CategoryName, result.CategoryName);

		}


		[Fact]
		public async Task GetById_InvaliCategoryId_ThrowsInvalidOperationException()
		{
			// Arrange
			int categoryId = 999;

			// Act
			categoryRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Category, bool>>>()))
				.ReturnsAsync(new List<Category>());

			var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(categoryId));

			// Assert
			Assert.IsType<InvalidOperationException>(ex);
		}

		[Fact]
		public async Task Delete_ValidCategoryId_Deletes()
		{
			// Arrange
			int categoryid = 1;
			var expectedCategory = new Category()
			{
				CategoryId = categoryid,
				CategoryName = "Test",
			};

			// Act
			categoryRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Category, bool>>>()))
			 .ReturnsAsync(new List<Category> { expectedCategory });
			await service.Delete(categoryid);

			// Assert
			categoryRepositoryMoq.Verify(x => x.Delete(It.IsAny<Category>()), Times.Once);
			repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);

		}


		[Fact]
		public async Task Delete_InvalidCategoryId_ThrowsInvalidOperationException()
		{
			// Arrange
			int addressId = 999;

			// Act
			categoryRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Category, bool>>>()))
				.ReturnsAsync(new List<Category>());

			var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(addressId));

			// Assert
			Assert.IsType<InvalidOperationException>(ex);
            categoryRepositoryMoq.Verify(x => x.Delete(It.IsAny<Category>()), Times.Never);
			repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
		}



	}
}
