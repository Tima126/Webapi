using Domain.Models;
using FluentValidation;

namespace BusinessLogic.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Address1)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z0-9\s\.,'-]+$")
                .WithMessage("Address contains invalid characters.");

            RuleFor(address => address.City)
                .NotEmpty().WithMessage("City is required.")
                .Matches(@"^[a-zA-Z]+$")
                .MaximumLength(20)
                .WithMessage("City contains invalid characters.");

            RuleFor(address => address.PostalCode)
                .NotEmpty().WithMessage("PostalCode is required.")
                .Matches(@"^\d{6}$")
                .MaximumLength(6)
                .WithMessage("PostalCode contains invalid characters.");

            RuleFor(address => address.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Matches(@"^[a-zA-Z]+$")
                .MaximumLength(10)
                .WithMessage("Country contains invalid characters.");
        }
    }
}