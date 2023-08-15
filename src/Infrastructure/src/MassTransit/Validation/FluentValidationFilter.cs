using FluentValidation;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Contracts;
using MassTransit;

namespace Giantnodes.Infrastructure.Masstransit.Validation;

public class FluentValidationFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
    where TMessage : class
{
    private readonly IValidator<TMessage>? _validator;

    public FluentValidationFilter(IEnumerable<IValidator<TMessage>>? validator)
    {
        _validator = validator?.FirstOrDefault();
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("fluent-validation-filter");
    }

    public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
    {
        if (_validator == null)
        {
            await next.Send(context);
            return;
        }

        var message = context.Message;
        var result = await _validator.ValidateAsync(message, context.CancellationToken);
        if (result.IsValid)
        {
            await next.Send(context);
            return;
        }

        var properties = result
            .Errors
            .GroupBy(error => error.PropertyName)
            .Select(group => new InvalidValidationProperty
            {
                Property = group.Key,
                Issues = group.Select(error => new InvalidValidationRule
                {
                    Rule = error.ErrorCode,
                    Reason = error.ErrorMessage
                }).ToArray()
            })
            .ToArray();

        var fault = FaultKind.Validation;
        await context.RespondAsync(new ValidationFault
        {
            Type = fault.Type,
            RequestId = context.RequestId,
            TimeStamp = InVar.Timestamp,
            Code = fault.Code,
            Message = fault.Message,
            Properties = properties
        });
    }
}