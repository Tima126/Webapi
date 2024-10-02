using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class NotificationTypeValid: AbstractValidator<NotificationType>
    {


        public NotificationTypeValid()
        {
            RuleFor(notificationtype => notificationtype.NotificationTypeId)
                .GreaterThan(0).WithMessage("NotificationType Id must be greater than 0.");
            RuleFor(notificationtype => notificationtype.TypeName)
                .NotEmpty().WithMessage("Message is required.")
                 .MaximumLength(20).WithMessage("Message must not exceed 500 characters.");

        }

    }
}
