using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;

public class ProbeStartedActivity : IStateMachineActivity<ProbeSaga, JobStarted>
{
    private readonly ApplicationDbContext _database;

    public ProbeStartedActivity(ApplicationDbContext database)
    {
        _database = database;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<ProbeStartedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ProbeSaga, JobStarted> context, IBehavior<ProbeSaga, JobStarted> next)
    {
        var probe = await _database.Probes.SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        probe.Status = ProbeStatus.Processing;
        probe.StartedAt = context.Message.Timestamp;

        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ProbeSaga, JobStarted, TException> context, IBehavior<ProbeSaga, JobStarted> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}