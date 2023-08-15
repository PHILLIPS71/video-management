using Giantnodes.Infrastructure.Faults.Types;

namespace Giantnodes.Infrastructure.Validation.Exceptions;

public class ValidationException : Exception
{
    public ValidationFault Fault { get; }

    public ValidationException(ValidationFault fault)
        : base(fault.Message)
    {
        Fault = fault;
    }
}