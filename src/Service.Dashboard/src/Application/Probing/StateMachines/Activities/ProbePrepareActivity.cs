using Giantnodes.Service.Dashboard.Application.Contracts.Probing.Events;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;

public class ProbePrepareActivity : IStateMachineActivity<ProbeSaga, ProbeSubmittedEvent>
{
    private readonly ApplicationDbContext _database;
    private readonly IRequestClient<ProbeFileSystem> _request;

    public ProbePrepareActivity(ApplicationDbContext database, IRequestClient<ProbeFileSystem> request)
    {
        _database = database;
        _request = request;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<ProbePrepareActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    { 
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ProbeSaga, ProbeSubmittedEvent> context, IBehavior<ProbeSaga, ProbeSubmittedEvent> next)
    {
        var response = await _request.GetResponse<JobSubmissionAccepted>(new { context.Saga.FullPath });
        context.Saga.JobId = response.Message.JobId;
        
        var probe = await _database
            .Probes
            .SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);
        
        probe.Status = ProbeStatus.Submitted;
        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ProbeSaga, ProbeSubmittedEvent, TException> context, IBehavior<ProbeSaga, ProbeSubmittedEvent> next) 
        where TException : Exception
    {
        return next.Faulted(context);
    }
}