namespace Prototype_MassTransit.Contracts.Reserves;

public interface ISubmittedReserve
{
    public Guid Id { get; }
    public string PlanningCode { get; }
    public string ResourceCode { get; }
    public int Duration { get; }
    public int Effort { get; }
}