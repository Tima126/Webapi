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
	public class AddressServiceTest
	{
		private readonly AddressService service;
		private readonly Mock<IAddressRepository> addressRepositoryMoq;
		private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
		private readonly AddressValidator _validator;

		public AddressServiceTest()
		{
			repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
			addressRepositoryMoq = new Mock<IAddressRepository>();

			repositoryWrapperMoq.Setup(x => x.Address)
				.Returns(addressRepositoryMoq.Object);

			service = new AddressService(repositoryWrapperMoq.Object);
			_validator = new AddressValidator();
		}


		[Theory]
		[MemberData(nameof(GetIncorectAddress))]
		public async Task Create_InvalidAddress_ThrowsValidationException(Address address)
		{
			// Arrange
			var Addressnew = address;

			// Act
			var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Create(Addressnew));

			// Assert
			addressRepositoryMoq.Verify(x => x.Create(It.IsAny<Address>()), Times.Never);
			Assert.IsType<FluentValidation.ValidationException>(ex);
		}
			
		public static IEnumerable<object[]> GetIncorectAddress()
		{
			return new List<object[]>
			{
				new object[] {new Address() {AddressId=0, Address1 =" ", City=" лжэ0000", PostalCode=" ", Country=" "}},
				new object[] {new Address() {AddressId=0, Address1 ="", City="", PostalCode="", Country=""}},
				new object[] {new Address() {AddressId=1, Address1 ="333", City="", PostalCode="er45", Country="3331"}},
				new object[] {new Address() {AddressId=1, Address1 ="1111", City="", PostalCode="", Country="ghghghghыапыапыапыапghghghghghgh"}},
			};
		}

        [Fact]
        public async Task Create_NullAddress_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(null));

            // Act
            addressRepositoryMoq.Verify(x => x.Create(It.IsAny<Address>()), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }


        [Fact]
        public async Task Update_ValidAddress_UpdatesUserSuccessfully()
        {
            // Arrange
            var address = new Address()
            {
                Address1 = "gg",
                City = "gg",
                PostalCode = "123456",
                Country= "jo",
            };
            // Act
            await service.Update(address);
            // Assert
            addressRepositoryMoq.Verify(x => x.Update(It.IsAny<Address>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_NullAddress_ThrowsArgumentNullException()
        {
            // Arrange
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update(null));

            // Act
            addressRepositoryMoq.Verify(x => x.Update(It.IsAny<Address>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);

            // Assert
            Assert.IsType<ArgumentNullException>(ex);
        }



        [Theory]
        [MemberData(nameof(GetIncorectAddressUpdate))]
        public async Task Update_InvalidAddress_ThrowsValidationException(Address address)
        {
            // Arrange
            var Addressnew = address;

            // Act
            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => service.Update(Addressnew));

            // Assert
            addressRepositoryMoq.Verify(x => x.Update(It.IsAny<Address>()), Times.Never);
            Assert.IsType<FluentValidation.ValidationException>(ex);
        }

        public static IEnumerable<object[]> GetIncorectAddressUpdate()
        {
            return new List<object[]>
            {
                new object[] {new Address() {AddressId=0, Address1 =" ", City=" лжэ0000", PostalCode=" ", Country=" "}},
                new object[] {new Address() {AddressId=0, Address1 ="", City="", PostalCode="", Country=""}},
                new object[] {new Address() {AddressId=1, Address1 ="333", City="", PostalCode="er45", Country="3331"}},
                new object[] {new Address() {AddressId=1, Address1 ="1111", City="", PostalCode="111111", Country="ghghghghыапыапыапыапghghghghghgh"}},
            };
        }

        [Fact]
        public async Task GetById_ValidAddressId_ReturnsUser()
        {
            // Arrange
            int addressId = 1;
            var expectedUser = new Address()
            {
                AddressId=0,
                Address1 = "test",
                City = "test",
                PostalCode = "123456",
                Country = "test",
            };

            addressRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Address, bool>>>()))
             .ReturnsAsync(new List<Address> { expectedUser });

            // Act
            var result = await service.GetById(addressId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.AddressId, result.AddressId);
            Assert.Equal(expectedUser.Address1, result.Address1);
            Assert.Equal(expectedUser.City, result.City);
            Assert.Equal(expectedUser.PostalCode, result.PostalCode);
            Assert.Equal(expectedUser.Country, result.Country);
        }


        [Fact]
        public async Task GetById_InvalidAddressId_ThrowsInvalidOperationException()
        {
            // Arrange
            int addressId = 999;

            // Act
            addressRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Address, bool>>>()))
                .ReturnsAsync(new List<Address>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(addressId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
        }



        [Fact]
        public async Task Delete_ValidAddressId_DeletesUserSuccessfully()
        {
            // Arrange
            int addressId = 1;
            var address = new Address()
            {
                AddressId = 0,
                Address1 = "test",
                City = "test",
                PostalCode = "123456",
                Country = "test",
            };

            // Act
            addressRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Address, bool>>>()))
                .ReturnsAsync(new List<Address> { address });
            await service.Delete(addressId);

            // Assert
            addressRepositoryMoq.Verify(x => x.Delete(It.IsAny<Address>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }


        [Fact]
        public async Task Delete_InvalidAddressId_ThrowsInvalidOperationException()
        {
            // Arrange
            int addressId = 999;

            // Act
            addressRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Address, bool>>>()))
                .ReturnsAsync(new List<Address>());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Delete(addressId));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            addressRepositoryMoq.Verify(x => x.Delete(It.IsAny<Address>()), Times.Never);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
        }




    }
}