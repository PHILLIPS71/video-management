using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines.Activities;

public class EncodeFaultedActivity: IStateMachineActivity<EncodeSaga, JobFaulted>
{
    private readonly ApplicationDbContext _database;

    public EncodeFaultedActivity(ApplicationDbContext database)
    {
        _database = database;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeFaultedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncodeSaga, JobFaulted> context, IBehavior<EncodeSaga, JobFaulted> next)
    {
        var encode = await _database.Encodes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        encode.Status = EncodeStatus.Failed;
        encode.FailedAt = context.Message.Timestamp;
        encode.FailedReason = context.Message.Exceptions.Message;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EncodeSaga, JobFaulted, TException> context, IBehavior<EncodeSaga, JobFaulted> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}