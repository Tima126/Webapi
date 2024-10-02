using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation
{
    public class CategoryValid :AbstractValidator<Category>
    {

        public CategoryValid()
        {
            RuleFor(cat => cat.CategoryName)
               .NotEmpty().WithMessage("CategoryName is required.")
               .MaximumLength(20)
               .Matches(@"^[a-zA-Z]+$")
               .WithMessage("CategoryName contains invalid characters.");


            RuleFor(cat => cat.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId ID must be greater than 0.");



        }


    }
}
