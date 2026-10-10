using FluentValidation;
using Ordering.Application.Ordering.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.CreateOrderDto.UserName)
              .NotEmpty().WithMessage("{PropertyName} is required.")
              .MaximumLength(70).WithMessage("{PropertyName} must not exceed 70 chars");
            RuleFor(o => o.CreateOrderDto.TotalPrice)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} shouldn't be negative");
            RuleFor(o => o.CreateOrderDto.EmailAddress)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .EmailAddress().WithMessage("{PropertyName} must be a valid email address.");

            //RuleFor(o => o.CreateOrderDto.FirstName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.");

            //RuleFor(o => o.CreateOrderDto.LastName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.");

            //RuleFor(o => o.CreateOrderDto.CardNumber)
            //    .CreditCard().When(o => !string.IsNullOrEmpty(o.CreateOrderDto.CardNumber))
            //    .WithMessage("{PropertyName} must be a valid credit card number.");

            //RuleFor(o => o.CreateOrderDto.Expiration)
            //    .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$") // MM/YY format
            //    .When(o => !string.IsNullOrEmpty(o.CreateOrderDto.Expiration))
            //    .WithMessage("{PropertyName} must be in MM/YY format.");

            //RuleFor(o => o.CreateOrderDto.Cvv)
            //    .Matches(@"^\d{3,4}$")
            //    .When(o => !string.IsNullOrEmpty(o.CreateOrderDto.Cvv))
            //    .WithMessage("{PropertyName} must be 3 or 4 digits.");
        }
    }
}
