using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class OrderDetailsValid : AbstractValidator<OrderDetail>
    {
        public OrderDetailsValid()
        {
            RuleFor(orderDetail => orderDetail.OrderDetailId)
                .GreaterThan(0).WithMessage("OrderDetailId must be greater than 0.");

            RuleFor(orderDetail => orderDetail.OrderId)
                .NotNull().WithMessage("OrderId cannot be null.")
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(orderDetail => orderDetail.ProductId)
                .NotNull().WithMessage("ProductId cannot be null.")
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(orderDetail => orderDetail.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(orderDetail => orderDetail.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}