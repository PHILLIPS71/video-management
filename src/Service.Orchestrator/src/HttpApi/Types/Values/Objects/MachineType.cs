using Giantnodes.Service.Orchestrator.Domain.Values;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Values.Objects;

[ObjectType<Machine>]
public static partial class MachineType
{
    static partial void Configure(IObjectTypeDescriptor<Machine> descriptor)
    {
        descriptor
            .Field(x => x.Name);

        descriptor
            .Field(x => x.UserName);

        descriptor
            .Field(x => x.ProcessorType);
    }
}