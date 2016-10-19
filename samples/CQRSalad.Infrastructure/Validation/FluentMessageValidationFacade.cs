using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Domain;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace CQRSalad.Infrastructure.Validation
{
    public class FluentMessageValidationFacade : IMessageValidationFacade
    {
        private readonly FluentValidatorsRegistry _registry;

        public FluentMessageValidationFacade(FluentValidatorsRegistry registry)
        {
            _registry = registry;
        }

        public bool IsValid<TMessage>(TMessage instance) where TMessage : class
        {
            return _registry.CreateValidators(typeof(TMessage)).All(validator => validator.Validate(instance).IsValid);
        }

        public void Validate<TMessage>(TMessage instance) where TMessage : class
        {
            List<IValidator> validators = _registry.CreateValidators(typeof(TMessage));
            validators.ForEach(validator =>
            {
                ValidationResult result = validator.Validate(instance);

                if (result.IsValid)
                {
                    return;
                }

                string validationErrors = String.Join("", result.Errors.Select(x => "\r\n" + x.ErrorMessage).ToArray());
                string errorMessage = $"Command validation failed: {validationErrors}";

                throw new InvalidCommandException<TMessage>(errorMessage, instance);
            });
        }
    }

    public class FluentValidatorFor<TMessage> : AbstractValidator<TMessage> where TMessage : class
    {
        public FluentValidatorFor()
        {
        }
    }

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

    public sealed class FluentValidatorsRegistry
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, List<Type>> _validatorsMap;

        public FluentValidatorsRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _validatorsMap = new Dictionary<Type, List<Type>>();
        }

        /// <summary>
        /// Register all validators in assembly
        /// </summary>
        public void Register(params Assembly[] assemblies)
        {
            var interfaceMarker = typeof(IValidator<>);

            // We scan assemblies for all types belong to ScanRule namespaces
            IEnumerable<Type> types = assemblies.SelectMany(assembly => assembly.GetExportedTypes());

            foreach (Type type in types)
            {
                // We filter interfaces by type and generic characteristics
                List<Type> validatorTypes = type.GetInterfaces()
                                                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceMarker)
                                                .ToList();

                foreach (Type validatorType in validatorTypes)
                {
                    Type validationTarget = validatorType.GetGenericArguments().First();
                    AddValidator(validationTarget, type);
                }
            }
        }

        public List<IValidator> CreateValidators(Type targetType)
        {
            return GetValidators(targetType).Select(validator => (IValidator)_serviceProvider.GetService(validator)).ToList();
        }

        private IEnumerable<Type> GetValidators(Type validationTargetType)
        {
            if (!_validatorsMap.ContainsKey(validationTargetType))
            {
                return new List<Type>();
            }

            return _validatorsMap[validationTargetType];
        }

        private void AddValidator(Type validationTargetType, Type validatorType)
        {
            if (!_validatorsMap.ContainsKey(validationTargetType))
            {
                _validatorsMap[validationTargetType] = new List<Type>();
            }

            if (!_validatorsMap[validationTargetType].Contains(validatorType))
            {
                _validatorsMap[validationTargetType].Add(validatorType);
            }
        }
    }
}