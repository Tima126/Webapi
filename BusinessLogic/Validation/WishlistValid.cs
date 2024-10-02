using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class WishlistValidator : AbstractValidator<Wishlist>
    {
        public WishlistValidator()
        {
            RuleFor(wishlist => wishlist.WishlistId)
                .GreaterThan(0).WithMessage("WishlistId must be greater than 0.");

            RuleFor(wishlist => wishlist.UserId)
                .NotNull().WithMessage("UserId cannot be null.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(wishlist => wishlist.ProductId)
                .NotNull().WithMessage("ProductId cannot be null.")
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
        }
    }
}