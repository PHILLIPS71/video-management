using Giantnodes.Infrastructure.Faults.Exceptions;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class DomainExceptionType : ObjectType<DomainException>
{
    protected override void Configure(IObjectTypeDescriptor<DomainException> descriptor)
    {
        descriptor
            .Field("request_id")
            .Resolve(p => p.Parent<DomainException>().Fault.RequestId);

        descriptor
            .Field("type")
            .Resolve(p => p.Parent<DomainException>().Fault.Type);

        descriptor
            .Field("code")
            .Resolve(p => p.Parent<DomainException>().Fault.Code);

        descriptor
            .Field("message")
            .Resolve(p => p.Parent<DomainException>().Fault.Message);

        descriptor
            .Field("time_stamp")
            .Resolve(p => p.Parent<DomainException>().Fault.TimeStamp);

        descriptor
            .Field("property")
            .Resolve(p => p.Parent<DomainException>().Fault.Property);
    }
}