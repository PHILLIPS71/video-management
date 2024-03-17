using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.EncodeProfiles.Commands;

public class EncodeProfileCreateConsumer : IConsumer<EncodeProfileCreate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeProfileRepository _repository;

    public EncodeProfileCreateConsumer(IUnitOfWorkService uow, IEncodeProfileRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<EncodeProfileCreate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

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

        var profile = new EncodeProfile(
            context.Message.Name,
            container,
            codec,
            preset,
            tune,
            context.Message.Quality);

        try
        {
            _repository.Create(profile);
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

        await context.RespondAsync(new EncodeProfileCreate.Result { Id = profile.Id });
    }
}