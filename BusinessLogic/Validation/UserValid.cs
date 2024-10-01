using Domain.Models;
using FluentValidation;


public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {


        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(20)
            .Matches(@"^[a-zA-Z]+$")
            .WithMessage("First name must contain only letters and no spaces.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches(@"^[a-zA-Z]+$")
            .MaximumLength(20)
            .WithMessage("Last name must contain only letters and no spaces.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(50)
            .EmailAddress()
            .WithMessage("Email is not valid.");

        RuleFor(user => user.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .MaximumLength(11)
            .Matches(@"^[0-9]+$");

        RuleFor(user => user.AddressId)
            .GreaterThan(0).WithMessage("Address ID must be greater than 0.");
    }
}