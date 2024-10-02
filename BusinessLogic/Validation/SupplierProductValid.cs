using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class SupplierProductValidator : AbstractValidator<SupplierProduct>
    {
        public SupplierProductValidator()
        {
            RuleFor(supplierProduct => supplierProduct.SupplierProductId)
                .GreaterThan(0).WithMessage("SupplierProductId must be greater than 0.");

            RuleFor(supplierProduct => supplierProduct.SupplierId)
                .NotNull().WithMessage("SupplierId cannot be null.")
                .GreaterThan(0).WithMessage("SupplierId must be greater than 0.");

            RuleFor(supplierProduct => supplierProduct.ProductId)
                .NotNull().WithMessage("ProductId cannot be null.")
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
        }
    }
}