﻿namespace Giantnodes.Infrastructure.Faults;

public class FaultKind : Enumeration
{
    public static readonly FaultKind Validation =
        new(100, FaultType.InvalidRequest, "validation_fault", "one or more validation issues occurred");

    public static readonly FaultKind Constraint =
        new(101, FaultType.InvalidRequest, "constraint_fault", "duplicate value violates a unique constraint");

    public FaultKind(int id, FaultType type, string code, string message)
        : base(id, code)
    {
        Type = type;
        Code = code;
        Message = message;
    }

    /// <summary>
    /// The type of fault that can occur.
    /// </summary>
    public FaultType Type { get; private set; }

    /// <summary>
    /// The unique code of the kind of fault.
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// The information about the kind of fault.
    /// </summary>
    public string Message { get; private set; }
}