
namespace webApi.Contracts
{
    public class CreateUserRequest
    {

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public int AddressId { get; set; }
    }
}
