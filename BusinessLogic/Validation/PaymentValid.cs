using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(payment => payment.PaymentId)
                .GreaterThan(0).WithMessage("PaymentId must be greater than 0.");

            RuleFor(payment => payment.OrderId)
                .NotNull().WithMessage("OrderId cannot be null.")
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(payment => payment.PaymentDate)
                .NotNull().WithMessage("PaymentDate cannot be null.")
                .Must(BeAValidPaymentDate).WithMessage("PaymentDate must be a valid date in the past or present.");

            RuleFor(payment => payment.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(payment => payment.PaymentMethodId)
                .NotNull().WithMessage("PaymentMethodId cannot be null.")
                .GreaterThan(0).WithMessage("PaymentMethodId must be greater than 0.");
        }

        private bool BeAValidPaymentDate(DateTime? paymentDate)
        {
            return paymentDate <= DateTime.Now;
        }
    }
}