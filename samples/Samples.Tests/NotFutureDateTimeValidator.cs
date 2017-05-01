using System;
using FluentValidation.Validators;

namespace CQRSalad.Infrastructure.Validation
{
    internal class NotFutureDateTimeValidator : PropertyValidator
    {
        public NotFutureDateTimeValidator()
            : base("{PropertyName}' be less than or equal to DateTime.UtcNow.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            DateTime now = DateTime.UtcNow;
            DateTime? value = context.PropertyValue as DateTime?;

            return value.HasValue && value.Value <= now;
        }
    }
}