using FluentValidation.Results;
using Giantnodes.Infrastructure.MassTransit.Validation.Abstractions;
using Giantnodes.Infrastructure.MassTransit.Validation.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;

namespace Giantnodes.Infrastructure.MassTransit.Validation.Extensions
{
    public static class ValidationResultExtensions
    {
        public static ValidationException ToException(this ValidationResult result)
        {
            return new ValidationException(result.ToFault());
        }

        internal static ValidationFault ToFault(this ValidationResult result)
        {
            var properties = result
                .Errors
                .GroupBy(error => error.PropertyName)
                .Select(group => new InvalidValidationProperty
                {
                    Property = group.Key,
                    Issues = group.Select(error => new InvalidValidationRule { Rule = error.ErrorCode, Reason = error.ErrorMessage }).ToArray()
                })
                .ToArray();

            return new ValidationFault { Properties = properties };
        }
    }
}
