using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.EncodeProfiles.Commands;

public class EncodeProfileUpdateConsumer : IConsumer<EncodeProfileUpdate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeProfileRepository _repository;

    public EncodeProfileUpdateConsumer(IUnitOfWorkService uow, IEncodeProfileRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<EncodeProfileUpdate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var profile = await _repository
            .SingleOrDefaultAsync(x => x.Id == context.Message.Id, context.CancellationToken);

        if (profile == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Id));
            return;
        }

        VideoFileContainer? container = null;
        if (context.Message.Container.HasValue)
        {
            container = Enumeration.TryParse<VideoFileContainer>(context.Message.Container.Value);
            if (container == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Container));
                return;
            }
        }

        var codec = Enumeration.TryParse<EncodeCodec>(context.Message.Codec);
        if (codec == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Codec));
            return;
        }

        var preset = Enumeration.TryParse<EncodePreset>(context.Message.Preset);
        if (preset == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Preset));
            return;
        }

        EncodeTune? tune = null;
        if (context.Message.Tune.HasValue)
        {
            tune = Enumeration.TryParse<EncodeTune>(context.Message.Tune.Value);
            if (tune == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Tune));
                return;
            }
        }

        profile.SetName(context.Message.Name);
        profile.SetContainer(container);
        profile.SetCodec(codec);
        profile.SetPreset(preset);
        profile.SetTune(tune);
        profile.SetQuality(context.Message.Quality);

        try
        {
            await uow.CommitAsync(context.CancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.InnerException is PostgresException pg)
        {
            var param = pg.ConstraintName switch
            {
                "ix_encode_profiles_name" => nameof(context.Message.Name),
                _ => null
            };

            await context.RejectAsync(FaultKind.Constraint, param);
            return;
        }

        await context.RespondAsync(new EncodeProfileUpdate.Result { Id = profile.Id });
    }
}