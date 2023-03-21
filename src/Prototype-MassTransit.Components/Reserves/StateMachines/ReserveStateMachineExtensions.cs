using MassTransit;
using Prototype_MassTransit.Contracts.Reserves;

namespace Prototype_MassTransit.Components.Reserves.StateMachines;

public static class ReserveStateMachineExtensions
{
    public static EventActivityBinder<ReserveState, ISubmitReserve> CopyDataToSagaInstance(
        this EventActivityBinder<ReserveState, ISubmitReserve> binder)
    {
        return binder.Then(x =>
        {
            x.Saga.Duration = x.Message.Duration;
            x.Saga.Effort = x.Message.Effort;
            x.Saga.ResourceCode = x.Message.ResourceCode;
            x.Saga.PlanningCode = x.Message.PlanningCode;
            x.Saga.Timestamp = InVar.Timestamp;
        });
    }
}