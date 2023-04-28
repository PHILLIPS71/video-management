using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;

namespace Giantnodes.Infrastructure.MassTransit.Validation.Exceptions
{
    public class ValidationException : DomainException<ValidationFault>
    {
        public ValidationException(ValidationFault error)
            : base(error, "Validation failed. See errors property for details.")
        {
        }
    }
}
