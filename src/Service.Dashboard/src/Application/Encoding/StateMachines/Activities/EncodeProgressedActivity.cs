using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines.Activities;

public class EncodeProgressedActivity : IStateMachineActivity<EncodeSaga, EncodeFileProgressed>
{
    private readonly ApplicationDbContext _database;

    public EncodeProgressedActivity(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeCompletedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncodeSaga, EncodeFileProgressed> context, IBehavior<EncodeSaga, EncodeFileProgressed> next)
    {
        var encode = await _database.Encodes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        encode.Status = EncodeStatus.Processing;
        encode.Percent = context.Message.Percent;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EncodeSaga, EncodeFileProgressed, TException> context, IBehavior<EncodeSaga, EncodeFileProgressed> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}