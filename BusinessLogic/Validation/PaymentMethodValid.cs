using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class PaymentMethodValidator : AbstractValidator<PaymentMethod>
    {
        public PaymentMethodValidator()
        {
            RuleFor(paymentMethod => paymentMethod.PaymentMethodId)
                .GreaterThan(0).WithMessage("PaymentMethodId must be greater than 0.");

            RuleFor(paymentMethod => paymentMethod.MethodName)
                .NotEmpty().WithMessage("MethodName is required.")
                .MaximumLength(50).WithMessage("MethodName must not exceed 50 characters.");
        }
    }
}