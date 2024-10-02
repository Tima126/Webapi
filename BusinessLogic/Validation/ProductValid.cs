using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(product => product.ProductName)
                .NotEmpty().WithMessage("ProductName is required.")
                .MaximumLength(100).WithMessage("ProductName must not exceed 100 characters.");

            RuleFor(product => product.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(product => product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(product => product.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to 0.");

            RuleFor(product => product.CategoryId)
                .NotNull().WithMessage("CategoryId cannot be null.")
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

            RuleFor(product => product.DiscountId)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountId must be greater than or equal to 0.");
        }
    }
}