using CoursePlus.Shared.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Infrastructure
{
    public class UserValidator : AbstractValidator<CustomUser>
    {
        public UserValidator()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("You must enter an email address");
            RuleFor(p => p.Email).EmailAddress().WithMessage("You must provide a valid email address");
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("You must enter a firstname");
            RuleFor(p => p.FirstName).MaximumLength(50).WithMessage("FirstName cannot be longer than 50 characters");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("You must enter a lastname");
            RuleFor(p => p.LastName).MaximumLength(50).WithMessage("Lastname cannot be longer than 50 characters");
        }
    }
}
