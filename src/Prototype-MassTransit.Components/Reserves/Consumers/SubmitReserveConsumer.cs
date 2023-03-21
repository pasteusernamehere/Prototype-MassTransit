using MassTransit;
using Microsoft.Extensions.Logging;
using Prototype_MassTransit.Contracts.Reserves;

namespace Prototype_MassTransit.Components.Reserves.Consumers;

public class SubmitReserveConsumer : IConsumer<ISubmitReserve>
{
    private readonly ILogger<SubmitReserveConsumer> _logger;

    public SubmitReserveConsumer(ILogger<SubmitReserveConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<ISubmitReserve> context)
    {
        _logger.Log(LogLevel.Debug, "{Consumer}: {Id}", nameof(SubmitReserveConsumer),
            context.Message.Id);

        await context.Publish<ISubmittedReserve>(new
        {
            context.Message.Id,
            context.Message.PlanningCode,
            context.Message.ResourceCode,
            context.Message.Duration,
            context.Message.Effort
        });
    }
}