using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;

namespace CQRSalad.Infrastructure.Validation
{
    public class ValidationController
    {
        private readonly IServiceProvider _serviceProvider;

        private static readonly ConcurrentDictionary<Type, HashSet<Type>> _validatorsCache =
            new ConcurrentDictionary<Type, HashSet<Type>>();

        public ValidationController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IEnumerable<Type> GetValidators(Type source)
        {
            if (!_validatorsCache.ContainsKey(source))
            {
                return new List<Type>();
            }

            return _validatorsCache[source].ToList();
        }

        private void AddValidator(Type source, Type validator)
        {
            var validators = _validatorsCache.GetOrAdd(source, key => new HashSet<Type>());
            validators.Add(validator);
        }

        public void RegisterFromAssemblies(params Assembly[] assemblies)
        {
            var publicTypes = assemblies.SelectMany(assembly => assembly.GetExportedTypes());
            foreach (Type type in publicTypes)
            {
                var validators =
                    type.GetInterfaces()
                        .Where(_interface =>
                            _interface.IsGenericType &&
                            _interface.GetGenericTypeDefinition() == typeof (IValidator<>));

                foreach (Type validatorType in validators)
                {
                    Type validationTarget = validatorType.GetGenericArguments().First();
                    AddValidator(source: validationTarget, validator: type);
                }
            }
        }

        public void Validate<TMessage>(TMessage instance) where TMessage : class
        {
            var validators = GetValidators(typeof (TMessage))
                .Select(x => _serviceProvider.GetService(x))
                .Cast<IValidator>();

            var errors = validators
                .Select(x => x.Validate(instance))
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            if (errors.Count > 0)
            {
                string errorMessage = String.Join("\r\n", errors);
                throw new InvalidMessageException<TMessage>(errorMessage, instance);
            }
        }
    }
}