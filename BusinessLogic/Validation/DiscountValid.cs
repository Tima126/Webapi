using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class DiscountValid : AbstractValidator<Discount>
    {
        public DiscountValid()
        {
            RuleFor(disc => disc.Code)
                .NotEmpty().WithMessage("CodeDiscount is required.")
                .MaximumLength(10)
                .Matches(@"^[0-9]+$")
                .WithMessage("CodeDiscount contains invalid characters.");

            RuleFor(disc => disc.DiscountPercentage)
                .NotNull().WithMessage("Discount percentage cannot be null.")
                .GreaterThanOrEqualTo(0).WithMessage("Discount percentage must be greater than or equal to 0.")
                .LessThanOrEqualTo(100).WithMessage("Discount percentage must be less than or equal to 100.");

            RuleFor(disc => disc.ExpiryDate)
                .NotNull().WithMessage("Expiry date cannot be null.")
                .Must(BeAValidExpiryDate).WithMessage("Expiry date must be in the future.");
        }

        private bool BeAValidExpiryDate(DateOnly expiryDate)
        {
            return expiryDate > DateOnly.FromDateTime(DateTime.Today);
        }
    }
}