using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Contracts;

namespace Giantnodes.Infrastructure.Validation.Exceptions;

public class ValidationException : Exception
{
    public Guid? RequestId { get; }

    public FaultType Type { get; }

    public string Code { get; }

    public DateTime TimeStamp { get; }

    public IEnumerable<InvalidValidationProperty> Parameters { get; }

    public ValidationException(ValidationFault fault)
        : base(fault.Message)
    {
        RequestId = fault.RequestId;
        Type = fault.Type;
        Code = fault.Code;
        TimeStamp = fault.TimeStamp;
        Parameters = fault.Properties;
    }
}