using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;

public class ProbeFaultedActivity : IStateMachineActivity<ProbeSaga, JobFaulted>
{
    private readonly ApplicationDbContext _database;

    public ProbeFaultedActivity(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void Probe(ProbeContext context)
    { 
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<ProbeFaultedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    { 
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ProbeSaga, JobFaulted> context, IBehavior<ProbeSaga, JobFaulted> next)
    {
        var probe = await _database.Probes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        probe.Status = ProbeStatus.Failed;
        probe.FailedAt = context.Message.Timestamp;
        probe.FailedReason = context.Message.Exceptions.Message;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ProbeSaga, JobFaulted, TException> context, IBehavior<ProbeSaga, JobFaulted> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}