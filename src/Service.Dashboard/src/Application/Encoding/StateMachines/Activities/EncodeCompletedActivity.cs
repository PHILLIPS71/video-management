using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines.Activities;

public class EncodeCompletedActivity : IStateMachineActivity<EncodeSaga, JobCompleted>
{
    private readonly ApplicationDbContext _database;

    public EncodeCompletedActivity(ApplicationDbContext database)
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

    public async Task Execute(BehaviorContext<EncodeSaga, JobCompleted> context, IBehavior<EncodeSaga, JobCompleted> next)
    {
        var encode = await _database.Encodes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        encode.Status = EncodeStatus.Completed;
        encode.CompletedAt = context.Message.Timestamp;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EncodeSaga, JobCompleted, TException> context, IBehavior<EncodeSaga, JobCompleted> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}