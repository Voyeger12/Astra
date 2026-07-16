using Astra.Application.Observability;

namespace Astra.Application.Agents;

public enum AstraAgentRunStatus
{
    Preparing,
    Connecting,
    Generating,
    Cancelling
}

public abstract record AstraAgentEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt)
{
    public bool IsTerminal => this is AstraAgentCompletedEvent or AstraAgentCancelledEvent or AstraAgentFailedEvent;
}
