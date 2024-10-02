using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(order => order.UserId)
                .NotNull().WithMessage("UserId cannot be null.")
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(order => order.OrderDate)
                .NotNull().WithMessage("OrderDate cannot be null.")
                .Must(BeAValidOrderDate).WithMessage("OrderDate must be a valid date in the past or present.");

            RuleFor(order => order.TotalAmount)
                .GreaterThan(0).WithMessage("TotalAmount must be greater than 0.");

            RuleFor(order => order.StatusId)
                .NotNull().WithMessage("StatusId cannot be null.")
                .GreaterThan(0).WithMessage("StatusId must be greater than 0.");

            RuleFor(order => order.DiscountId)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountId must be greater than or equal to 0.");
        }

        private bool BeAValidOrderDate(DateTime? orderDate)
        {
            return orderDate <= DateTime.Now;
        }
    }
}