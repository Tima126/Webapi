using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class NotificationValid : AbstractValidator<Notification>
    {

        public NotificationValid()
        {
            RuleFor(notification => notification.NotificationId)
                .GreaterThan(0).WithMessage("NotificationId must be greater than 0.");

            RuleFor(notification => notification.UserId)
                .NotNull().WithMessage("UserId cannot be null.");

            RuleFor(notification => notification.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(500).WithMessage("Message must not exceed 500 characters.");

            RuleFor(notification => notification.SentDate)
                .NotNull().WithMessage("SentDate cannot be null.")
                .Must(BeAValidSentDate).WithMessage("SentDate must be a valid date in the past or present.");
        }

        private bool BeAValidSentDate(DateTime? sentDate)
        {
            return sentDate <= DateTime.Now;
        }
    }



}

