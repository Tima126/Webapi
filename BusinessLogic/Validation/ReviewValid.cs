using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(review => review.ReviewId)
                .GreaterThan(0).WithMessage("ReviewId must be greater than 0.");

            RuleFor(review => review.ProductId)
                .NotNull().WithMessage("ProductId cannot be null.")
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(review => review.UserId)
                .NotNull().WithMessage("UserId cannot be null.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(review => review.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(review => review.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");

            RuleFor(review => review.ReviewDate)
                .NotNull().WithMessage("ReviewDate cannot be null.")
                .Must(BeAValidReviewDate).WithMessage("ReviewDate must be a valid date in the past or present.");
        }

        private bool BeAValidReviewDate(DateTime? reviewDate)
        {
            return reviewDate <= DateTime.Now;
        }
    }
}