using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;

public class ProbeCompletedActivity : IStateMachineActivity<ProbeSaga, JobCompleted>
{
    private readonly ApplicationDbContext _database;

    public ProbeCompletedActivity(ApplicationDbContext database)
    {
        _database = database;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<ProbeCompletedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ProbeSaga, JobCompleted> context, IBehavior<ProbeSaga, JobCompleted> next)
    {
        var probe = await _database.Probes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        probe.Status = ProbeStatus.Completed;
        probe.CompletedAt = context.Message.Timestamp;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ProbeSaga, JobCompleted, TException> context, IBehavior<ProbeSaga, JobCompleted> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}