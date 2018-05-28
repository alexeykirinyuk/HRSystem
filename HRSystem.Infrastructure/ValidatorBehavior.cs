using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Global.Validation;
using MediatR;

namespace HRSystem.Infrastructure
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var list = new List<ValidationFailure>();
            foreach (var validator in _validators)
            {
                await validator.Validate(list, request).ConfigureAwait(false);
            }

            if (list.Count != 0)
            {
                throw new ValidationException(list);
            }

            return await next();
        }
    }
}