using FluentValidation;
using Ordering.Application.Ordering.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            //RuleFor(o => o.UpdateOrderDto.Id)
            //  .NotEmpty().WithMessage("{PropertyName} is required")
            //  .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
            //RuleFor(o => o.UpdateOrderDto.UserName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .MaximumLength(70).WithMessage("{PropertyName} must not exceed 70 chars");
            //RuleFor(o => o.UpdateOrderDto.TotalPrice)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} shouldn't be negative");
            //RuleFor(o => o.UpdateOrderDto.EmailAddress)
            //   .NotEmpty().WithMessage("{PropertyName} is required.")
            //   .EmailAddress().WithMessage("{PropertyName} must be a valid email address.");

            //RuleFor(o => o.UpdateOrderDto.FirstName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.");

            //RuleFor(o => o.UpdateOrderDto.LastName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
