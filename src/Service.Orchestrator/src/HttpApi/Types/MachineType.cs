using Giantnodes.Service.Orchestrator.Domain.Values;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

public class MachineType : ObjectType<Machine>
{
    protected override void Configure(IObjectTypeDescriptor<Machine> descriptor)
    {
        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.UserName);

        descriptor
            .Field(x => x.ProcessorType);
    }
}