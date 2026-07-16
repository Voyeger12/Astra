using Astra.Application.Observability;

namespace Astra.Application.Agents;

public sealed record AstraAgentTextDeltaEvent : AstraAgentEvent
{
    public AstraAgentTextDeltaEvent(
        OperationId operationId,
        AgentRunId agentRunId,
        DateTimeOffset occurredAt,
        ProviderCallId providerCallId,
        string delta)
        : base(operationId, agentRunId, occurredAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(delta);
        ProviderCallId = providerCallId;
        Delta = delta;
    }

    public ProviderCallId ProviderCallId { get; }
    public string Delta { get; }
}
