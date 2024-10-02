using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class OrderStatusValidator : AbstractValidator<OrderStatus>
    {
        public OrderStatusValidator()
        {
            RuleFor(orderStatus => orderStatus.StatusId)
                .GreaterThan(0).WithMessage("StatusId must be greater than 0.");

            RuleFor(orderStatus => orderStatus.StatusName)
                .NotEmpty().WithMessage("StatusName is required.")
                .MaximumLength(50).WithMessage("StatusName must not exceed 50 characters.");
        }
    }
}