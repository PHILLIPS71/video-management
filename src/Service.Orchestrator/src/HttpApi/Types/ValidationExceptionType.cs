using Giantnodes.Infrastructure.Validation.Exceptions;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

public class ValidationExceptionType : ObjectType<ValidationException>
{
    protected override void Configure(IObjectTypeDescriptor<ValidationException> descriptor)
    {
        descriptor
            .Field("request_id")
            .Resolve(p => p.Parent<ValidationException>().Fault.RequestId);

        descriptor
            .Field("type")
            .Resolve(p => p.Parent<ValidationException>().Fault.Type);

        descriptor
            .Field("code")
            .Resolve(p => p.Parent<ValidationException>().Fault.Code);

        descriptor
            .Field("message")
            .Resolve(p => p.Parent<ValidationException>().Fault.Message);

        descriptor
            .Field("time_stamp")
            .Resolve(p => p.Parent<ValidationException>().Fault.TimeStamp);
    }
}