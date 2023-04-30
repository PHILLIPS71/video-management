using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;

public class ProbeCancelledActivity: IStateMachineActivity<ProbeSaga, JobCanceled>
{
    private readonly ApplicationDbContext _database;

    public ProbeCancelledActivity(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<ProbeCancelledActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ProbeSaga, JobCanceled> context, IBehavior<ProbeSaga, JobCanceled> next)
    {
        var probe = await _database.Probes.SingleAsync(x => x.Id == context.Saga.CorrelationId, CancellationToken.None);
        probe.Status = ProbeStatus.Cancelled;
        probe.CancelledAt = context.Message.Timestamp;
        
        context.Saga.CancelledAt = context.Message.Timestamp;

        await _database.SaveChangesAsync(CancellationToken.None);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ProbeSaga, JobCanceled, TException> context, IBehavior<ProbeSaga, JobCanceled> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}