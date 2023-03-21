using MassTransit;

namespace Prototype_MassTransit.Components.Reserves.StateMachines;

public class ReserveState : SagaStateMachineInstance, ISagaVersion
{
    public string CurrentState { get; set; }
    public string PlanningCode { get; set; } = string.Empty;
    public string ResourceCode { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Effort { get; set; }
    public DateTime Timestamp { get; set; }
    public int Version { get; set; }
    public Guid CorrelationId { get; set; }
}