using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(supplier => supplier.SupplierId)
                .GreaterThan(0).WithMessage("SupplierId must be greater than 0.");

            RuleFor(supplier => supplier.SupplierName)
                .NotEmpty().WithMessage("SupplierName is required.")
                .MaximumLength(20).WithMessage("SupplierName must not exceed 20 characters.");

            RuleFor(supplier => supplier.ContactName)
                .MaximumLength(20).WithMessage("ContactName must not exceed 20 characters.");

            RuleFor(user => user.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Phone number is required.")
                    .MaximumLength(11)
                    .Matches(@"^[0-9]+$");

            RuleFor(supplier => supplier.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(50)
                .EmailAddress()
                .WithMessage("Email is not valid.");
        }
    }
}