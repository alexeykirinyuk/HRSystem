using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRSystem.Common.Errors;
using HRSystem.CQRS.Infrastructure.Interfaces;
using HRSystem.CQRS.Infrastructure.Validation;

namespace HRSystem.CQRS.Infrastructure.Implementations
{
    public abstract class CommandHandlerBase<TDefinition> : ICommandHandler<TDefinition>
        where TDefinition : ICommandDefinition<TDefinition>
    {
        private readonly IServiceProvider _serviceProvider;

        protected CommandHandlerBase(
            IServiceProvider serviceProvider)
        {
            ArgumentValidator.EnsureNotNull(nameof(serviceProvider), serviceProvider);

            _serviceProvider = serviceProvider;
        }

        public void Handle(TDefinition parameters)
        {
            ArgumentValidator.EnsureNotNull(nameof(parameters), parameters);

            CheckSecurity(parameters);
            ValidateDataAnnotations(parameters);
            ValidateParameters(parameters);
            HandleImpl(parameters);
        }

        protected virtual void CheckSecurity(TDefinition parameters)
        {
        }

        protected abstract void HandleImpl(TDefinition parameters);

        protected virtual void ValidateParameters(TDefinition parameters)
        {
        }

        private void ValidateDataAnnotations(TDefinition parameters)
        {
            var validationErrors = new List<ValidationResult>();
            var isValid =
                DataAnnotationsValidator.TryValidateObjectRecursive(parameters, validationErrors, _serviceProvider);

            if (!isValid)
            {
                throw new ValidationException("Parameters are invalid");
            }
        }
    }
}