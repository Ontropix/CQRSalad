using System;
using FluentValidation.Validators;

namespace CQRSalad.Infrastructure.Validation
{
    internal class UriStringValidator : PropertyValidator
    {
        private readonly UriKind _kind;

        public UriStringValidator(UriKind kind = UriKind.RelativeOrAbsolute)
            : base("{PropertyName}' is not a valid Uri.")
        {
            _kind = kind;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            string value = context.PropertyValue as string;
            return value != null && Uri.IsWellFormedUriString(value, _kind);
        }
    }
}