using Giantnodes.Infrastructure.Faults.Types;

namespace Giantnodes.Infrastructure.Faults.Exceptions;

public class DomainException : Exception
{
    public DomainFault Fault { get; }

    public DomainException(DomainFault fault)
        : base(fault.Message)
    {
        Fault = fault;
    }
}