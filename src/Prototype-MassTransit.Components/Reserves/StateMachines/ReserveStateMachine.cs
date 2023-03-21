using MassTransit;
using Microsoft.Extensions.Logging;
using Prototype_MassTransit.Contracts;
using Prototype_MassTransit.Contracts.Reserves;

namespace Prototype_MassTransit.Components.Reserves.StateMachines;

public class ReserveStateMachine : MassTransitStateMachine<ReserveState>
{
    static ReserveStateMachine()
    {
        //Initialize message contracts to ensure message correlation is configured correctly
        MessageContracts.Initialize();
    }

    public ReserveStateMachine(ILogger<ReserveStateMachine> logger)
    {
        InstanceState(x => x.CurrentState);

        Initially(When(SubmitReserveEvent)
            .CopyDataToSagaInstance()
            .Then(x => logger.LogInformation("Reserve saga transitioning to: {State}", nameof(SubmittedState)))
            .Then(x => logger.LogInformation("Reserve.Id: {Id}", x.Message.Id))
            .Then(x => logger.LogInformation("Saga.CorrelationId: {Id}", x.Saga.CorrelationId))
            // .PublishAsync(x => x.Init<ISubmittedReserve>(new
            //     { x.Message.Id, x.Message.Duration, x.Message.Effort, x.Message.ResourceCode, x.Message.PlanningCode }))
            .TransitionTo(SubmittedState));

        During(SubmittedState, When(SubmittedReserveEvent)
            .Then(x => logger.LogInformation("Reserve saga transitioning to: {State}", nameof(AcceptedState)))
            .Then(x => logger.LogInformation("Reserve.Id: {Id}", x.Message.Id))
            .Then(x => logger.LogInformation("Saga.CorrelationId: {Id}", x.Saga.CorrelationId))
            .TransitionTo(AcceptedState));
    }

    public Event<ISubmitReserve> SubmitReserveEvent { get; }
    public Event<ISubmittedReserve> SubmittedReserveEvent { get; }

    public State AcceptedState { get; }
    public State SubmittedState { get; }
}