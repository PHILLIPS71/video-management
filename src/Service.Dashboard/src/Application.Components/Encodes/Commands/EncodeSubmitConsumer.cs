using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Commands;

public class EncodeSubmitConsumer : IConsumer<EncodeSubmit.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemEntryRepository _entries;
    private readonly IEncodeProfileRepository _profiles;

    public EncodeSubmitConsumer(
        IUnitOfWorkService uow,
        IFileSystemEntryRepository entries,
        IEncodeProfileRepository profiles)
    {
        _uow = uow;
        _entries = entries;
        _profiles = profiles;
    }

    public async Task Consume(ConsumeContext<EncodeSubmit.Command> context)
    {
        var entries = await _entries.ToListAsync(x => context.Message.Entries.Contains(x.Id));
        if (entries.Count == 0)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Entries));
            return;
        }

        var profile = await _profiles.SingleOrDefaultAsync(x => x.Id == context.Message.EncodeProfileId);
        if (profile == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.EncodeProfileId));
            return;
        }

        var files = new List<FileSystemFile>();
        foreach (var entry in entries)
        {
            switch (entry)
            {
                case FileSystemFile file:
                    files.Add(file);
                    break;

                case FileSystemDirectory directory:
                    files.AddRange(directory.Entries.OfType<FileSystemFile>());
                    break;
            }
        }

        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encodes = files
                .Select(file => file.Encode(profile))
                .ToList();

            await uow.CommitAsync(context.CancellationToken);

            var ids = encodes.Select(x => x.Id).ToArray();
            await context.RespondAsync(new EncodeSubmit.Result { Encodes = ids });
        }
    }
}