using System;
using FluentValidation;

namespace CQRSalad.Infrastructure.Validation
{
    public static class FluentValidatorRuleExtensions
    {
        public static IRuleBuilderOptions<T, DateTime> NotFuture<T>(this IRuleBuilder<T, DateTime> builder)
        {
            return builder.SetValidator(new NotFutureDateTimeValidator());
        }

        public static IRuleBuilderOptions<T, string> IsValidUri<T>(this IRuleBuilder<T, string> builder, UriKind kind = UriKind.RelativeOrAbsolute)
        {
            return builder.SetValidator(new UriStringValidator(kind));
        }
    }
}