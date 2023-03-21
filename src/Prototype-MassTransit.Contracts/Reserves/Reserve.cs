namespace Prototype_MassTransit.Contracts.Reserves;

public class Reserve
{
    public Guid Id { get; set; }
    public string PlanningCode { get; set; } = string.Empty;
    public string ResourceCode { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Effort { get; set; }
}