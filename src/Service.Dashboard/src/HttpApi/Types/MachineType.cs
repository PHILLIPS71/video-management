using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Types;

public class MachineType : ObjectType<Machine>
{
    protected override void Configure(IObjectTypeDescriptor<Machine> descriptor)
    {
        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.UserName);
    }
}