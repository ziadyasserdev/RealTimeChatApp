using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> _validators)
        {
            this._validators = _validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var Context = new ValidationContext<TRequest>(request);
            var Errors = _validators
                .Select(v => v.Validate(Context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();
            if (Errors.Any())
                throw new ValidationException(Errors);
            return await next();
        }
    }

}
